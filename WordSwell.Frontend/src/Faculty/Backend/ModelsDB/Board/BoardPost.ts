import { Board } from '@/Faculty/Backend/ModelsDB/Board/Board';
import { PostStateType } from '@/Faculty/Backend/ModelsDB_partial/Board/PostStateType';
import { BoardPostContents } from '@/Faculty/Backend/ModelsDB/Board/BoardPostContents';

/** 게시판의 게시물 */
export interface BoardPost 
{
    /** 게시판의 게시물 고유 번호 */
    idBoardPost: number,
    /** 소속 게시판 고유번호 - 외래키 */
    idBoard: number,
    /** 연결된 소속 게시판 정보 */
    Board: Board,
    PostState: PostStateType,
    /** 제목 */
    Title?: string,
    /** 작성자 고유번호 */
    idUser: number,
    /** 비회원일때 유저 이름 */
    UserName: string,
    /** 작성 시간 */
    WriteTime: Date,
    /** 마지막으로 수정한 유저 번호 */
    idUser_Edit?: number,
    /** 수정 시간 */
    EditTime?: Date,
    /** 다른 개체에서 이 개체로 연결된 리스트.
            리스트로 표현되어 있지만 1:1구조이다. */
    Contents?: BoardPostContents[],
}