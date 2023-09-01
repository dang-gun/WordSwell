

import { BoardPost } from '@/ModelsDB/Board/BoardPost';
import { BoardPostContents } from '@/ModelsDB/Board/BoardPostContents';

/** 지정한 게시판의 지정한 게시물을 보기 결과 리스트 */
export interface PostViewResultModel 
{
    /** 게시물 */
    Post: BoardPost,
    /** 게시물의 내용물 */
    PostContents: BoardPostContents,
    InfoCode: string,
    Message: string,
}