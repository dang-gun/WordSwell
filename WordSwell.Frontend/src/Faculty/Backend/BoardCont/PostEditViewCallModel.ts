/** 게시물 리스트를 요청할때 모델 */
export interface PostEditViewCallModel 
{
    /** 게시판 고유 번호 */
    idBoard: number,
    /** 게시물 고유 번호 */
    idBoardPost: number,
    /** 비밀번호 - 비회원이 글쓴 경우 넣는다. */
    Password: string,
}