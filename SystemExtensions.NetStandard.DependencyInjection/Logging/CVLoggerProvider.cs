﻿using Microsoft.CorrelationVector;
using Microsoft.Extensions.Logging;
using System.Extensions;

namespace System.Logging;

public sealed class CVLoggerProvider : ICVLoggerProvider
{
    private readonly ILogsWriter logsManager;
    private readonly CorrelationVector correlationVector;

    public CVLoggerProvider(ILogsWriter logsWriter)
    {
        this.logsManager = logsWriter.ThrowIfNull(nameof(logsWriter));
        this.correlationVector = new CorrelationVector();
    }

    public void LogEntry(Log log)
    {
        if (this.correlationVector is not null)
        {
            log.CorrelationVector = this.correlationVector.Value.ToString();
            this.correlationVector.Increment();
        }

        this.logsManager.WriteLog(log);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new CVLogger(categoryName, this);
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }
}
