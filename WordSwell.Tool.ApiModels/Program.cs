using System.Diagnostics;
using System.Runtime.CompilerServices;
using Utility.FileAssist;
using Utility.ProjectXml;
using WordSwell.Tool.ApiModels.Faculty;
using WordSwell.Tool.ApiModels.Faculty.ObjectToOut;

namespace WordSwell.Tool.ApiModels;

internal class Program
{
    
    static void Main(string[] args)
    {
        //프로젝트 루트 폴더
        string sProjectRootDir
            = Path.GetFullPath(
                Path.Combine("..", "..", "..")
                , Environment.CurrentDirectory);

        Console.WriteLine("Hello, WordSwell.Tool.ApiModels!");

        //출력할 위치
        string sOutputPath = "D:\\OutputFiles";
        //출력 타입
        string sOutputType = "typescript";

        for (int i = 0; i < args.Length; ++i)
        {
            string sCmd = args[i].ToLower();
            switch(sCmd)
            {
                case "-outputfolder"://출력폴더 지정
                    {
                        sOutputPath = args[i + 1];
                    }
                    break;

                case "-outputtype"://출력 타입
                    {
                        switch(args[i + 1])
                        {
                            case "typescript":
                            case "ts":
                                sOutputType = "typescript";
                                break;

                            default:
                                Console.WriteLine("====== 타입 지정 잘못됨 ======");
                                return;
                                //break;
                        }
                    }
                    break;
            }
        }

        

        

        //XML 파일 복사 **********************
        XmlFileAssist xmlFA 
            = new XmlFileAssist(
                sProjectRootDir
                , "DocXml");
        //https://stackoverflow.com/questions/15292758/way-to-determine-whether-executing-in-ide-or-not
        if (true == System.Diagnostics.Debugger.IsAttached)
        {//IDE에서 실행중이다.

            xmlFA.XmlFilesCopy();
            Console.WriteLine("====== End XML Files copy ======");
        }

        //xml 파일 패스 읽기
        xmlFA.XmlFilePathReload();


        //XML 읽어들이기 *****************
        ProjectXmlAssist xml 
            = new ProjectXmlAssist(xmlFA.XmlFilePathList);


        //지정된 네임스페이스에서 모델찾기 *******************
        //출력 폴더 지정
        ObjectToOutBase otoTemp;
        switch(sOutputType)
        {
            case "typescript":
                otoTemp = new ObjectToOut_Typescript(sOutputPath, xml);
                break;

            default:
                return;

        }
        //파일로 출력
        otoTemp.ToTargetSave(
            new NamespaceTargetModel[]
            {
                new NamespaceTargetModel()
                {
                    AssemblyName = "WordSwell.DB"
                    , NamespaceList 
                        = new string[] 
                        { 
                            "ModelsDB" 
                            , "ModelsDB_partial"
                        }
                }
                , new NamespaceTargetModel()
                {
                    AssemblyName = "WordSwell.ApiModels"
                    , NamespaceList = new string[] { "WordSwell.ApiModels" }
                }
            });


        Console.WriteLine("------- 'R' 을 눌러 프로그램 종료 ------");

        ConsoleKeyInfo keyinfo;
        do
        {
            keyinfo = Console.ReadKey();
        } while (keyinfo.Key != ConsoleKey.R);
    }


}