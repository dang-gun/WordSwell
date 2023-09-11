import { BoardPost } from '@/Faculty/Backend/ModelsDB/Board/BoardPost';
import { BoardPostContents } from '@/Faculty/Backend/ModelsDB/Board/BoardPostContents';
import { FileDbInfo } from '@/Faculty/Backend/ModelsDB/FileDb/FileDbInfo';

/** 게시물 수정용 보기 요청 */
export interface PostEditViewResultModel 
{
    /** 게시물 */
    Post?: BoardPost,
    /** 게시물의 내용물 */
    PostContents?: BoardPostContents,
    /** 첨부 파일 리스트 */
    FileList?: FileDbInfo[],
    InfoCode: string,
    Message: string,
}