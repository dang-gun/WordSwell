using DGUtility.ApiResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    /// 지정한 게시판의 조건에 맞는 리스트를 표시한다.
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
        if (null == callData.ShowCount 
            || 0 >= callData.ShowCount)
        {
            callData.ShowCount = 10;
        }

        //페이지 번호
        if (null == callData.PageNumber
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
        }//end using db1

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
                        case BoardSearchTargetType.PostIndex:
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

                        case BoardSearchTargetType.Title:
                            {
                                iqG_R 
                                    = iqG_R
                                        .Where(w => callData.Search.Contains(w.Title));
                            }
                            break;

                        case BoardSearchTargetType.Contents:
                            {
                                iqG_R = iqG_R.Where(w =>
                                callData.Search.Contains(
                                    w.Contents!.Count > 0 
                                    ? w.Contents.First().Contents
                                    : string.Empty)
                                );
                                            
                            }
                            break;

                        case BoardSearchTargetType.TitleAndContents:
                            {
                                iqG_R
                                    = iqG_R
                                        .Where(w => callData.Search.Contains(w.Title)
                                        || callData.Search.Contains(
                                                w.Contents!.Count > 0
                                                ? w.Contents.First().Contents
                                                : string.Empty));
                            }
                            break;

                        case BoardSearchTargetType.UserName:
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

    /// <summary>
    /// 지정한 게시판의 지정한 게시물의 내용을 본다.
    /// </summary>
    /// <param name="callData"></param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<PostViewResultModel> PostView([FromQuery] PostViewCallModel callData)
    {
        ApiResultReady arReturn = new ApiResultReady(this);
        PostViewResultModel rmReturn = new PostViewResultModel();
        arReturn.ResultObject = rmReturn;

        //지정된 게시판
        Board? findBoard = null;
        //지정된 게시물
        BoardPost? findPost = null;
        //지정된 게시물의 내용
        BoardPostContents? findPostContents = null;

        using (ModelsDbContext db1 = new ModelsDbContext())
        {
            //게시판 검색
            findBoard
                = db1.Board
                    .Where(w => w.idBoard == callData.idBoard)
                    .FirstOrDefault();

            if (null == findBoard)
            {
                arReturn.ApiResultInfoSet(
                    "B1-200001"
                    , "게시판을 찾을 수 없습니다");
            }
        }//end using db1


        if (true == arReturn.IsSuccess())
        {
            using (ModelsDbContext db2 = new ModelsDbContext())
            {
                //게시물 검색
                findPost
                    = db2.BoardPost
                        .Where(w => w.idBoard == callData.idBoard 
                                && w.idBoardPost == callData.idBoardPost)
                        .Include(i => i.Contents)
                        .FirstOrDefault();

                if (null == findPost)
                {
                    arReturn.ApiResultInfoSet(
                        "B1-200010"
                        , "게시물이 없습니다");
                }
                else
                {
                    if(null == findPost!.Contents
                        || 0 >= findPost!.Contents.Count)
                    {//게시물 내용이 없다.
                        arReturn.ApiResultInfoSet(
                        "B1-200011"
                        , "게시물의 내용이 없습니다");
                    }
                    else
                    {//게시물 내용이 있다.
                        findPostContents = findPost.Contents.First();
                    }
                }

            }//end using db2
        }


        if (true == arReturn.IsSuccess())
        {
            //결과 넣기
            rmReturn.Post = findPost;
            rmReturn.PostContents = findPostContents;
        }


        return arReturn.ToResult();
    }

    /// <summary>
    /// 게시물 작성
    /// </summary>
    /// <param name="callData"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult<PostWriteResultModel> PostWrite([FromBody] PostWriteCallModel callData)
    {
        ApiResultReady arReturn = new ApiResultReady(this);
        PostWriteResultModel rmReturn = new PostWriteResultModel();
        arReturn.ResultObject = rmReturn;

        DateTime dtNow = DateTime.Now;


        if(null == callData.Password
            || string.Empty == callData.Password)
        {
            arReturn.ApiResultInfoSet(
                    "B1-300001"
                    , "비회원 작성에서 비밀번호는 필수 있습니다.");
        }
        else if(null == callData.Title
            || string.Empty == callData.Title)
        {
            arReturn.ApiResultInfoSet(
                    "B1-300020"
                    , "제목을 넣어 주세요");
        }
        else if (null == callData.Contents
            || string.Empty == callData.Contents)
        {
            arReturn.ApiResultInfoSet(
                    "B1-300021"
                    , "내용을 넣어 주세요");
        }

        if (true == arReturn.IsSuccess())
        {
            //게시물 작성
            BoardPost newBP = new BoardPost();
            newBP.idBoard = callData.idBoard;
            newBP.Title = callData.Title!;
            newBP.WriteTime = dtNow;

            //게시물 내용 작성
            BoardPostContents newBPC = new BoardPostContents();
            newBPC.Password = callData.Password!;
            newBPC.Contents = callData.Contents!;

            using (ModelsDbContext db1 = new ModelsDbContext())
            {
                //게시물 등록
                db1.BoardPost.Add(newBP);
                db1.SaveChanges();


                //등록된 게시물 번호 지정
                newBPC.idBoardPost = newBP.idBoardPost;
                db1.BoardPostContents.Add(newBPC);
                db1.SaveChanges();

            }//end if db1


            //전달할 데이터
            rmReturn.idBoard = newBP.idBoard;
            rmReturn.idBoardPost = newBP.idBoardPost;
        }


        return arReturn.ToResult();
    }
}
