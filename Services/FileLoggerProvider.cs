using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Ava.Services
{
    /// <summary>
    /// Provides a logger that writes log messages to a file.
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _logDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLoggerProvider"/> class.
        /// </summary>
        /// <param name="logDirectory">The directory where log files will be stored.</param>
        public FileLoggerProvider(string logDirectory)
        {
            _logDirectory = logDirectory;
            EnsureLogDirectoryExists();
        }

        /// <summary>
        /// Creates a logger that writes to a file.
        /// </summary>
        /// <param name="categoryName">The category name for the logger.</param>
        /// <returns>An instance of <see cref="ILogger"/> that writes to a file.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            var logFileName = $"{DateTime.Now:yyyy-MM-dd}.log";
            var filePath = Path.Combine(_logDirectory, logFileName);
            return new FileLogger(filePath);
        }

        /// <summary>
        /// Disposes resources used by the <see cref="FileLoggerProvider"/>.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Ensures that the log directory exists.
        /// </summary>
        private void EnsureLogDirectoryExists()
        {
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }
    }
}