namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 검색 대상
/// </summary>
public enum BoardSearchTargetType
{
    /// <summary>
    /// 없음
    /// </summary>
    None = 0,

    /// <summary>
    /// 게시물 고유번호
    /// </summary>
    PostIndex,

    /// <summary>
    /// 제목
    /// </summary>
    Title,

    /// <summary>
    /// 내용
    /// </summary>
    Contents,

    /// <summary>
    /// 제목 + 내용
    /// </summary>
    TitleAndContents,

    /// <summary>
    /// 유저 이름
    /// </summary>
    UserName,
}
