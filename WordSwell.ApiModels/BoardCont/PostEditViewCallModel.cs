namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 게시물 리스트를 요청할때 모델
/// </summary>
public class PostEditViewCallModel
{
    /// <summary>
    /// 게시판 고유 번호
    /// </summary>
    public long idBoard { get; set; }

    /// <summary>
    /// 게시물 고유 번호
    /// </summary>
    public long idBoardPost { get; set; }

    /// <summary>
    /// 비밀번호 - 비회원이 글쓴 경우 넣는다.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
