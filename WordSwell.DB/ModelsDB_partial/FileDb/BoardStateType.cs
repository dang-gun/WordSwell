namespace ModelsDB_partial.FileDb;

/// <summary>
/// 파일DB 상태
/// </summary>
public enum FileDbStateType
{
    /// <summary>
    /// 상태 없음
    /// </summary>
    None = 0,

    /// <summary>
    /// 승인 대기
    /// </summary>
    /// <remarks>
    /// 승인 후 개시되는 경우 연결된 게시물의 상태가 변경될때까지 파일의 접근을 제한한다.
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
