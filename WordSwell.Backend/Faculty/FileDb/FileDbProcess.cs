using WordSwell.ApiModels.FileDb;

namespace WordSwell.Backend.Faculty.FileDb;


/// <summary>
/// 파일DB를 관리하기위한 기능
/// </summary>
public class FileDbProcess : IDisposable
{
    /// <summary>
    /// 개체 생성
    /// </summary>
    public FileDbProcess()
    {
    }

    /// <summary>
    /// 명시적 파괴
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 파일 저장 시작
    /// </summary>
    /// <param name="FileData"></param>
    /// <returns></returns>
    public bool Save(List<FileItemModel> FileData)
    {
        foreach (FileItemModel item in FileData)
        {
            //item.
        }


        return true;
    }
}
