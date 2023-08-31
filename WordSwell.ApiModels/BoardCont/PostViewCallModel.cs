namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 게시물 보기 요청할때 모델
/// </summary>
public class PostViewCallModel
{
    /// <summary>
    /// 게시판 고유 번호
    /// </summary>
    public long idBoard { get; set; }

    /// <summary>
    /// 게시물 고유 번호
    /// </summary>
    public long idBoardPost { get; set; }

}
