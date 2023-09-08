
using DGUtility.ModelToOutFiles.Global.Attributes;

namespace WordSwell.Global.InfoMessage;


/// <summary>
/// 인포 메시지 - BoardController = B1
/// </summary>
[ModelOutputJson]
public class InfoMessage_BoardCont
{
    /// <summary>
    /// 게시판을 찾을 수 없습니다
    /// </summary>
    public readonly string PostList_Board_NotFound = "B1-100001";

    /// <summary>
    /// 게시판을 찾을 수 없습니다
    /// </summary>
    public readonly string PostView_Board_NotFound = "B1-200001";
    /// <summary>
    /// 게시물이 없습니다
    /// </summary>
    public readonly string PostView_BoardPost_NotFound = "B1-200010";
    /// <summary>
    /// 게시물의 내용이 없습니다
    /// </summary>
    public readonly string PostView_BoardPostContents_NotFound = "B1-200011";

}