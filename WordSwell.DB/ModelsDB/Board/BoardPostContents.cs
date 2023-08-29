using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ModelsDB.Board;

/// <summary>
/// 게시판의 게시물의 내용물
/// </summary>
public class BoardPostContents
{
    /// <summary>
    /// 게시판의 게시물의 내용물 고유 번호
    /// </summary>
    [Key]
    public long idBoardPostContents { get; set; }

    /// <summary>
    /// 게시판의 게시물 고유 번호
    /// </summary>
    public long idBoardPost { get; set; }


    /// <summary>
    /// 게시물 내용
    /// </summary>
    public string Contents { get; set; } = string.Empty;



}
