using Microsoft.Extensions.Logging;
using System.Extensions;

namespace System.Logging
{
    public struct ScopedLogger<T>
    {
        private readonly ILogger<T> logger;
        private readonly string scope;
        private readonly string flowId;

        private ScopedLogger(ILogger<T> logger, string scope, string flowId)
        {
            this.logger = logger;
            this.scope = scope;
            this.flowId = flowId.IsNullOrWhiteSpace() ? string.Empty : $"[{flowId}] ";
        }

        public ScopedLogger<T> LogInformation(string message, params object[] parameters)
        {
            this.logger.LogInformation(this.CreateMessage(message), parameters);
            return this;
        }

        public ScopedLogger<T> LogDebug(string message, params object[] parameters)
        {
            this.logger.LogDebug(this.CreateMessage(message), parameters);
            return this;
        }

        public ScopedLogger<T> LogWarning(string message, params object[] parameters)
        {
            this.logger.LogWarning(this.CreateMessage(message), parameters);
            return this;
        }

        public ScopedLogger<T> LogError(string message, params object[] parameters)
        {
            this.logger.LogError(this.CreateMessage(message), parameters);
            return this;
        }

        public ScopedLogger<T> LogCritical(string message, params object[] parameters)
        {
            this.logger.LogCritical(this.CreateMessage(message), parameters);
            return this;
        }

        public ScopedLogger<T> LogWarning(Exception e, string message, params object[] parameters)
        {
            this.logger.LogWarning(e, this.CreateMessage(message), parameters);
            return this;
        }

        public ScopedLogger<T> LogError(Exception e, string message, params object[] parameters)
        {
            this.logger.LogError(e, this.CreateMessage(message), parameters);
            return this;
        }

        public ScopedLogger<T> LogCritical(Exception e, string message, params object[] parameters)
        {
            this.logger.LogCritical(e, this.CreateMessage(message), parameters);
            return this;
        }

        private string CreateMessage(string message)
        {
            return $"{this.flowId}{this.scope}: {message}";
        }

        public static ScopedLogger<T> Create(ILogger<T> logger, string scope, string flowId)
        {
            return new ScopedLogger<T>(logger, scope.ThrowIfNull(nameof(scope)), flowId.ThrowIfNull(nameof(flowId)));
        }
    }
}
