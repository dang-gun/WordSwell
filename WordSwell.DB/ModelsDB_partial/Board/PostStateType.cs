namespace ModelsDB_partial.Board;

/// <summary>
/// 게시물 상태
/// </summary>
public enum PostStateType
{
    /// <summary>
    /// 상태 없음
    /// </summary>
    None = 0,

    /// <summary>
    /// 승인 대기
    /// </summary>
    /// <remarks>
    /// 승인 후 개시되는 게시판의 경우 사용함
    /// </remarks>
    WaitingApproval = 1,

    /// <summary>
    /// 정상 상태
    /// </summary>
    Normal = 10,
    

    /// <summary>
    /// 임시 차단
    /// </summary>
    Block_Temp = 100,
    /// <summary>
    /// 완전 차단
    /// </summary>
    Block = 101,

    /// <summary>
    /// 정상 삭제 - 유저가 삭제한 경우
    /// </summary>
    Delete = 1000,

    /// <summary>
    /// 관리자 권한이 있는 유저가 삭제
    /// </summary>
    Delete_Admin = 1100,
}
