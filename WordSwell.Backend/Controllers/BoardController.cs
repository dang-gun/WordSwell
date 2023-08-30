using DGUtility.ApiResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using ModelsContext;
using ModelsDB.Board;
using WordSwell.ApiModels.BoardCont;



namespace WordSwell.Backend.Controllers;

/// <summary>
/// 게시판 처리
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class BoardController : Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="callData"></param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<PostListResultModel> PostList([FromQuery] PostListCallModel callData)
    {
        ApiResultReady arReturn = new ApiResultReady(this);
        PostListResultModel rmReturn = new PostListResultModel();
        arReturn.ResultObject = rmReturn;


        //게시물 숫자
        if (null != callData.ShowCount 
            || 0 >= callData.ShowCount)
        {
            callData.ShowCount = 10;
        }

        //페이지 번호
        if (null != callData.PageNumber
            || 0 >= callData.PageNumber)
        {
            callData.PageNumber = 1;
        }


        //지정된 게시판
        Board? findBoard = null;

        using (ModelsDbContext db1 = new ModelsDbContext())
        {
            //게시판 검색
            findBoard 
                = db1.Board
                    .Where(w => w.idBoard == callData.idBoard)
                    .FirstOrDefault();

            if(null == findBoard)
            {
                arReturn.ApiResultInfoSet(
                    "B1-100001"
                    , "게시판을 찾을 수 없습니다");
            }
        }

        if (true == arReturn.IsSuccess())
        {
            using (ModelsDbContext db2 = new ModelsDbContext())
            {
                //검색 대상 전체 리스트 **************************
                IQueryable<BoardPost> iqG_R
                    = db2.BoardPost
                        .Where(w=>w.idBoard == findBoard!.idBoard);


                //검색어 확인 ***************************************
                if (null == callData.Search
                    || string.Empty == callData.Search)
                {//검색어 없음
                }
                else
                {
                    //검색
                    switch (callData.SearchTargetType)
                    {
                        case SearchTargetType.PostIndex:
                            {
                                long nPostIndex = 0;
                                if (true == Int64.TryParse(callData.Search, out nPostIndex))
                                {//숫자 변환이 됐다.
                                    iqG_R 
                                        = iqG_R.Where(w => w.idBoardPost == nPostIndex);
                                }
                                else
                                {//일치하는거 없음
                                    iqG_R = iqG_R.Take(0);
                                }

                                
                            }
                            break;

                        case SearchTargetType.Title:
                            {
                                iqG_R 
                                    = iqG_R
                                        .Where(w => callData.Search.Contains(w.Title));
                            }
                            break;

                        case SearchTargetType.Contents:
                            {
                                iqG_R = iqG_R.Where(w =>
                                callData.Search.Contains(
                                    w.Contents.Count > 0 
                                    ? w.Contents.First().Contents
                                    : string.Empty)
                                );
                                            
                            }
                            break;

                        case SearchTargetType.TitleAndContents:
                            {
                                iqG_R
                                    = iqG_R
                                        .Where(w => callData.Search.Contains(w.Title)
                                        || callData.Search.Contains(
                                                w.Contents.Count > 0
                                                ? w.Contents.First().Contents
                                                : string.Empty));
                            }
                            break;

                        case SearchTargetType.UserName:
                            {
                            }
                            break;

                        default://조건 안맞음
                            iqG_R = iqG_R.Take(0);
                            break;
                    }//end switch (callData.SearchTargetType)
                }


                //페이징전에 개수 파악 ***************************
                rmReturn.TotalCount = iqG_R.Count();
                rmReturn.PageNumber = Convert.ToInt32(callData.PageNumber);
                rmReturn.ShowCount = Convert.ToInt32(callData.ShowCount);


                //페이지 개수 만큼 자른다.***********************************
                iqG_R = iqG_R.Skip(rmReturn.ShowCount * (rmReturn.PageNumber - 1))
                            .Take(rmReturn.ShowCount);


                rmReturn.PostList = iqG_R.ToList();
            }//end using db2
        }
        

        return arReturn.ToResult();
    }
}
