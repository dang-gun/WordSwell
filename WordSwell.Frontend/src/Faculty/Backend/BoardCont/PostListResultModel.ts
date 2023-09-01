

import { BoardPost } from '@/Faculty/Backend/ModelsDB/Board/BoardPost';

/** 게시판 게시물 리스트 */
export interface PostListResultModel 
{
    /** 검색된 충전 요청 리스트 */
    PostList?: BoardPost[],
    /** 검색된 게시물 숫자 */
    TotalCount: number,
    /** 지금 보고 있는 페이지 번호 */
    PageNumber: number,
    /** 한 페이지의 컨탠츠 개수 */
    ShowCount: number,
    InfoCode: string,
    Message: string,
}