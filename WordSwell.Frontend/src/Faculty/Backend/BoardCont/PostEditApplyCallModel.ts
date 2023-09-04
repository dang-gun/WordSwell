import { FileItemModel } from '@/Faculty/Backend/FileDb/FileItemModel';

/** 게시물 수정 요청 모델 */
export interface PostEditApplyCallModel 
{
    /** 게시판 고유 번호 */
    idBoard: number,
    /** 게시물 고유 번호 */
    idBoardPost: number,
    /** 제목 */
    Title: string,
    /** 비밀번호 - 비회원이 글쓴 경우 넣는다. */
    Password: string,
    /** 게시물 내용 */
    Contents: string,
    /** 첨부 파일 리스트 */
    FileList?: FileItemModel[],
}