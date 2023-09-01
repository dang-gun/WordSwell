
using Newtonsoft.Json;
using DbAssist.Faculty;

namespace WordSwell.Tool.Db;

/// <summary>
/// 이 프로젝트는 제3자가 백엔드를 만들때 지원하기위한 프로그램이다.
/// 이 프로젝트에서는 마이그레이션을 하면 안된다.
/// </summary>
public static class Program
{

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, WordSwell.Tool.Db");

        //string sConnectStringSelect = string.Empty;
        string sConnectStringSelect = "DB_mssql_Test";

        //읽어들이 appsettings.json
        dynamic? jsonAppSettings = null;

        if (true == File.Exists("appsettings.json"))
        {//appsettings.json 파일이 있다.

            string sTemp = File.ReadAllText("appsettings.json");
            jsonAppSettings = JsonConvert.DeserializeObject(sTemp);

            if (jsonAppSettings != null)
            {
                sConnectStringSelect = jsonAppSettings["DB_select"];
            }
        }

        
            
        //콘솔 명령어를 최우선으로 사용한다.
        for (int i = 0; i < args.Length; ++i)
        {
            string sCmd = args[i].ToLower();
            switch (sCmd)
            {
                case "-select"://출력폴더 지정
                    {
                        sConnectStringSelect = args[i + 1];
                    }
                    break;

            }
        }


        if(string.Empty == sConnectStringSelect)
        {
            Console.WriteLine("선택된 DB가 없습니다.");
        }
        else if(jsonAppSettings != null)
        {//appsettings.json이 있다.

            DbInitialSetting dbInitialSetting;
            //DB 설정
            dbInitialSetting
                = new DbInitialSetting(
                    (jsonAppSettings[sConnectStringSelect].DBType).ToString()
                    , (jsonAppSettings[sConnectStringSelect].ConnectionString).ToString());
        }
        else
        {
            //마이그레이션할때는 이 정보를 바꿔야 한다.
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
        }





        Console.WriteLine("------- 'R' 을 눌러 프로그램 종료 ------");

        ConsoleKeyInfo keyinfo;
        do
        {
            keyinfo = Console.ReadKey();
        } while (keyinfo.Key != ConsoleKey.R);
    }
}