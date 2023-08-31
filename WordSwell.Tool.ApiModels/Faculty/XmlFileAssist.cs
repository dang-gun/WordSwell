using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.FileAssist;

namespace DGU_ModelToOutFiles.App.Faculty;

/// <summary>
/// XML 파일 지원
/// </summary>
internal class XmlFileAssist
{
    /// <summary>
    /// 프로젝트 루트 경로
    /// </summary>
    public string ProjectRootPath { get; set; }

    /// <summary>
    /// 출력 폴더 상대 경로
    /// </summary>
    public string OutputFolder_RelativePath { get; set; }

    /// <summary>
    /// 읽어들인 xml 파일 
    /// </summary>
    /// <remarks>
    /// XmlFilePathReload()로 리스트를 읽어야 내용물이 채워진다.
    /// </remarks>
    public string[] XmlFilePathList { get; private set; } = new string[0];

    /// <summary>
    /// 출력 폴더 전체 경로
    /// </summary>
    public string OutputFolder_FullPath
    {
        get
        {
            return Path.Combine(ProjectRootPath, OutputFolder_RelativePath);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sProjectRootPath"></param>
    /// <param name="sOutputFolder_RelativePath"></param>
    public XmlFileAssist(
        string sProjectRootPath
        , string sOutputFolder_RelativePath)
    {
        ProjectRootPath = sProjectRootPath;
        OutputFolder_RelativePath = sOutputFolder_RelativePath;
    }

    /// <summary>
    /// XML 파일 복사
    /// </summary>
    public void XmlFilesCopy()
    {
        //복사 대상 폴더
        string sTargetDir = Path.Combine(ProjectRootPath, "DocXml");

        //복사할 파일 리스트
        List<FileCopyDir_OutListModel> listProjectXmlDir
            = new List<FileCopyDir_OutListModel>();


        Console.WriteLine("====== XML Files copy ======");

        //□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
        //가급적 비주얼 스튜디오상 프로젝트 정렬에 맞출것!
        //□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□

        //WordSwell.ApiModels
        listProjectXmlDir
            .Add(new FileCopyDir_OutListModel()
            {
                Name = "WordSwell.ApiModels.xml"
                , OriginalDir
                    = Path.GetFullPath(
                        Path.Combine("..", "WordSwell.ApiModels")
                        , ProjectRootPath)
                , TargetDir = sTargetDir
            });

        //WordSwell.DB
        listProjectXmlDir
            .Add(new FileCopyDir_OutListModel()
            {
                Name = "WordSwell.DB.xml"
                , OriginalDir
                    = Path.GetFullPath(
                        Path.Combine("..", "WordSwell.DB")
                        , ProjectRootPath)
                , TargetDir = sTargetDir
            });

        //WordSwell.Backend
        listProjectXmlDir
            .Add(new FileCopyDir_OutListModel()
            {
                Name = "WordSwell.Backend.xml"
                , OriginalDir
                    = Path.GetFullPath(
                        Path.Combine("..", "WordSwell.Backend")
                        , ProjectRootPath)
                , TargetDir = sTargetDir
            });




        //파일 복사 *****************************
        foreach (FileCopyDir_OutListModel item in listProjectXmlDir)
        {
            string sOriginalFullDir = item.OriginalFullDir;

            if (true == File.Exists(sOriginalFullDir))
            {//원본 파일이 있다.

                //대상 위치에 복사
                File.Copy(sOriginalFullDir, item.TargetDirFull, true);
                Console.WriteLine($"file copy {sOriginalFullDir}, {item.TargetDirFull}");
            }
        }
    }//end XmlFilesCopy()


    /// <summary>
    /// 로컬에 있는 xml 파일리스트를 다시 읽어들인다.
    /// </summary>
    /// <remarks>
    /// 보통은 한번만 읽으면 되지만 파일리스트가 빈번하게 변경된다면 자주 읽어야 할 수 있다.
    /// </remarks>
    public void XmlFilePathReload()
    {
        DirectoryInfo dir = new DirectoryInfo(OutputFolder_FullPath);
        List<string> listFilePath = new List<string>();

        foreach (FileInfo fileItem in dir.GetFiles())
        {
            listFilePath.Add(fileItem.FullName);
        }

        XmlFilePathList = listFilePath.ToArray();
    }
}
