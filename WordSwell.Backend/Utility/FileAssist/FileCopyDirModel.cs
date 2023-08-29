using System.Text;


namespace Utility.FileAssist;

/// <summary>
/// 파일 복사 경로 모델
/// </summary>
public class FileCopyDirModel
{
	/// <summary>
	/// 파일의 이름(확장자 포함)
	/// </summary>
	public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 원본 파일 위치(이름 제외)
    /// </summary>
    public string OriginalDir { get; set; } = string.Empty;
    /// <summary>
    /// 원본 파일의 전체 경로
    /// </summary>
    public string OriginalFullDir
    {
        get
        {
            return Path.Combine(this.OriginalDir, this.Name);
        }
    }

    /// <summary>
    /// 파일을 저장할 위치(이름 제외)
    /// </summary>
    public string TargetDir { get; set; } = string.Empty;
    /// <summary>
    /// 파일을 저장할 위치의 전체 경로
    /// </summary>
    public string TargetDirFull
    {
        get
        {
            return Path.Combine(this.TargetDir, this.Name);
        }
    }

}
