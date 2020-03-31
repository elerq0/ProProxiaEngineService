using System;
using System.IO;

namespace ProxiaEngineService
{
    public class Logger
    {
        private readonly string _logFilePath;
        private readonly LoggingMode _mode;

        public Logger(string logFilePath, LoggingMode mode)
        {
            _logFilePath = logFilePath;
            _mode = mode;
        }


        /// <summary>
        /// Adds raw input to log file
        /// </summary>
        /// <param name="data">text to log</param>
        private void Write(string message) => File.AppendAllText(_logFilePath, message);

        /// <summary>
        /// Adds raw input to log file, appending new line at the end
        /// </summary>
        /// <param name="data">text to log</param>
        private void WriteLine(string message) => Write($"{message}\n");

        /// <summary>
        /// Log input text to log file
        /// </summary>
        /// <param name="data">text to log</param>
        public void Log(string message, LoggingMode mode = LoggingMode.Normal)
        {
            if (_mode < mode)
                return;
            
            WriteLine($"{DateTime.Now}\t{message}");
        }
    }

    public enum LoggingMode
    {
        Normal,
        Enhanced
    }
}
