using DGUtility.ApiResult;
using ModelsDB.Board;
using ModelsDB.FileDb;
using WordSwell.ApiModels.FileDb;

namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 지정한 게시판의 지정한 게시물을 보기 결과 리스트
/// </summary>
public class PostViewResultModel : ApiResultBaseModel
{
    /// <summary>
    /// 게시물
    /// </summary>
    public BoardPost? Post { get; set; }

    /// <summary>
    /// 게시물의 내용물
    /// </summary>
    public BoardPostContents? PostContents { get; set; }

    /// <summary>
    /// 첨부 파일 리스트
    /// </summary>
    public List<FileDbInfo>? FileList { get; set; }
}
