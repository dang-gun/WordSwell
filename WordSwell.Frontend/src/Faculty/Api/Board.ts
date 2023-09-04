import { PostDeleteCallModel } from "../Backend/BoardCont/PostDeleteCallModel";
import { PostEditApplyCallModel } from "../Backend/BoardCont/PostEditApplyCallModel";
import { PostEditApplyResultModel } from "../Backend/BoardCont/PostEditApplyResultModel";
import { PostEditViewCallModel } from "../Backend/BoardCont/PostEditViewCallModel";
import { PostEditViewResultModel } from "../Backend/BoardCont/PostEditViewResultModel";
import { PostListCallModel } from "../Backend/BoardCont/PostListCallModel";
import { PostListResultModel } from "../Backend/BoardCont/PostListResultModel";
import { PostViewCallModel } from "../Backend/BoardCont/PostViewCallModel";
import { PostViewResultModel } from "../Backend/BoardCont/PostViewResultModel";
import { PostWriteCallModel } from "../Backend/BoardCont/PostWriteCallModel";
import { PostWriteResultModel } from "../Backend/BoardCont/PostWriteResultModel";
import { Endpoint } from "./ApiEndpoint";
import { client } from "./Client";

/**
 * 게시물 작성 요청 API
 * @param { PostWriteCallModel } params 게시글 작성 요청 모델
 * @returns { PostWriteResultModel }
 */
export async function FetchPostWrite(params: PostWriteCallModel): Promise<PostWriteResultModel>
{
    const response = await client.post<PostWriteResultModel>(Endpoint.Board.PostWrite, params);

    return response.data;
}

/**
 * 게시글 리스트를 가져오는 API
 * @param { PostListCallModel } params 게시글 리스트 요청 모델
 * @returns { PostListResultModel }
 */
export async function FetchPostList(params: PostListCallModel): Promise<PostListResultModel>
{
    const response = await client.get<PostListResultModel>(Endpoint.Board.PostList, {
        params,
    });

    return response.data;
}

/**
 * 게시글을 조회하는 API
 * @param { PostViewCallModel } params 게시글 조회 요청 모델
 * @returns { PostViewResultModel }
 */
export async function FetchPostView(params: PostViewCallModel): Promise<PostViewResultModel>
{
    const response = await client.get<PostViewResultModel>(Endpoint.Board.PostView, {
        params,
    });

    return response.data;
}

/**
 * 게시글을 수정할 때 정보를 조회하는 API
 * @param { PostEditViewCallModel } params 게시글 정보 모델
 * @returns { PostWriteResultModel }
 */
export async function FetchPostEditView(params: PostEditApplyResultModel): Promise<PostEditViewResultModel>
{
    const response = await client.get<PostEditViewResultModel>(Endpoint.Board.PostEditView, {
        params,
    });

    return response.data;
}

/**
 * 게시글을 수정하는 API
 * @param { PostEditApplyCallModel } params 게시글 수정 요청 모델
 * @returns { PostEditApplyResultModel }
 */
export async function FetchPostEdit(params: PostEditApplyCallModel): Promise<PostEditApplyResultModel>
{
    const repsonse = await client.patch<PostEditApplyResultModel>(
        Endpoint.Board.PostEditApply,
        params
    );

    return repsonse.data;
}

/**
 * 게시글을 삭제하는 API
 * @param { PostDeleteCallModel } params 게시글 삭제 요청 모델
 * @returns { PostEditApplyResultModel }
 */
export async function FetchPostDelete(params: PostDeleteCallModel): Promise<PostEditApplyResultModel>
{
    const response = await client.delete<PostEditApplyResultModel>(
        Endpoint.Board.PostDelete,
        {
            params,
        }
    );

    return response.data;
}
