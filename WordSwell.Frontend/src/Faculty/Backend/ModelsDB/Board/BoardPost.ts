

import { Board } from '@/Faculty/Backend/ModelsDB/Board/Board';
import { BoardPostContents } from '@/Faculty/Backend/ModelsDB/Board/BoardPostContents';

/** 게시판의 게시물 */
export interface BoardPost 
{
    /** 게시판의 게시물 고유 번호 */
    idBoardPost: number,
    /** 소속 게시판 고유번호 - 외래키 */
    idBoard: number,
    /** 연결된 소속 게시판 정보 */
    Board?: Board,
    /** 제목 */
    Title: string,
    /** 유저 아이디 */
    idUser: number,
    /** 작성 시간 */
    WriteTime: Date,
    /** 수정 시간 */
    EditTime?: Date,
    /** 다른 개체에서 이 개체로 연결된 리스트.
            리스트로 표현되어 있지만 1:1구조이다. */
    Contents?: BoardPostContents[],
}