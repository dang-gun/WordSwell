/** 검색 대상 */
export const enum BoardSearchTargetType 
{
    /** 없음 */
    None = 0,
    /** 게시물 고유번호 */
    PostIndex = 1,
    /** 제목 */
    Title = 2,
    /** 내용 */
    Contents = 3,
    /** 제목 + 내용 */
    TitleAndContents = 4,
    /** 유저 이름 */
    UserName = 5,
}