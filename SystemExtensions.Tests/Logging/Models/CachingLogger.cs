using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace SystemExtensions.NetStandard.Tests.Logging.Models
{
    public sealed class CachingLogger<T> : ILogger<T>
    {
        public List<string> LogCache { get; } = new();

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            this.LogCache.Add(message);
        }
    }
}
