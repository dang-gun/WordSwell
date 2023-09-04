/** 파일DB 상태 */
export const enum FileDbStateType 
{
    /** 상태 없음 */
    None = 0,
    /** 승인 대기 */
    WaitingApproval = 1,
    /** 정상 상태 */
    Normal = 10,
    /** 임시 차단 */
    Block_Temp = 100,
    /** 완전 차단 */
    Block = 101,
    /** 정상 삭제 - 유저가 삭제한 경우 */
    Delete = 1000,
    /** 관리자 권한이 있는 유저가 삭제 */
    Delete_Admin = 1100,
}