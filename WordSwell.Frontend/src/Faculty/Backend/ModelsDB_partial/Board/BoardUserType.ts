/** 보드 사용자 타입 */
export enum BoardUserType 
{
    /** 상태 없음 */
    None = 0,
    /** 비회원 */
    NonUser = 1,
    /** 회원 */
    User = 2,
    /** 관리자(개발자 포함) */
    Admin = 4,
    /** 이 게시판 관리자 */
    BoardAdmin = 1048576,
}