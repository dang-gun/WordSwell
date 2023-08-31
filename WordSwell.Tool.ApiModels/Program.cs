using DGU_ModelToOutFiles.App.Faculty;
using DGUtility.ProjectXml;

namespace WordSwell.Tool.ApiModels;

internal class Program
{
    
    static void Main(string[] args)
    {
        //프로젝트 루트 폴더
        string sProjectRootDir = string.Empty;
        if (true == System.Diagnostics.Debugger.IsAttached)
        {//IDE에서 실행중일때
            
            sProjectRootDir
                = Path.GetFullPath(
                    Path.Combine("..", "..", "..")
                    , Environment.CurrentDirectory);
        }
        else
        {//일반 실행일때

            // 현재 실행중인 프로그램 명을 포함한 경로
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // 현재 실행중인 프로그램의 경로
            sProjectRootDir = Path.GetDirectoryName(path)!;

            Console.WriteLine("false : " + sProjectRootDir);
        }
        

        //Application.StartupPath
        Console.WriteLine("Hello, WordSwell.Tool.ApiModels!");

        //출력할 위치
        string sOutputPath = "D:\\OutputFiles";
        //출력 타입
        string sOutputType = "typescript";
        //출력할 폴더 비우기 여부
        bool bOutputPathClear = false;
        //임포트시 앞에 붙을 루트
        string sImportRootDir = string.Empty;
        //string sImportRootDir = "./";

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

                case "-clear"://출력 폴더 비우기 여부
                    bOutputPathClear = true;
                    break;

                case "-importroot"://임포트시 앞에 붙을 루트
                    sImportRootDir = args[i + 1];
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
                otoTemp = new ObjectToOut_Typescript(sOutputPath, xml, sImportRootDir);
                break;

            default:
                Console.WriteLine("====== 잘못된 형식을 지정하였습니다. ======");
                return;

        }

        if(true == bOutputPathClear)
        {//출력 폴더 지우기

            DirectoryInfo diOutP = new DirectoryInfo(sOutputPath);

            //루트에 있는 파일 찾기
            FileInfo[] arrFI = diOutP.GetFiles();
            foreach (FileInfo fileItem in arrFI)
            {
                //파일 삭제
                fileItem.Delete();
            }

            //루트에 있는 폴더 찾기
            DirectoryInfo[] arrDI = diOutP.GetDirectories();
            foreach (DirectoryInfo diItem in arrDI)
            {
                //폴더 삭제
                diItem.Delete(true);
            }

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