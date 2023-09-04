using WordSwell.ApiModels.FileDb;

namespace WordSwell.ApiModels.BoardCont;

/// <summary>
/// 게시물 수정 요청 모델
/// </summary>
public class PostEditApplyCallModel
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
    /// 제목
    /// </summary>
    public string? Title { get; set; } = string.Empty;

    /// <summary>
    /// 비밀번호 - 비회원이 글쓴 경우 넣는다.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 게시물 내용
    /// </summary>
    public string? Contents { get; set; } = string.Empty;

    /// <summary>
    /// 첨부 파일 리스트
    /// </summary>
    public List<FileItemModel>? FileList { get; set; }
}
