using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Ava.Services
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _logDirectory;

        public FileLoggerProvider(string logDirectory)
        {
            _logDirectory = logDirectory;
            EnsureLogDirectoryExists();
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logFileName = $"{DateTime.Now:yyyy-MM-dd}.log";
            var filePath = Path.Combine(_logDirectory, logFileName);
            return new FileLogger(filePath);
        }

        public void Dispose() { }

        private void EnsureLogDirectoryExists()
        {
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }
    }
}