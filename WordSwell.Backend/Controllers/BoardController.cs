using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using DGUtility.ApiResult;

using ModelsContext;
using ModelsDB.Board;
using ModelsDB_partial.Board;

using WordSwell.ApiModels.BoardCont;
using Game_Adosaki.Global;
using WordSwell.Backend.Faculty.FileDb;
using ModelsDB;
using ModelsDB.FileDb;
using WordSwell.ApiModels.FileDb;

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
                //해당 게시판
                //정상 상태 게시물만
                IQueryable<BoardPost> findBpList
                    = db2.BoardPost
                        .Where(w => w.idBoard == findBoard!.idBoard
                                && w.PostState == PostStateType.Normal);


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
                                    findBpList 
                                        = findBpList.Where(w => w.idBoardPost == nPostIndex);
                                }
                                else
                                {//일치하는거 없음
                                    findBpList = findBpList.Take(0);
                                }

                                
                            }
                            break;

                        case BoardSearchTargetType.Title:
                            {
                                findBpList 
                                    = findBpList
                                        .Where(w => callData.Search.Contains(w.Title));
                            }
                            break;

                        case BoardSearchTargetType.Contents:
                            {
                                findBpList = findBpList.Where(w =>
                                callData.Search.Contains(
                                    w.Contents!.Count > 0 
                                    ? w.Contents.First().Contents
                                    : string.Empty)
                                );
                                            
                            }
                            break;

                        case BoardSearchTargetType.TitleAndContents:
                            {
                                findBpList
                                    = findBpList
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
                            findBpList = findBpList.Take(0);
                            break;
                    }//end switch (callData.SearchTargetType)
                }


                //페이징전에 개수 파악 ***************************
                rmReturn.TotalCount = findBpList.Count();
                rmReturn.PageNumber = Convert.ToInt32(callData.PageNumber);
                rmReturn.ShowCount = Convert.ToInt32(callData.ShowCount);


                //페이지 개수 만큼 자른다.***********************************
                findBpList = findBpList.Skip(rmReturn.ShowCount * (rmReturn.PageNumber - 1))
                            .Take(rmReturn.ShowCount);


                rmReturn.PostList = findBpList.ToList();
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
        //첨부된 파일 리스트
        List<FileDbInfo>? findFileList = null;


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
                        findPostContents.FileList 
                            = db2.FileDbInfo
                                .Where(w=>w.idBoardPostContents == findPostContents.idBoardPostContents)
                                .ToList();

                        if (null != findPostContents.FileList)
                        {
                            findFileList = findPostContents.FileList.ToList();
                        }
                    }
                }

            }//end using db2
        }


        if (true == arReturn.IsSuccess())
        {
            //결과 넣기
            rmReturn.Post = findPost;
            rmReturn.PostContents = findPostContents;
            rmReturn.FileList = findFileList;
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
            newBP.PostState = ModelsDB_partial.Board.PostStateType.Normal;
            newBP.Title = callData.Title!;
            newBP.UserName = callData.UserName;
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

            if (null != callData.FileList
                && 0 < callData.FileList.Count)
            {//파일 리스트가 있는경우

                int nSuccess 
                    = GlobalStatic.FileDbProc
                        .Save(
                            rmReturn.idBoardPost
                            , newBPC.idBoardPostContents
                            , dtNow
                            , callData.FileList);

                if(nSuccess != callData.FileList.Count)
                {
                    arReturn.ApiResultInfoSet(
                        "0"
                        , "일부 첨부파일을 저장하는데 실패 했습니다.");
                }

                
                //내용물에서 고유번호로 바꿔야 하는 대상을 찾아 바꾼다.
                for(int i = 0; i < callData.FileList.Count; ++i)
                {
                    FileItemModel item = callData.FileList[i];

                    //변경에 사용될 추가 문자열
                    string sAddData = string.Empty;

                    switch(item.Type)
                    {

                        case "image":
                            sAddData = string.Empty;
                            break;

                        default:
                            sAddData = "file:";
                            break;
                    }

                    //추가 문자열에 새로 생성된 이름 붙이기
                    sAddData += item.FileInfoName;

                    newBPC.Contents
                        = newBPC.Contents
                            .Replace(
                                $"![{item.idLocal}]"
                                , $"![{sAddData}]");
                }

                using (ModelsDbContext db2 = new ModelsDbContext())
                {
                    //수정할 대상을 다시 찾고
                    BoardPostContents findBPC
                        = db2.BoardPostContents
                            .Where(w => w.idBoardPostContents == newBPC.idBoardPostContents)
                            .First();

                    //수정할 데이터 저장
                    findBPC.Contents = newBPC.Contents;
                    db2.SaveChanges();

                }//end using db2
            }
        }


        return arReturn.ToResult();
    }

    /// <summary>
    /// 게시물 수정을 위한 보기요청
    /// </summary>
    /// <param name="callData"></param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<PostEditViewResultModel> PostEditView([FromQuery] PostEditViewCallModel callData)
    {
        ApiResultReady arReturn = new ApiResultReady(this);
        PostEditViewResultModel rmReturn = new PostEditViewResultModel();
        arReturn.ResultObject = rmReturn;


        //지정된 게시판
        Board? findBoard = null;
        //지정된 게시물
        BoardPost? findPost = null;
        //지정된 게시물의 내용
        BoardPostContents? findPostContents = null;



        if (null == callData.Password
            || string.Empty == callData.Password)
        {
            arReturn.ApiResultInfoSet(
                    "B1-400001"
                    , "비회원 작성에서 비밀번호는 필수 있습니다.");
        }

        
        if (true == arReturn.IsSuccess())
        {
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
                        "B1-400100"
                        , "게시판을 찾을 수 없습니다");
                }
            }//end using db1
        }

        if (true == arReturn.IsSuccess())
        {
            using (ModelsDbContext db3 = new ModelsDbContext())
            {
                //게시물 검색
                findPost
                    = db3.BoardPost
                        .Where(w => w.idBoard == callData.idBoard
                                && w.idBoardPost == callData.idBoardPost)
                        .Include(i => i.Contents)
                        .FirstOrDefault();

                if (null == findPost)
                {
                    arReturn.ApiResultInfoSet(
                        "B1-400101"
                        , "게시물이 없습니다");
                }
                else
                {
                    if (null == findPost!.Contents
                        || 0 >= findPost!.Contents.Count)
                    {//게시물 내용이 없다.
                        arReturn.ApiResultInfoSet(
                            "B1-400102"
                            , "게시물의 내용이 없습니다");
                    }
                    else if (findPost!.Contents.First().Password != callData.Password)
                    {
                        arReturn.ApiResultInfoSet(
                            "B1-400103"
                            , "비밀 번호가 틀렸습니다.");
                    }
                    else
                    {//게시물 내용이 있다.
                        findPostContents = findPost.Contents.First();
                    }
                }

            }//end using db3
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
    /// 게시물 수정 요청
    /// </summary>
    /// <param name="callData"></param>
    /// <returns></returns>
    [HttpPut]
    public ActionResult<PostEditApplyResultModel> PostEditApply([FromQuery] PostEditApplyCallModel callData)
    {
        ApiResultReady arReturn = new ApiResultReady(this);
        PostEditApplyResultModel rmReturn = new PostEditApplyResultModel();
        arReturn.ResultObject = rmReturn;

        DateTime dtNow = DateTime.Now;


        //지정된 게시판
        Board? findBoard = null;
        //지정된 게시물
        BoardPost? findPost = null;
        //지정된 게시물의 내용
        BoardPostContents? findPostContents = null;

        if (null == callData.Password
            || string.Empty == callData.Password)
        {
            arReturn.ApiResultInfoSet(
                    "B1-500001"
                    , "비회원 글에서 비밀번호는 필수 있습니다.");
        }
        else if (null == callData.Title
            || string.Empty == callData.Title)
        {
            arReturn.ApiResultInfoSet(
                    "B1-500020"
                    , "제목을 넣어 주세요");
        }
        else if (null == callData.Contents
            || string.Empty == callData.Contents)
        {
            arReturn.ApiResultInfoSet(
                    "B1-500021"
                    , "내용을 넣어 주세요");
        }

        if (true == arReturn.IsSuccess())
        {
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
                        "B1-500100"
                        , "게시판을 찾을 수 없습니다");
                }
            }//end using db1
        }

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
                        "B1-500101"
                        , "게시물이 없습니다");
                }
                else
                {
                    if (null == findPost!.Contents
                        || 0 >= findPost!.Contents.Count)
                    {//게시물 내용이 없다.
                        arReturn.ApiResultInfoSet(
                            "B1-500102"
                            , "게시물의 내용이 없습니다");
                    }
                    else if (findPost!.Contents.First().Password != callData.Password)
                    {
                        arReturn.ApiResultInfoSet(
                            "B1-500103"
                            , "비밀 번호가 틀렸습니다.");
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
            using (ModelsDbContext db3 = new ModelsDbContext())
            {
                //수정 사항 적용
                findPost!.Title = callData.Title!;

                //아직은 회원 기능이 없으므로 0으로 넣는다.
                findPost.idUser_Edit = 0;
                findPost.EditTime = dtNow;

                findPostContents!.Contents = callData.Contents!;


                db3.BoardPost.Update(findPost);
                db3.BoardPostContents.Update(findPostContents);

                db3.SaveChanges();
            }//end using db2
        }


        return arReturn.ToResult();
    }

    /// <summary>
    /// 게시물 삭제
    /// </summary>
    /// <remarks>
    /// 실제로 삭제되는건 아니고 상태만 삭제로 바꾼다.<br />
    /// 특정조건(예> 스케줄러에 의한 삭제 )에 맞으면 영구삭제 한다.
    /// </remarks>
    /// <param name="callData"></param>
    /// <returns></returns>
    [HttpDelete]
    public ActionResult<PostDeleteResultModel> PostDelete([FromQuery] PostDeleteCallModel callData)
    {
        ApiResultReady arReturn = new ApiResultReady(this);
        PostDeleteResultModel rmReturn = new PostDeleteResultModel();
        arReturn.ResultObject = rmReturn;

        DateTime dtNow = DateTime.Now;


        //지정된 게시판
        Board? findBoard = null;
        //지정된 게시물
        BoardPost? findPost = null;
        //지정된 게시물의 내용
        BoardPostContents? findPostContents = null;

        if (null == callData.Password
            || string.Empty == callData.Password)
        {
            arReturn.ApiResultInfoSet(
                    "B1-600001"
                    , "비회원 글에서 비밀번호는 필수 있습니다.");
        }

        if (true == arReturn.IsSuccess())
        {
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
                        "B1-600100"
                        , "게시판을 찾을 수 없습니다");
                }
            }//end using db1
        }

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
                        "B1-600101"
                        , "게시물이 없습니다");
                }
                else
                {
                    if (null == findPost!.Contents
                        || 0 >= findPost!.Contents.Count)
                    {//게시물 내용이 없다.
                        arReturn.ApiResultInfoSet(
                            "B1-600102"
                            , "게시물의 내용이 없습니다");
                    }
                    else if (findPost!.Contents.First().Password != callData.Password)
                    {
                        arReturn.ApiResultInfoSet(
                            "B1-600103"
                            , "비밀 번호가 틀렸습니다.");
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
            using (ModelsDbContext db3 = new ModelsDbContext())
            {
                //수정 사항 적용
                //아직은 회원 기능이 없으므로 0으로 넣는다.
                findPost!.idUser_Edit = 0;
                findPost.PostState = ModelsDB_partial.Board.PostStateType.Delete;
                findPost.EditTime = dtNow;

                db3.BoardPost.Update(findPost);

                db3.SaveChanges();
            }//end using db2
        }


        return arReturn.ToResult();
    }
}
