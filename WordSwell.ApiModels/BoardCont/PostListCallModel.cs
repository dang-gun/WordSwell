namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 게시물 리스트를 요청할때 모델
/// </summary>
public class PostListCallModel
{
    /// <summary>
    /// 게시판 고유 번호
    /// </summary>
    public long idBoard { get; set; }

    /// <summary>
    /// 검색어
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// 검색 대상
    /// </summary>
    public SearchTargetType? SearchTargetType { get; set; }

    /// <summary>
    /// 한 페이지 표시 개수
    /// </summary>
    public int? ShowCount { get; set; }
    /// <summary>
    /// 표시할 페이지 번호
    /// </summary>
    public int? PageNumber { get; set; }

}
