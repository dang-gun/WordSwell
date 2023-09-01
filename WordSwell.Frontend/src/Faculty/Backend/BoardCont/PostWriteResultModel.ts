/** 게시물 작성 결과 */
export interface PostWriteResultModel 
{
    /** 게시판 */
    idBoard: number,
    /** 생성된 게시물 고유 번호 */
    idBoardPost: number,
    InfoCode: string,
    Message: string,
}