import { PostStateType } from '@/Faculty/Backend/ModelsDB_partial/Board/PostStateType';

/** 게시판의 댓글 */
export interface BoardPostReply 
{
    /** 게시판의 게시물 고유 번호 */
    idBoardPostReply: number,
    /** 종속된 게시물의 고유 번호 - 외래키 */
    idBoardPostContents: number,
    /** 종속된 게시물의 고유 번호 */
    idBoardPost: number,
    /** 대상 댓글의 번호(최상위이면 0) */
    idBoardPostReply_Target: number,
    /** 게시물 상태 */
    PostState: PostStateType,
    /** 작성자 고유번호 */
    idUser: number,
    /** 비회원일때 유저 이름 */
    UserName?: string,
    /** 비밀번호 */
    Password?: string,
    /** 게시물 내용 */
    Contents: string,
    /** 작성 시간 */
    WriteTime: Date,
    /** 마지막으로 수정한 유저 번호 */
    idUser_Edit?: number,
    /** 수정 시간 */
    EditTime?: Date,
}