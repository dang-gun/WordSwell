using DGUtility.ApiResult;
using ModelsDB.Board;

namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 게시물 작성 결과
/// </summary>
public class PostWriteResultModel : ApiResultBaseModel
{
    /// <summary>
    /// 게시판
    /// </summary>
    public long idBoard { get; set; }
    /// <summary>
    /// 생성된 게시물 고유 번호
    /// </summary>
    public long idBoardPost { get; set; }
}
