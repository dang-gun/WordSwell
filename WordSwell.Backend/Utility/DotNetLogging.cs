using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Utility.ApplicationLogger;

/// <summary>
/// 공유 로거Shared logger
/// </summary>
/// <remarks>
/// https://stackoverflow.com/questions/48676152/asp-net-core-web-api-logging-from-a-static-class
/// 
/// *** 종속성 ***
/// Microsoft.Extensions.Hosting
/// NReco.Logging.File
/// </remarks>
public class DotNetLogging
{
    /// <summary>
    /// 로거 팩토리
    /// </summary>
    public ILoggerFactory LoggerFactory_My { get; set; }

    /// <summary>
    /// 로그처리를 지원을 위한 기능
    /// </summary>
    public DotNetLogging()
    {
        this.LoggerFactory_My = new LoggerFactory();
    }
    /// <summary>
    /// 전달받은 ILoggerFactory개체를 이용하여 로거를 초기화한다.
    /// null이면 기본 옵션으로 새로 생성한다.
    /// </summary>
    /// <remarks>
    /// 자동 생성시 NReco.Logging.File를 기준으로 작성된다.
    /// </remarks>
    /// <param name="loggerFactory">생성한 ILoggerFactory 개채. null이면 자동생성</param>
    /// <param name="bConsole">자동생성시 콘솔 사용여부</param>
    public DotNetLogging(
        ILoggerFactory? loggerFactory
        , bool bConsole)
    {
        if(null != loggerFactory)
        {
            this.LoggerFactory_My = loggerFactory;
        }
        else
        {
            //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0#non-host-console-app
            this.LoggerFactory_My
                = LoggerFactory.Create(
                    loggingBuilder => this.configure(loggingBuilder, bConsole));
        }
    }

    /// <summary>
    /// 미리 세팅된 기본 옵션으로 로거를 생성한다.
    /// </summary>
    /// <param name="loggingBuilder"></param>
    /// <param name="bConsole"></param>
    /// <returns></returns>
    public ILoggingBuilder configure(
        ILoggingBuilder loggingBuilder
        , bool bConsole)
    {
        if (true == bConsole)
        {//콘솔 사용
            loggingBuilder.AddSimpleConsole(x => x.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ");
            //loggingBuilder.AddSimpleConsole(x=>x.TimestampFormat)
        }

        loggingBuilder.AddFile("Logs/app_{0:yyyy}-{0:MM}-{0:dd}.log"
            , fileLoggerOpts =>
            {
                fileLoggerOpts.FormatLogFileName = fName =>
                {
                    return String.Format(fName, DateTime.UtcNow);
                };

                fileLoggerOpts.FormatLogEntry = (lmMsg) =>
                {
                    string sLevel = string.Empty;

                    switch (lmMsg.LogLevel)
                    {
                        case LogLevel.Information:
                            sLevel = "Info";
                            break;
                        case LogLevel.Warning:
                            sLevel = "Warn";
                            break;

                        default:
                            sLevel = lmMsg.LogLevel.ToString();
                            break;
                    }

                    return $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {sLevel} [{lmMsg.LogName}] {lmMsg.Message}";
                };

            });

        return loggingBuilder;
    }

    /// <summary>
    /// 로거에 기록할 카테고리 정보
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal ILogger CreateLogger<T>() => LoggerFactory_My.CreateLogger<T>();
    /// <summary>
    /// 로거 생성
    /// </summary>
    /// <param name="sCategoryName"></param>
    /// <returns></returns>
    internal ILogger CreateLogger(string sCategoryName) 
        => LoggerFactory_My.CreateLogger(sCategoryName);

    /// <summary>
    /// 호출한 클래스를 찾아 카테고리 이름으로 출력한다.
    /// </summary>
    /// <returns>없으면 빈값</returns>
    private string CategoryName()
    {
        string sReturn = string.Empty;
        //= new StackTrace().GetFrame(2).GetMethod().ReflectedType.Name;

        StackTrace stTemp = new StackTrace();
        StackFrame? sfTemp = stTemp.GetFrame(2);

        if (null != sfTemp)
        {
            MethodBase? mbTemp = sfTemp.GetMethod();

            if (null != mbTemp)
            {
                Type? typeTemp = mbTemp.ReflectedType;
                if (null != mbTemp)
                {
                    sReturn = mbTemp.Name;
                }
            }
        }

        return sReturn;
    }


    /// <summary>
    /// 정보 로그로 출력
    /// </summary>
    /// <param name="sMessage"></param>
    internal void LogInfo(string sMessage)
    {
        this.LogInfo(this.CategoryName(), sMessage);
    }
    /// <summary>
    /// 정보 로그로 출력
    /// </summary>
    /// <param name="sCategoryName"></param>
    /// <param name="sMessage"></param>
    internal void LogInfo(
        string sCategoryName
        , string sMessage)
    {
        this.LoggerFactory_My
            .CreateLogger(sCategoryName)
            .LogInformation(sMessage);
    }

    /// <summary>
    /// 에러 로그로 출력
    /// </summary>
    /// <param name="ex"></param>
    internal void LogError(Exception ex)
    {
        this.LogError(ex.ToString());
    }
    /// <summary>
    /// 에러 로그로 출력
    /// </summary>
    /// <param name="sMessage"></param>
    internal void LogError(string sMessage)
    {
        this.LogError(this.CategoryName(), sMessage);
    }
    /// <summary>
    /// 에러 로그로 출력
    /// </summary>
    /// <param name="sCategoryName"></param>
    /// <param name="sMessage"></param>
    internal void LogError(
        string sCategoryName
        , string sMessage)
    {
        this.LoggerFactory_My
            .CreateLogger(sCategoryName)
            .LogError(sMessage);
    }


    /// <summary>
    /// 디버그 로그 출력
    /// </summary>
    /// <param name="sMessage"></param>
    internal void LogDebug(string sMessage)
    {
        this.LogDebug(this.CategoryName(), sMessage);
    }
    /// <summary>
    /// 디버그 로그 출력
    /// </summary>
    /// <param name="sCategoryName"></param>
    /// <param name="sMessage"></param>
    internal void LogDebug(
        string sCategoryName
        , string sMessage)
    {
        this.LoggerFactory_My
            .CreateLogger(sCategoryName)
            .LogDebug(sMessage);
    }

    

    /// <summary>
    /// 경고 로그 출력
    /// </summary>
    /// <param name="sMessage"></param>
    internal void LogWarning(string sMessage)
    {
        this.LogWarning(this.CategoryName(), sMessage);
    }
    /// <summary>
    /// 경고 로그 출력
    /// </summary>
    /// <param name="sCategoryName"></param>
    /// <param name="sMessage"></param>
    internal void LogWarning(
        string sCategoryName
        , string sMessage)
    {
        this.LoggerFactory_My
            .CreateLogger(sCategoryName)
            .LogWarning(sMessage);
    }

}
