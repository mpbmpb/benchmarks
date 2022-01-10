using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Benchmarks;

[MemoryDiagnoser]
public class LoggingTests
{
    private const string Message = "This is a message";
    private const string MessageWithParameters = "A message with parameters {0} and {1}";
    
    private readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole().SetMinimumLevel(LogLevel.Warning);
    });

    private readonly ILogger<LoggingTests> _logger;
    private readonly ILoggerAdapter<LoggingTests> _loggerAdapter;
    private readonly NullLogger<LoggingTests> _nullLogger = new();
    private readonly ILoggerAdapter<LoggingTests> _nullAdapter;
    private StringWriter _messages = new();

    private readonly Logger log = new LoggerConfiguration()
        .MinimumLevel.Warning()
        .WriteTo.Console()
        .CreateLogger();

    private readonly Logger nullLog;
    
    

    public LoggingTests()
    {
        _logger = new Logger<LoggingTests>(_loggerFactory);
        _loggerAdapter = new LoggerAdapter<LoggingTests>(_logger);
        _nullAdapter = new LoggerAdapter<LoggingTests>(_nullLogger);
        nullLog = new LoggerConfiguration()
            .MinimumLevel.Warning()
            .WriteTo.TextWriter(_messages)
            .CreateLogger();
    }

    [Benchmark]
    public void Log_without_if()
    {
        _logger.LogInformation(Message);
    }

    [Benchmark]
    public void Log_with_if()
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(Message);
        }
    }

    [Benchmark]
    public void Log_without_if_with_parameters()
    {
        _logger.LogInformation(MessageWithParameters, 42, 101);
    }

    [Benchmark]
    public void Log_with_if_with_parameters()
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(MessageWithParameters, 42, 101);
        }
    }
   
    [Benchmark]
    public void Log_with_adapter_with_parameters()
    {
        _loggerAdapter.LogInformation(MessageWithParameters, 42, 101);
    }

    [Benchmark]
    public void Log_using_Serilog_with_parameters()
    {
        log.Information(MessageWithParameters, 42, 101);
    }

    [Benchmark]
    public void Log_to_void_with_parameters()
    {
        _nullLogger.LogWarning(MessageWithParameters, 42, 101);
    }

    [Benchmark]
    public void Log_to_void_using_Adapter_with_parameters()
    {
        _nullAdapter.LogWarning(MessageWithParameters, 42, 101);
    }

    [Benchmark]
    public void Log_to_void_using_Serilog_with_parameters()
    {
        nullLog.Information(MessageWithParameters, 42, 101);
    }
    
}


public interface ILoggerAdapter<T>
{
    void LogInformation(string message);
    
    void LogInformation<T0>(string message, T0 arg0);
    
    void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1);
    
    void LogWarning(string message);
    
    void LogWarning<T0>(string message, T0 arg0);
    
    void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1);
    
}

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
    private ILogger<T> _logger;

    public LoggerAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(message);
        }
    }

    public void LogInformation<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(message, arg0);
        }
    }

    public void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(message, arg0, arg1);
        }
    }

    public void LogWarning(string message)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogWarning(message);
        }
    }

    public void LogWarning<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogWarning(message, arg0);
        }   
    }

    public void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogWarning(message, arg0, arg1);
        }    
    }
}