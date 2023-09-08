namespace WordSwell.Backend;


/// <summary>
/// 
/// </summary>
public class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        IHost build = CreateHostBuilder(args).Build();
        ServiceProvider = build.Services;
        build.Run();
        //CreateHostBuilder(args).Build().Run();
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
                //������϶��� ��ü ���� ���
                webBuilder.UseUrls("https://*:7250");
#endif

                webBuilder.UseStartup<Startup>();
            });
}