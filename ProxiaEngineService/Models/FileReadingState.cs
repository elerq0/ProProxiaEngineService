using Microsoft.Win32;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using ProxiaEngineService.Models.FileTypeModels;
using System.IO;

namespace ProxiaEngineService.Models
{
    public enum FilenameFormat
    {
        Long,
        Short
    }

    class FileReadingState
    {
        private readonly string _keyname;
        private readonly FilenameFormat _filenameFormat;
        private string _typeString;
        private string _timestampString;
        private string _currentFilePath;
        private int _currentLine;
        private int _currentFileLength;
        private string _regex;
        private IEnumerable<string> _supportedTypes;
        private string _dateTimeFormat =>
         _filenameFormat == FilenameFormat.Short
             ? "yyyyMMddHHmmss"
             : "yyyyMMddHHmmssfff";
        private string[] _currentFile = null;

        public FileReadingState(string registryPath, FilenameFormat filenameFormat, bool clearStoredParam)
        {
            _keyname = registryPath;
            _filenameFormat = filenameFormat;
            
            _typeString = string.Empty;
            _timestampString = string.Empty;

            _regex = _filenameFormat == FilenameFormat.Long
              ? @"^.*\\([^\\0-9]+)([0-9]{17})\.(?:txt|TXT)$"  //WG Corection regex pattern
              : @"^.*\\([^\\0-9]+)([0-9]{14})\.(?:txt|TXT)$";

            _supportedTypes = DocumentBase.GetDeutchNames();

            if (clearStoredParam)
            {
                CurrentFilePath = string.Empty; //set change CurrentLine
            }
            else
            {
                _currentLine = Registry.GetValue(_keyname, nameof(CurrentLine), 0) as int? ?? 0;
                CurrentFilePath = Registry.GetValue(_keyname, nameof(CurrentFilePath), string.Empty) as string ?? string.Empty; //Important order, set canchange CurrentLine
            }
        }

        private bool CheckFileNameTimestamp(string timestamp)
        {
            try
            {
                DateTime dateTime = DateTime.ParseExact(timestamp, DateTimeFormat, CultureInfo.InvariantCulture);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private Tuple<bool,string,string,string> ExtractFileInfoFromFilePath( string filePath )
        {
            Match match = Regex.Match(filePath, _regex);

            return Tuple.Create(
                match.Success && _supportedTypes.Contains(match.Groups[1].Value) && CheckFileNameTimestamp(match.Groups[2].Value),
                filePath,
                match.Groups[1].Value,
                match.Groups[2].Value );
        }

        public string ExtractTimestampStringFromfilePath( string filePath )
        {
            var fileInfo = ExtractFileInfoFromFilePath(filePath);

            if (!fileInfo.Item1)
            {
                return string.Empty;
            }
            else
            {
                return fileInfo.Item4;
            }
        }

        public bool EOFCurrentFile()
        {
            return _currentLine >= _currentFileLength;
        }

        public string GetNextLine()
        {
            string result;

            if (EOFCurrentFile())
                throw new Exception("Try to read past EOF [" + CurrentFileName + "]");

            result = _currentFile[_currentLine];
            CurrentLine++;
            return result;
        }

        public string DateTimeFormat
        {
            get { return _dateTimeFormat; }
        }

        public string TypeString
        {
            get { return _typeString; }
        }

        public string CurrentFileName
        {
            get { return _typeString + _timestampString + ".txt"; }
        }

        public string CurrentFilePath
        {
            get { return _currentFilePath; }
            set
            {
                //TODO how to make it in one transaction????
                CurrentLine = 0;

                var fileInfo = ExtractFileInfoFromFilePath(value);

                if (fileInfo.Item1)
                {
                    _currentFilePath = fileInfo.Item2;
                    _typeString = fileInfo.Item3;
                    _timestampString = fileInfo.Item4;
                    _currentFile = File.ReadAllLines(_currentFilePath);
                    _currentFileLength = _currentFile.Length;
                }
                else
                {
                    _currentFilePath = string.Empty;
                    _typeString = string.Empty;
                    _timestampString = string.Empty;
                    _currentFile = null;
                    _currentFileLength = 0;
                }
                Registry.SetValue(_keyname, nameof(CurrentFilePath), _currentFilePath, RegistryValueKind.String);            
            }
        }

        public int CurrentLine
        {
            get { return _currentLine; }
            set
            {
                Registry.SetValue(_keyname, nameof(CurrentLine), value, RegistryValueKind.DWord);
                _currentLine = value;
            }
        }
    }
}
