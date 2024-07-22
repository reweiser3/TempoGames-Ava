using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Ava.Services
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly object _lock = new object();

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {formatter(state, exception)}{Environment.NewLine}";
            lock (_lock)
            {
                try
                {
                    using (var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.Write))
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(message);
                    }
                }
                catch (IOException ex)
                {
                    // Handle the exception or log it
                    Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
                }
            }
        }
    }
}