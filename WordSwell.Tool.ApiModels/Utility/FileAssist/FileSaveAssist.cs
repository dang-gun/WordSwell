using System.Text;


namespace Utility.FileAssist;

/// <summary>
/// 파일 저장 관련
/// </summary>
public class FileSaveAssist
{
	/// <summary>
	/// sFullDir 경로에 파일을 생성하고 내용을 저장한다.
	/// </summary>
	/// <param name="sFullFilePath"></param>
	/// <param name="sContents"></param>
	public void FileSave(string sFullFilePath, string sContents)
	{
		string? sdirectoryPath = Path.GetDirectoryName(sFullFilePath);
		if (sdirectoryPath != null)
		{
			if (false == Directory.Exists(sdirectoryPath))
			{//디랙토리가 없다.

				//디랙토리 생성
				Directory.CreateDirectory(sdirectoryPath);
			}
		}

		using (StreamWriter stream = new(sFullFilePath, false, Encoding.UTF8))
		{
			//파일 저장
			stream.Write(sContents);
		}//end using stream
	}
}
