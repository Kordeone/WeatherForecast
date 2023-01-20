#region

using NLog;
using RWF.Common.Models;

#endregion

namespace RWF.Common;

public interface ILoggerManager
{
    void LogTrace(string message);
    void LogDebug(string message);
    void LogInfo(string message);
    void LogInfo(UserRequestProperties request);
    void LogWarn(string message);
    void LogWarn(UserRequestProperties request);
    void LogError(string message);
    void LogError(UserRequestProperties request);
}

public class LoggerManager : ILoggerManager
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    public void LogDebug(string message)
    {
        Logger.Debug(message);
    }


    public void LogTrace(string message)
    {
        Logger.Trace(message);
    }

    public void LogError(string message)
    {
        Logger.Error(message);
    }

    public void LogError(UserRequestProperties request)
    {
        Logger.Error(GetInfoDictionary(request));
    }

    public void LogInfo(string message)
    {
        Logger.Info(message);
    }

    public void LogInfo(UserRequestProperties request)
    {
        Logger.Info(GetInfoDictionary(request));
    }


    public void LogWarn(string message)
    {
        Logger.Warn(message);
    }

    public void LogWarn(UserRequestProperties request)
    {
        Logger.Warn(GetInfoDictionary(request));
    }

    private LogEventInfo GetInfoDictionary(UserRequestProperties request)
    {
        LogEventInfo info = new LogEventInfo(LogLevel.Info, "UserRequest", request.Url);

        info.Properties.Add("Url", request.Url);
        info.Properties.Add("RegisterDate", request.RegisterDate);
        info.Properties.Add("Params", request.Params);
        info.Properties.Add("Responce", request.Response);
        info.Properties.Add("Duration", request.Duration);
        info.Properties.Add("Status", request.Status);
        info.Properties.Add("Error", !string.IsNullOrEmpty(request.Error) ? request.Error : "(:");

        return info;
    }
}