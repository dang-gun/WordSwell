using Utility.FileAssist;
using Utility.ProjectXml;
using WordSwell.Tool.ApiModels.Faculty;
using WordSwell.Tool.ApiModels.Faculty.ObjectToOut;

namespace WordSwell.Tool.ApiModels;

internal class Program
{
    

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, WordSwell.Tool.ApiModels!");


        //프로젝트 루트 폴더
        string sProjectRootDir
            = Path.GetFullPath(
                Path.Combine("..", "..", "..")
                , Environment.CurrentDirectory);

        //출력할 위치
        string sOutputPath = "D:\\OutputFiles";

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
        ObjectToOutBase otoTemp = new ObjectToOut_Typescript(sOutputPath, xml);
        //파일로 출력
        otoTemp.ToTargetSave(
            new NamespaceTargetModel[]
            {
                new NamespaceTargetModel()
                {
                    AssemblyName = "WordSwell.DB"
                    , NamespaceList = new string[] { "ModelsDB" }
                }
                , new NamespaceTargetModel()
                {
                    AssemblyName = "WordSwell.ApiModels"
                    , NamespaceList = new string[] { "WordSwell.ApiModels.TestCont" }
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