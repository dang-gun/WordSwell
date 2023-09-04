﻿import { BoardPost } from '@/Faculty/Backend/ModelsDB/Board/BoardPost';
import { BoardPostContents } from '@/Faculty/Backend/ModelsDB/Board/BoardPostContents';
import { FileItemModel } from '@/Faculty/Backend/FileDb/FileItemModel';

/** 지정한 게시판의 지정한 게시물을 보기 결과 리스트 */
export interface PostViewResultModel 
{
    /** 게시물 */
    Post?: BoardPost,
    /** 게시물의 내용물 */
    PostContents?: BoardPostContents,
    /** 첨부 파일 리스트 */
    FileList?: FileItemModel[],
    InfoCode: string,
    Message: string,
}