namespace Utility.AspdotnetLogger;

/// <summary>
/// Shared logger
/// </summary>
/// <remarks>
/// https://stackoverflow.com/questions/48676152/asp-net-core-web-api-logging-from-a-static-class
/// </remarks>
public class ApplicationLogging
{
    /// <summary>
    /// 로거 팩토리
    /// </summary>
    public ILoggerFactory LoggerFactory { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ApplicationLogging()
    {
        this.LoggerFactory = new LoggerFactory();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    public ApplicationLogging(ILoggerFactory loggerFactory)
    {
        this.LoggerFactory = loggerFactory;
    }

    /// <summary>
    /// 로그 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    /// <summary>
    /// 로그 생성
    /// </summary>
    /// <param name="sCategoryName"></param>
    /// <returns></returns>
    internal ILogger CreateLogger(string sCategoryName) => LoggerFactory.CreateLogger(sCategoryName);


    /// <summary>
    /// 에러 로그로 출력
    /// </summary>
    /// <param name="sCategoryName"></param>
    /// <param name="sMessage"></param>
    internal void LogError(
        string sCategoryName
        , string sMessage)
    {
        this.LoggerFactory
            .CreateLogger(sCategoryName)
            .LogError(sMessage);
    }
}
