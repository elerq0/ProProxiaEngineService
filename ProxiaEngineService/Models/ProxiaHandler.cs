using ProxiaEngineService.Models.FileTypeModels;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProxiaEngineService.Models
{
    class ProxiaHandler
    {
        private const string History = "history.txt";
        private const string Settings = "settings.txt";
        private const string SerializeFile = @"History\serialized.txt";
        private const string DefaultProxia = @"DefaultDir\Proxia";
        private const string DefaultSuccess = @"DefaultDir\Success";
        private const string DefaultFail = @"DefaultDir\Fail";
        private readonly bool _isDump;
        private readonly Logger _logger = new Logger(Properties.Settings.Default.LogPath, Properties.Settings.Default.LoggingMode == "Normal" ? LoggingMode.Normal : LoggingMode.Enhanced);
        

        private readonly string _proxiaPath;
        private readonly string _successPath;
        private readonly string _failPath;

        private FileSystemWatcher _watcher;

        public ProxiaHandler(string proxiaPath, string successPath, string failPath)
        {
            if (!File.Exists(Settings))
            {
                using (File.Create(Settings))
                { }
                _isDump = false;
            }

            using(var reader = new StreamReader(Settings, true))
            {
                string settingsData = reader.ReadToEndAsync().Result;
                var tempValues = new[] { "true", "True", "true\r\n", "True\r\n" };
                _isDump = tempValues.Contains(settingsData);
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; //new CultureInfo("en-US");
            _proxiaPath = proxiaPath == string.Empty ? DefaultProxia : proxiaPath;
            _successPath = successPath == string.Empty ? DefaultSuccess : successPath;
            _failPath = failPath == string.Empty ? DefaultFail : failPath;

            if (!Directory.Exists(_proxiaPath))
                Directory.CreateDirectory(_proxiaPath);
            if (!Directory.Exists(_successPath))
                Directory.CreateDirectory(_successPath);
            if (!Directory.Exists(_failPath))
                Directory.CreateDirectory(_failPath);

            _watcher = new FileSystemWatcher(_proxiaPath)
            {
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.*"
            };
            _watcher.Changed += WatcherChanged;
            _watcher.EnableRaisingEvents = true;
        }

        private void WatcherChanged(object sender, FileSystemEventArgs args)
        {
            Thread.Sleep(10);
            string[] files = Directory.GetFiles(_proxiaPath);

            foreach(var filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string fileType = Regex.Replace(fileName, "[0-9]", string.Empty);
                string fileDate = Regex.Replace(fileName, "[^0-9]", string.Empty);
                if (!CheckDate(fileDate)) continue;
                string fileNameWithExtention = Path.GetFileName(filePath);
                var document = DocumentBase.Translate(fileType);
                //TODO: if fileType is incorrect then it should move the file to FAIL folder
                string[] lines;
                Encoding encoding;

                using (var reader = new StreamReader(filePath))
                {
                    reader.Peek();
                    encoding = reader.CurrentEncoding;
                    lines = File.ReadAllLines(filePath, encoding);
                }

                var isOk = true;
                string errorText = string.Empty;

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    try
                    {
                        document.ReadFromProxiaFormat(line);
                        string res = document.ToProxiaFormat();
                        if (res != line)
                            throw new Exception("Strange error with convertion to proxia format");

                        if (_isDump)
                        {
                            using(var writer = new StreamWriter(SerializeFile, true))
                            {
                                writer.WriteLine("Readed file line {0}:", i);
                                writer.WriteLine(res);
                                writer.WriteLine("--------------------------------------------");
                                writer.WriteLine();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        isOk = false;
                        string str = $"Error in {i} line";
                        Exception exc = new Exception(str, e);
                        errorText = exc.ToString();
                    }
                }

                if (!File.Exists(History))
                {
                    using (var stream = File.Create(History))
                    { }
                }

                string message;
                if (isOk)
                {
                    message = $"Successfully read {fileNameWithExtention} file";
                    Log(message);
                    if(encoding == Encoding.Unicode)
                        File.Move(filePath, _successPath + "\\" + fileNameWithExtention);
                    else
                    {
                        using (var stream = File.Create(_successPath + "\\" + fileNameWithExtention))
                        { }
                        File.WriteAllLines(_successPath + "\\" + fileNameWithExtention, lines, Encoding.Unicode);
                        File.Delete(filePath);
                    }
                }
                else
                {
                    message = $"Failed read of {fileNameWithExtention} file because of exception : \r\n {errorText}";
                    Log(message);
                    if(encoding == Encoding.Unicode)
                        File.Move(filePath, _failPath + "\\" + fileNameWithExtention);
                    else
                    {
                        using (var stream = File.Create(_failPath + "\\" + fileNameWithExtention))
                        { }
                        File.WriteAllLines(_failPath + "\\" + fileNameWithExtention, lines, Encoding.Unicode);
                        File.Delete(filePath);
                    }
                }
            }
        }

        #region Helpers

        private static bool CheckDate(string dateString)
        {
            try
            {
                var format = dateString.Length == 14 ? "yyyyMMddHHmmss" : "yyyyMMddHHmmssfff";

                var dateTime = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static readonly string endingLine = string.Concat(Enumerable.Repeat("-", 46));
        private void Log(string message)
        {
            using (var writer = new StreamWriter(History, true))
            {
                _logger.Log(message);
            }
        }


        #endregion
    }
}