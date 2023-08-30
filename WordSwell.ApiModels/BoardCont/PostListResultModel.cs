using DGUtility.ApiResult;
using ModelsDB.Board;

namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 게시판 게시물 리스트
/// </summary>
public class PostListResultModel : ApiResultBaseModel
{
    /// <summary>
    /// 검색된 충전 요청 리스트
    /// </summary>
    public List<BoardPost>? PostList { get; set; }

    /// <summary>
    /// 검색된 게시물 숫자
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// 지금 보고 있는 페이지 번호
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// 한 페이지의 컨탠츠 개수
    /// </summary>
    public int ShowCount { get; set; }
}
