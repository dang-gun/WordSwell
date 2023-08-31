/** 테스트용 타입 - 타입스크립트=역참조 불가 */
export const enum TestConstType 
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