/** 게시물 작성 요청 모델 */
export interface PostWriteCallModel 
{
    /** 게시판 고유 번호 */
    idBoard: number,
    /** 제목 */
    Title: string,
    /** 비밀번호 - 비회원이 글쓴 경우 넣는다. */
    Password: string,
    /** 비회원일때 유저 이름 */
    UserName: string,
    /** 게시물 내용 */
    Contents: string,
}