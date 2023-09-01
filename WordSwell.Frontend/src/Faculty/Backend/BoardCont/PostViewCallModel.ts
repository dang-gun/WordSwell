/** 게시물 보기 요청할때 모델 */
export interface PostViewCallModel 
{
    /** 게시판 고유 번호 */
    idBoard: number,
    /** 게시물 고유 번호 */
    idBoardPost: number,
}