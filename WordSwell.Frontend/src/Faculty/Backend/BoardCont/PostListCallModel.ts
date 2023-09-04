import { BoardSearchTargetType } from '@/Faculty/Backend/BoardCont/BoardSearchTargetType';

/** 게시물 리스트를 요청할때 모델 */
export interface PostListCallModel 
{
    /** 게시판 고유 번호 */
    idBoard: number,
    /** 검색어 */
    Search?: string,
    /** 검색 대상 */
    SearchTargetType?: BoardSearchTargetType,
    /** 한 페이지 표시 개수 */
    ShowCount?: number,
    /** 표시할 페이지 번호 */
    PageNumber?: number,
}