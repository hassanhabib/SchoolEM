using System;
using Microsoft.Extensions.Logging;

namespace SchoolEM.Brokers.Logging
{
    public class LoggingBroker : ILoggingBroker
    {
        private ILogger logger;

        public LoggingBroker(ILogger logger)
        {
            this.logger = logger;
        }

        public void LogCritical(Exception exception)
        {
            this.logger.LogCritical(exception.Message, exception);
        }

        public void LogDebug(string message)
        {
            this.logger.LogDebug(message);
        }

        public void LogError(Exception exception)
        {
            this.logger.LogError(exception.Message, exception);
        }

        public void LogInformation(string message)
        {
            this.logger.LogInformation(message);
        }

        public void LogTrace(string message)
        {
            this.logger.LogTrace(message);
        }

        public void LogWarning(string message)
        {
            this.logger.LogWarning(message);
        }
    }
}
