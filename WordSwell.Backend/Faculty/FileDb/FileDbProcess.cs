using WordSwell.ApiModels.FileDb;

namespace WordSwell.Backend.Faculty.FileDb;


/// <summary>
/// 파일DB를 관리하기위한 기능
/// </summary>
public class FileDbProcess
{
    /// <summary>
    /// 개체 생성
    /// </summary>
    public FileDbProcess()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="FileData"></param>
    /// <returns></returns>
    public bool Save(List<FileItemModel> FileData)
    {
        foreach (FileItemModel item in FileData)
        {

        }


        return true;
    }
}
