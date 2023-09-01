import { BoardPost } from '@/Faculty/Backend/ModelsDB/Board/BoardPost';
import { BoardPostContents } from '@/Faculty/Backend/ModelsDB/Board/BoardPostContents';

/** 게시물 수정용 보기 요청 */
export interface PostEditViewResultModel 
{
    /** 게시물 */
    Post: BoardPost,
    /** 게시물의 내용물 */
    PostContents: BoardPostContents,
    InfoCode: string,
    Message: string,
}