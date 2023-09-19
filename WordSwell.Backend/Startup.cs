using DbAssist.Faculty;
using DGUtility.FileAssist.FileCopy;
using DGUtility.XmlFileAssist;
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
    /// 프로젝트 XML 파일 관리
    /// </summary>
    public XmlFileAssist? XmlFA { get; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="env"></param>
    public Startup(
        IConfiguration configuration
        , IHostEnvironment env)
    {
        this.Configuration = configuration;

        //DB 정보 읽기 ******************
        //DB정보 받으려는 타겟
        //string sConnectStringSelect = "Test_sqlite";
        string sConnectStringSelect = Configuration["DB_select"]!;
        string sSettingInfo_gitignoreDir = "SettingInfo_gitignore.json";

        #region DB 설정
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
        #endregion


        //로컬 경로 저장
        GlobalStatic.FileProc.ProjectRootPath = env.ContentRootPath;


        if (true == env.IsDevelopment())
        {//개발모드일때

            GlobalStatic.FileProc.ClientAppSrcPath.Add(
                Path.GetFullPath(
                    Path.Combine("..", "WordSwell.Frontend", "wwwroot", "production")
                    , GlobalStatic.FileProc.ProjectRootPath));

            GlobalStatic.FileProc.OutputFilePath
                = Path.GetFullPath(
                    Path.Combine("..", "WordSwell.Frontend", "wwwroot", "production", "UploadFile")
                    , GlobalStatic.FileProc.ProjectRootPath);
        }
        else
        {//프로덕션일때
            GlobalStatic.FileProc.ClientAppSrcPath.Add(
                Path.Combine(GlobalStatic.FileProc.ProjectRootPath, "wwwroot", "production"));

            GlobalStatic.FileProc.OutputFilePath
                = Path.Combine(GlobalStatic.FileProc.ProjectRootPath, "wwwroot", "production", "UploadFile");
        }




        //복사하고 읽을 프로젝트 파일 지정 ******************
        this.XmlFA
            = new XmlFileAssist(
                GlobalStatic.FileProc.ProjectRootPath
                , "DocXml");

        //지정된 폴더의 파일 리스트
        string[] arrFile
            = Directory.GetFiles(
                Path.GetFullPath(
                    Path.Combine("..", "WordSwell.Tool.ApiModels", "DocXml")
                    , GlobalStatic.FileProc.ProjectRootPath));



        //읽어들인 경로로 파일 복사
        foreach (string sItem in arrFile)
        {
            this.XmlFA.XmlFilesAdd(
                Path.GetFileName(sItem)
                , Path.GetDirectoryName(sItem)!);
        }

        //xml 파일 패스 읽기
        this.XmlFA.XmlFilePathReload();




        //xml 파일 복사 **********************
        //대상 폴더
        string sXmlTarget = Path.GetFullPath(Path.Combine(".", "DocXml"));
        if (true == System.Diagnostics.Debugger.IsAttached
            && true == GlobalStatic.DebugIs)
        {//IDE가 연결되어 있고
         //디버그 모드일때만

            //XML 파일 복사 **********************
            this.XmlFA!.XmlFilesCopy();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.WithOrigins("https://localhost:9501")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((host) => true)
                        //.WithMethods("GET", "POST", "PUT")
                        .AllowCredentials();
                });
        });

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

            foreach (string sItem in this.XmlFA!.ProjectXmlPathListTarget)
            {
                c.IncludeXmlComments(sItem);
            }
        });


        //로그 파일 설정
        services.AddLogging(
            loggingBuilder
                => GlobalStatic.Log.configure(loggingBuilder, true));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="LoggerFactory"></param>
    public void Configure(
        IApplicationBuilder app
        , IWebHostEnvironment env
        , ILoggerFactory LoggerFactory)
    {
        //전역 로거 설정
        GlobalStatic.Log
            = new Utility.ApplicationLogger.DotNetLogging(LoggerFactory, true);



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
        //크로스도메인 사용
        app.UseCors("CorsPolicy");

        //3.0 api 라우트
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });


        //스케줄러 진행 *****************************
        GlobalStatic.TimeSked.On1Day -= TimeSked_On1Day;
        GlobalStatic.TimeSked.On1Day += TimeSked_On1Day;

        //프로그램이 시작하면 하루 처리 돌려야함
        this.TimeSked_On1Day();
        //스케줄러 시작
        GlobalStatic.TimeSked.Start();
    }

    /// <summary>
    /// 하루 날짜 변경
    /// </summary>
    private void TimeSked_On1Day()
    {
        //하루가 지나면 
        GlobalStatic.FileDbProc.BoardFileSaveFolderPath_Reset();
    }
}
