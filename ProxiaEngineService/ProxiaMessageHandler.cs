using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ProxiaEngineService.Models;
using ProxiaEngineService.Models.FileTypeModels;

namespace ProxiaEngineService
{
    public class ProxiaMessageHandler
    {
        private readonly string _rootPath;
        private string ReadPath => Path.Combine(_rootPath, "COSCOM");
        private string HistoryPath => Path.Combine(_rootPath, "History/COSCOM");
        private string ErrorPath => Path.Combine(_rootPath, "History/ERROR");
        private string WritePath => Path.Combine(_rootPath, "PPS");
        private DocumentBase _lastMessage;

        private string[] _currentFile = new string[0];
        private readonly FileReadingState _state;
        private readonly Logger _logger;

        public FilenameFormat FilenameFormat { get; }
        public ProxiaMessageError IfError { get; set; } = ProxiaMessageError.ExceptionOnError;

        public ProxiaMessageHandler(string rootPath, FilenameFormat filenameFormat, bool clearStoredParam , Logger logger, ProxiaMessageError ifError) //WG ifError need be init before NextMessage();
        {
            FilenameFormat = filenameFormat;
            _rootPath = rootPath;
            IfError = ifError;
            _logger = logger;

            _state = new FileReadingState(Properties.Settings.Default.RegistryPath, FilenameFormat, clearStoredParam);

            NextMessage();
        }
        
        #region Reading
        // ------------------------------ READING ------------------------------

        /// <summary>
        /// Checks wherether exists unparsed valid document
        /// </summary>
        /// <returns>True, if exists, false overwise</returns>
        public bool IsMessage() => _lastMessage != null;

        public DocumentBase GetMessage() => _lastMessage;

        public void NextMessage()
        {
            do
            {
                while (_state.EOFCurrentFile())
                {
                    if (!string.IsNullOrEmpty(_state.CurrentFilePath))
                    {
                        if (File.Exists(Path.Combine(HistoryPath, _state.CurrentFileName)))
                            File.Delete(Path.Combine(HistoryPath, _state.CurrentFileName));
                        
                        File.Move(Path.Combine(ReadPath, _state.CurrentFileName),
                            Path.Combine(HistoryPath, _state.CurrentFileName));
                    }
                    _state.CurrentFilePath = GetOldestFilePath();
                    if (string.IsNullOrEmpty(_state.CurrentFilePath))
                    {
                        _lastMessage = null;
                        _logger.Log("No matched file found", LoggingMode.Enhanced);
                        return;
                    }
                }

                _lastMessage = Parse(_state.GetNextLine());
            } while (_lastMessage == null);
            _logger.Log(_state.CurrentFileName + "\t" + _state.CurrentLine.ToString("####") + "\t" + "NextMessage", LoggingMode.Enhanced);
        }

        #endregion

        #region Writing

        // ------------------------------ WRITING ------------------------------

        /// <summary>
        /// Writes given document to proper file
        /// </summary>
        /// <param name="document"></param>
        public void WriteMessage(DocumentBase document)
        {
            string content = document.ToProxiaFormat();
            var filename = GetFileName(document.DeutschName);
            File.WriteAllText($"{WritePath}/{filename}", content, (new UTF8Encoding(false)));
        }

        /// <summary>
        /// Writes given list of documents to proper file
        /// </summary>
        /// <param name="document"></param>
        public void WriteMessageList(Logger logger,List<DocumentBase> documents)
        {
            string content = "";
            foreach (var document in documents)
            {
                content += document.ToProxiaFormat() + System.Environment.NewLine;
            } 
            var filename = GetFileName(documents.First().DeutschName);
            logger.Log($"{WritePath}/{filename}", LoggingMode.Enhanced);
            File.WriteAllText($"{WritePath}/{filename}", content, (new UTF8Encoding(false)));
        }

        /// <summary>
        /// Document buffer (for <see cref="AddMessage"/> and <see cref="FlushMessages"/>)
        /// </summary>
        private readonly Dictionary<string, ValuesList<DocumentBase>> _documents =
            new Dictionary<string, ValuesList<DocumentBase>>();

        /// <summary>
        /// Add input document to buffer
        /// </summary>
        /// <param name="document">Document to add</param>
        public void AddMessage(DocumentBase document)
        {

            if (!_documents.ContainsKey(document.DeutschName))
                _documents.Add(document.DeutschName, new ValuesList<DocumentBase>());
        }

        /// <summary>
        /// Writed buffered messages to proper files (one per type)
        /// </summary>
        public void FlushMessages()
        {
            foreach (var pair in _documents)
            {
                var list = pair.Value;
                if (!list.Any()) continue;

                var filename = GetFileName(list.First().DeutschName);

                list.WriteToFile($"{WritePath}/{filename}");
                list.Clear();
            }
        }

        #endregion

        #region Helpers

        // ------------------------------ Helpers ------------------------------

        /// <summary>
        /// Return timestamp for next file as string
        /// </summary>
        /// <returns></returns>
        private string GetFileNameTimestamp() => DateTime.Now.ToString(_state.DateTimeFormat); // can be changed here

        /// <summary>
        /// Convert Proxia-string in valid document class
        /// </summary>
        /// <param name="data">string with data in Proxia format</param>
        /// <returns>Class representation of input on success and null overwise</returns>
        private DocumentBase Parse(string data)
        {
            var doc = DocumentBase.Translate(_state.TypeString);
            try
            {
                doc.ReadFromProxiaFormat(data);
                return doc;
            }
            catch (ProxiaParseException e)
            {
                _logger.Log(_state.CurrentFileName+"\t"+(_state.CurrentLine+1).ToString("####")+"\t"+e.Message);

                File.Copy(Path.Combine(ReadPath, _state.CurrentFileName), Path.Combine(ErrorPath, _state.CurrentFileName), true);

                if (IfError == ProxiaMessageError.ExceptionOnError) throw new ProxiaParseException(e.Message);

                return null;
            }
        }

        private string GetOldestFilePath()
        {
            var files = Directory.GetFiles(ReadPath);
            string filename = string.Empty;
            string timestamp = string.Empty;
            string tempTimestamp;
            
            if (!files.Any()) return string.Empty;

            foreach (string file in files)
            {
                tempTimestamp = _state.ExtractTimestampStringFromfilePath(file);

                if (tempTimestamp != string.Empty &&
                    (timestamp == string.Empty ||
                     String.Compare(tempTimestamp, timestamp, false) < 0) )
                {
                    filename = file;
                    timestamp = tempTimestamp;
                }
            }
            return filename;
        }

        private string GetFileName(string type)
        {
            var timestamp = GetFileNameTimestamp();
            return $"{type}{timestamp}.txt";
        }

        public string GetStateInfo()
        {
            return _state.CurrentFileName + "\t" + _state.CurrentLine.ToString("####") + "\t";
        }

        #endregion

    }

    #region Enums

    internal enum ProxiaMessageType
    {
        NoneMt,
        ReturnStatementMt,
        ProductionPlanMt,
        ProductionOrderMt
    }

    public enum ProxiaMessageError
    {
        ExceptionOnError,
        SkipOnError
    }
    #endregion
}