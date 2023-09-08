using DGUtility.FileAssist.FileCopy;
using DGUtility.FileAssist.FileSave;

namespace Utility.FileAssist;

/// <summary>
/// 파일 변환 관련
/// </summary>
public class FileProcess
{
	/// <summary>
	/// 파일 저장 지원
	/// </summary>
	private FileSaveAssist FileSaveAssist = new FileSaveAssist();

    /// <summary>
    /// 프로젝트 기준 루트 경로
    /// </summary>
    public string ProjectRootPath { get; set; } = string.Empty;

    /// <summary>
    /// 프로젝트 밑의 ClientApp/src 폴더 경로
    /// </summary>
    /// <remarks>
    /// 플록시 프로젝트의 경우 플로시 프로젝트의 ClientApp 폴더를 지정한다.<br />
    /// 이 프로젝트를 배포할때는 wwwroot아래의 배포 폴더를 지정한다.<br />
    /// 여러폴더에 배포해야하는 경우(예> 홈과 어드민이 별도의 프론트엔드로 나눠있는 경우)
    /// 이 리스트에 지정된 폴더에 모두 배포된다.<br />
    /// </remarks>
    public List<string> ClientAppSrcPath { get; set; } = new List<string>();

    /// <summary>
    /// 파일을 저장하고 출력할 폴더
    /// </summary>
    /// <remarks>
    /// 배포 버전과 상관없이 파일이 출력되는 위치이다.<br />
    /// 업로드된 파일과 같이 유저가 직접올린 파일이 있는 위치이다.
    /// </remarks>
    public string OutputFilePath { get; set; } = string.Empty;


    /// <summary>
    /// 지정된 경로 타입 +  파일을 생성하고 내용을 저장한다.
    /// </summary>
    /// <param name="typeFileDir"></param>
    /// <param name="sFilePath">파일 이름+확장자가 포함된 경로</param>
    /// <param name="sContents">문자열로된 내용</param>
    public void FileSave(
		FileDirType typeFileDir
		, string sFilePath
		, string sContents)
	{
		switch (typeFileDir)
		{
			case FileDirType.ClientAppSrcDir:
				foreach (string sItem in this.ClientAppSrcPath)
				{
					this.FileSave(@$"{sItem}\{sFilePath}", sContents);
				}
				break;

			case FileDirType.OutputFileDir:
				this.FileSave(@$"{this.OutputFilePath}\{sFilePath}", sContents);
				break;

			default:
				this.FileSave(@$"{this.ProjectRootPath}\{sFilePath}", sContents);
				break;
		}
	}

    /// <summary>
    /// 지정된 경로 타입 +  파일을 생성하고 내용을 저장한다.
    /// </summary>
    /// <param name="typeFileDir"></param>
    /// <param name="sFilePath">파일 이름+확장자가 포함된 경로</param>
    /// <param name="byteContents">바이너리 내용</param>
    public void FileSave(
        FileDirType typeFileDir
        , string sFilePath
        , byte[] byteContents)
    {
        switch (typeFileDir)
        {
            case FileDirType.ClientAppSrcDir:
                foreach (string sItem in this.ClientAppSrcPath)
                {
                    this.FileSave(@$"{sItem}\{sFilePath}", byteContents);
                }
                break;

            case FileDirType.OutputFileDir:
                this.FileSave(@$"{this.OutputFilePath}\{sFilePath}", byteContents);
                break;

            default:
                this.FileSave(@$"{this.ProjectRootPath}\{sFilePath}", byteContents);
                break;
        }
    }

    /// <summary>
    /// 경로에 디랙토리와 파일을 생성하고 내용을 저장한다.
    /// </summary>
    /// <param name="sFullFilePath"></param>
    /// <param name="sContents">문자열로된 내용</param>
    public void FileSave(string sFullFilePath, string sContents)
	{
        this.FileSaveAssist.FileSave(sFullFilePath, sContents);
    }

    /// <summary>
    /// 경로에 디랙토리와 파일을 생성하고 내용을 저장한다.
    /// </summary>
    /// <param name="sFullFilePath"></param>
    /// <param name="byteContents">바이너리 내용</param>
    public void FileSave(string sFullFilePath, byte[] byteContents)
    {
        this.FileSaveAssist.FileSave(sFullFilePath, byteContents);
    }
}
