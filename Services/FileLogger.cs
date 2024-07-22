using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Ava.Services
{
    /// <summary>
    /// A custom logger that logs messages to a file.
    /// </summary>
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        /// <param name="filePath">The path to the log file.</param>
        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>A disposable object that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state) => null;

        /// <summary>
        /// Checks if the given log level is enabled.
        /// </summary>
        /// <param name="logLevel">The log level to check.</param>
        /// <returns>true if the log level is enabled; otherwise, false.</returns>
        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="logLevel">The log level.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="state">The state to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="formatter">The formatter function to create a log message from the state and exception.</param>
        /// <exception cref="IOException">Thrown when writing to the log file fails.</exception>
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
