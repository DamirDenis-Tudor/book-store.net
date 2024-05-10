﻿
using Microsoft.Extensions.Logging;


namespace Logger;

public class Logging
{
    private static readonly ILoggerFactory LoggerInstance = LoggerFactory.Create(
        builder => builder
            .AddConsole()
            .AddDebug()
            .SetMinimumLevel(LogLevel.Debug)
    );

    private readonly Dictionary<Type, ILogger> _loggers = new();

    private Logging() { }

    public static Logging Instance => new();

    public ILogger GetLogger<T>()
    {
        if (_loggers.ContainsKey(typeof(T))) return _loggers[typeof(T)];
        var logger = LoggerInstance.CreateLogger<T>();
        _loggers[typeof(T)] = logger;
        return logger;
    }
}