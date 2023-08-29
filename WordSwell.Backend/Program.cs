namespace WordSwell.Backend;


/// <summary>
/// 
/// </summary>
public class Program
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
#if DEBUG
                //디버그일때만 전체 접속 허용
                webBuilder.UseUrls("https://*:7250");
#endif

                webBuilder.UseStartup<Startup>();
            });
}