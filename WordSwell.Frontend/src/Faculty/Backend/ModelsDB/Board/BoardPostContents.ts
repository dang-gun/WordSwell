import { BoardPost } from '@/Faculty/Backend/ModelsDB/Board/BoardPost';

/** 게시판의 게시물의 내용물 */
export interface BoardPostContents 
{
    /** 게시판의 게시물의 내용물 고유 번호 */
    idBoardPostContents: number,
    /** 게시판의 게시물 고유 번호 - 외래키 */
    idBoardPost: number,
    /** 연결된 게시물 */
    BoardPost?: BoardPost,
    /** 비밀번호 */
    Password: string,
    /** 게시물 내용 */
    Contents: string,
}