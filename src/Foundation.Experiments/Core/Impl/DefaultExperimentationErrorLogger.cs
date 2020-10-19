using EPiServer.Logging;
using EPiServer.ServiceLocation;
using OptimizelySDK.Logger;
using ILogger = OptimizelySDK.Logger.ILogger;

namespace Foundation.Experiments.Core.Impl
{
    public class DefaultExperimentationErrorLogger : ILogger
    {
        private readonly EPiServer.Logging.ILogger _logger;

        public DefaultExperimentationErrorLogger()
        {
            ServiceLocator.Current.TryGetExistingInstance(out EPiServer.Logging.ILogger epiErrorLogger);
            _logger = epiErrorLogger;
        }

        public void Log(LogLevel level, string message)
        {
            if (_logger == null)
                return;

            if (level == LogLevel.ERROR)
                _logger.Log(Level.Error, message);
            else if (level == LogLevel.WARN)
                _logger.Log(Level.Warning, message);
            else if (level == LogLevel.INFO)
                _logger.Log(Level.Information, message);
            else
                _logger.Log(Level.Debug, message);
        }
    }
}
