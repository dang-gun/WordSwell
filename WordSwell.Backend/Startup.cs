using DbAssist.Faculty;
using Game_Adosaki.Global;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Utility.FileAssist;

namespace WordSwell.Backend;

/// <summary>
/// 
/// </summary>
public class Startup
{
    /// <summary>
	/// 
	/// </summary>
	public IConfiguration Configuration { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="env"></param>
    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
        this.Configuration = configuration;


        //DB 정보 읽기 ******************
        //DB정보 받으려는 타겟
        //string sConnectStringSelect = "Test_sqlite";
        string sConnectStringSelect = Configuration["DB_select"]!;
        string sSettingInfo_gitignoreDir = "SettingInfo_gitignore.json";

        DbInitialSetting dbInitialSetting;
        if (true == File.Exists(sSettingInfo_gitignoreDir))
        {//sSettingInfo_gitignoreDir파일이 있다.

            string s = File.ReadAllText(sSettingInfo_gitignoreDir);
            dynamic json = JsonConvert.DeserializeObject(s)!;

            

            //DB 설정
            dbInitialSetting
                = new DbInitialSetting(
                    (json[sConnectStringSelect].DBType).ToString()
                    , (json[sConnectStringSelect].ConnectionString).ToString());
        }
        else
        {
            //DB 설정
            dbInitialSetting
                = new DbInitialSetting(
                    Configuration[sConnectStringSelect + ":DBType"]!
                    , Configuration[sConnectStringSelect + ":ConnectionString"]!);
        }

        
        //로컬 경로 저장
        GlobalStatic.FileProc.ProjectRootDir = env.ContentRootPath;


        if (true == env.IsDevelopment())
        {//개발모드일때

            GlobalStatic.FileProc.ClientAppSrcDir.Add(
                Path.GetFullPath(
                    Path.Combine("..", "WordSwell.Frontend", "wwwroot", "production")
                    , GlobalStatic.FileProc.ProjectRootDir));

            GlobalStatic.FileProc.OutputFileDir
                = Path.GetFullPath(
                    Path.Combine("..", "WordSwell.Frontend", "wwwroot", "production", "UploadFile")
                    , GlobalStatic.FileProc.ProjectRootDir);
        }
        else
        {//프로덕션일때
            GlobalStatic.FileProc.ClientAppSrcDir.Add(
                Path.Combine(GlobalStatic.FileProc.ProjectRootDir, "wwwroot", "production"));

            GlobalStatic.FileProc.OutputFileDir
                = Path.Combine(GlobalStatic.FileProc.ProjectRootDir, "wwwroot", "production", "UploadFile");
        }


        


        //xml 파일 복사 **********************
        //대상 폴더
        string sXmlTarget = Path.GetFullPath(Path.Combine(".", "DocXml"));
        if (true == System.Diagnostics.Debugger.IsAttached
            && true == GlobalStatic.DebugIs)
        {//IDE가 연결되어 있고
         //디버그 모드일때만

            //지정된 폴더의 파일 리스트
            string[] arrFile 
                = Directory.GetFiles(
                    Path.GetFullPath(
                        Path.Combine("..", "WordSwell.Tool.ApiModels", "DocXml")
                        , GlobalStatic.FileProc.ProjectRootDir));

            

            //읽어들인 경로로 파일 복사
            foreach (string sItem in arrFile)
            {
                string sFileName = Path.GetFileName(sItem);
                string dest = Path.Combine(sXmlTarget, sFileName);
                File.Copy(sItem, dest, true);
            }
        }

        //복사된 xml 파일을 모두 지정
        FileInfo[] arrFI = new DirectoryInfo(sXmlTarget).GetFiles();
        foreach (FileInfo itemFI in arrFI)
        {
            GlobalStatic.FileProc.ProjectXmlDir_Other
            .Add(new FileCopyDir_OutListModel()
            {
                Name = itemFI.FullName
                , OriginalDir = sXmlTarget
                , TargetDir = Path.Combine(GlobalStatic.FileProc.ProjectRootDir, "DocXml")
            });
        }
        

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            //API모델을 파스칼 케이스 유지하기
            .AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1"
                , new OpenApiInfo
                {
                    Title = "WordSwell.Backend 프로젝트 Web API"
                    , Description = "WordSwell.Backend 프로젝트의 표준 백엔드 구현"
                    , Version = "v1"
                    , License = new OpenApiLicense
                    {
                        Name = "MIT"
                        , Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

            foreach (string sItem in GlobalStatic.FileProc.ProjectXmlDir_Other_FullAll)
            {
                c.IncludeXmlComments(sItem);
            }
        });

        //로그 파일 설정
        services.AddLogging(loggingBuilder => {
            loggingBuilder.AddFile("Logs/app_{0:yyyy}-{0:MM}-{0:dd}.log", fileLoggerOpts => {
                fileLoggerOpts.FormatLogFileName = fName => {
                    return String.Format(fName, DateTime.UtcNow);
                };
            });
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(
        IApplicationBuilder app
        , IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {//개발 버전에서만 스웨거 사용
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            string sSwagger = this.Configuration["swagger"];

            if ("true" == sSwagger)
            {//스웨거 사용
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }


        //https로 자동 리디렉션
        app.UseHttpsRedirection();

        //3.0 api 라우트
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }
}
