#region

using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using RWF.Common;
using RWF.Common.Models;

#endregion

namespace RWF.WebFramework.Middlewares;

public class LogMiddleware
{
    private readonly ILoggerManager _logger;
    private readonly RequestDelegate _next;

    public LogMiddleware(ILoggerManager logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var watch = Stopwatch.StartNew();
        DateTime startDate = DateTime.Now;
        string url = httpContext.Request.Path.Value;
        UserRequestProperties userRequest = new();

        Stream originalBody = httpContext.Response.Body;

        try
        {
            userRequest = new UserRequestProperties
            {
                RegisterDate = startDate.ToString(),
                Params = await GetRequestBodyAsync(httpContext),
                Url = httpContext.Request.Host + url
            };

            string response;
            using (var memStream = new MemoryStream())
            {
                httpContext.Response.Body = memStream;

                await _next(httpContext);

                memStream.Position = 0;

                response = await new StreamReader(memStream).ReadToEndAsync();
                userRequest.Response = response.Replace("\"", "'");

                memStream.Position = 0;

                await memStream.CopyToAsync(originalBody);
            }

            userRequest.Duration = watch.ElapsedMilliseconds.ToString();
            watch.Stop();
            if (userRequest.Response.Contains("'StatusCode':200"))
                _logger.LogInfo(userRequest);
            else
                _logger.LogWarn(userRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarn(userRequest);
            httpContext.Response.Body = originalBody;
            await HandleExceptionAsync(httpContext, userRequest, ex, startDate);
        }
    }

    private async Task<string> GetRequestBodyAsync(HttpContext httpContext)
    {
        var req = httpContext.Request;

        // Allows using several time the stream in ASP.Net Core
        req.EnableBuffering();

        var bodyStr = "";
        // Arguments: Stream, Encoding, detect encoding, buffer size 
        // AND, the most important: keep stream opened
        using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
        {
            bodyStr = await reader.ReadToEndAsync();
        }

        // Rewind, so the core is not lost when it looks the body for the request
        req.Body.Position = 0;

        return bodyStr.Replace("\"", "'");
    }

    private async Task HandleExceptionAsync(HttpContext context, UserRequestProperties userRequest, Exception ex,
        DateTime startDate)
    {
        var errorLog = new ErrorLog
        {
            Message = ex + (ex.InnerException == null
                ? ""
                : "/n/n with Inner Exception: /n/n" + ex.InnerException),
            StackTrace = ex.StackTrace + (ex.InnerException == null
                ? ""
                : "/n/n with Inner Exception StackTrace: /n/n" + ex.StackTrace),
            RegisterDate = DateTime.Now,
        };

        userRequest.Error = JObject.FromObject(errorLog).ToString();
        userRequest.Duration = Convert.ToInt32((DateTime.Now - startDate).TotalMilliseconds).ToString();

        _logger.LogError(userRequest);

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = 500;
        var jsonstring = JObject.FromObject("Something bad happened").ToString();

        await context.Response.WriteAsync(jsonstring, Encoding.UTF8);
    }
}