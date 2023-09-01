import { BoardStateType } from '@/Faculty/Backend/ModelsDB_partial/Board/BoardStateType';
import { BoardPost } from '@/Faculty/Backend/ModelsDB/Board/BoardPost';

/** 게시판 */
export interface Board 
{
    /** 게시판 고유번호 */
    idBoard: number,
    /** 게시판 제목 */
    Title: string,
    /** 게시판 상태 */
    State: BoardStateType,
    /** 다른 개체에서 이 개체로 연결된 리스트 */
    Posts?: BoardPost[],
}