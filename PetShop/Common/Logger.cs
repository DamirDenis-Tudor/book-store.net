/**************************************************************************
 *                                                                        *
 *  Description: Logger Utility                                           *
 *  Website:     https://github.com/DamirDenis-Tudor/PetShop-ProiectIP    *
 *  Copyright:   (c) 2024, Damir Denis-Tudor                              *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using Microsoft.Extensions.Logging;

namespace Logger;

/// <summary>
/// Singleton class for managing loggers.
/// </summary>
public class Logger
{
    private static readonly ILoggerFactory LoggerInstance = LoggerFactory.Create(
        builder => builder
            .AddConsole()
            .AddDebug()
            .SetMinimumLevel(LogLevel.Debug)
    );

    private readonly Dictionary<Type, ILogger> _loggers = new();

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    private Logger() { }

    /// <summary>
    /// Gets the singleton instance of the Logger class.
    /// </summary>
    public static Logger Instance => new();

    /// <summary>
    /// Gets the logger for a specific type.
    /// </summary>
    /// <typeparam name="T">The type to get the logger for.</typeparam>
    /// <returns>The logger instance for the specified type.</returns>
    public ILogger GetLogger<T>()
    {
        if (_loggers.ContainsKey(typeof(T))) return _loggers[typeof(T)];
        var logger = LoggerInstance.CreateLogger<T>();
        _loggers[typeof(T)] = logger;
        return logger;
    }
}