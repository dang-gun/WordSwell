namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 게시물 작성 요청 모델
/// </summary>
public class PostWriteCallModel
{
    /// <summary>
    /// 게시판 고유 번호
    /// </summary>
    public long idBoard { get; set; }


    /// <summary>
    /// 제목
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 비밀번호 - 비회원이 글쓴 경우 넣는다.
    /// </summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// 비회원일때 유저 이름
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 게시물 내용
    /// </summary>
    public string Contents { get; set; } = string.Empty;
}
