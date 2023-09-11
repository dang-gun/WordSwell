
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


    /// <summary>
    /// 비회원 작성에서 비밀번호는 필수 있습니다.
    /// </summary>
    public readonly string PostWrite_PasswordRequired_NonMember = "B1-300001";
    /// <summary>
    /// 제목을 넣어 주세요
    /// </summary>
    public readonly string PostWrite_PleaseEnterTitle = "B1-300020";
    /// <summary>
    /// 내용을 넣어 주세요
    /// </summary>
    public readonly string PostWrite_PleaseEnterContents = "B1-300021";
    /// <summary>
    /// 일부 파일을 저장하는데 실패 했습니다.(글 저장은 성공)
    /// </summary>
    public readonly string PostWrite_FileSave_Fail = "0";


    /// <summary>
    /// 비회원 작성에서 비밀번호는 필수 있습니다.
    /// </summary>
    public readonly string PostEditView_PasswordRequired_NonMember = "B1-400001";
    /// <summary>
    /// 게시판을 찾을 수 없습니다
    /// </summary>
    public readonly string PostEditView_Board_NotFound = "B1-400100";
    /// <summary>
    /// 게시물이 없습니다
    /// </summary>
    public readonly string PostEditView_BoardPost_NotFound = "B1-400101";
    /// <summary>
    /// 게시물의 내용이 없습니다
    /// </summary>
    public readonly string PostEditView_BoardPostContents_NotFound = "B1-400102";
    /// <summary>
    /// 비밀 번호가 틀렸습니다.
    /// </summary>
    public readonly string PostEditView_PasswordIncorrect = "B1-400103";

    /// <summary>
    /// 비회원 작성에서 비밀번호는 필수 있습니다.
    /// </summary>
    public readonly string PostEditApply_PasswordRequired_NonMember = "B1-500001";
    /// <summary>
    /// 제목을 넣어 주세요
    /// </summary>
    public readonly string PostEditApply_PleaseEnterTitle = "B1-500020";
    /// <summary>
    /// 내용을 넣어 주세요
    /// </summary>
    public readonly string PostEditApply_PleaseEnterContents = "B1-500021";
    /// <summary>
    /// 게시판을 찾을 수 없습니다
    /// </summary>
    public readonly string PostEditApply_Board_NotFound = "B1-500100";
    /// <summary>
    /// 게시물이 없습니다
    /// </summary>
    public readonly string PostEditApply_BoardPost_NotFound = "B1-500101";
    /// <summary>
    /// 게시물의 내용이 없습니다
    /// </summary>
    public readonly string PostEditApply_BoardPostContents_NotFound = "B1-500102";
    /// <summary>
    /// 비밀 번호가 틀렸습니다.
    /// </summary>
    public readonly string PostEditApply_PasswordIncorrect = "B1-500103";
    /// <summary>
    /// 일부 파일을 저장하는데 실패 했습니다.(글 저장은 성공)
    /// </summary>
    public readonly string PostEditApply_FileSave_Fail = "0";


    /// <summary>
    /// 비회원 작성에서 비밀번호는 필수 있습니다.
    /// </summary>
    public readonly string PostDelete_PasswordRequired_NonMember = "B1-600001";
    /// <summary>
    /// 게시판을 찾을 수 없습니다
    /// </summary>
    public readonly string PostDelete_Board_NotFound = "B1-600100";
    /// <summary>
    /// 게시물이 없습니다
    /// </summary>
    public readonly string PostDelete_BoardPost_NotFound = "B1-600101";
    /// <summary>
    /// 게시물의 내용이 없습니다
    /// </summary>
    public readonly string PostDelete_BoardPostContents_NotFound = "B1-600102";
    /// <summary>
    /// 비밀 번호가 틀렸습니다.
    /// </summary>
    public readonly string PostDelete_PasswordIncorrect = "B1-600103";
}