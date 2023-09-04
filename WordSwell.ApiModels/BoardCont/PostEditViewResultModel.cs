using DGUtility.ApiResult;
using ModelsDB.Board;
using WordSwell.ApiModels.FileDb;

namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 게시물 수정용 보기 요청
/// </summary>
public class PostEditViewResultModel : ApiResultBaseModel
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
    public List<FileItemModel>? FileList { get; set; }
}
