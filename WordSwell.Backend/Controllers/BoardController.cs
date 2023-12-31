﻿using Microsoft.AspNetCore.Mvc;
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
using WordSwell.Backend.Global;

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
                    GlobalInfo.BoardCont.PostList_Board_NotFound
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
                //게시물 번호 기준 내림차순
                IQueryable<BoardPost> findBpList
                    = db2.BoardPost
                        .Where(w => w.idBoard == findBoard!.idBoard
                                && w.PostState == PostStateType.Normal)
                        .OrderByDescending(ob => ob.idBoardPost);


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
                    GlobalInfo.BoardCont.PostView_Board_NotFound
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
                        GlobalInfo.BoardCont.PostView_BoardPost_NotFound
                        , "게시물이 없습니다");
                }
                else
                {
                    if(null == findPost!.Contents
                        || 0 >= findPost!.Contents.Count)
                    {//게시물 내용이 없다.
                        arReturn.ApiResultInfoSet(
                        GlobalInfo.BoardCont.PostView_BoardPostContents_NotFound
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
                    GlobalInfo.BoardCont.PostWrite_PasswordRequired_NonMember
                    , "비회원 작성에서 비밀번호는 필수 있습니다.");
        }
        else if(null == callData.Title
            || string.Empty == callData.Title)
        {
            arReturn.ApiResultInfoSet(
                    GlobalInfo.BoardCont.PostWrite_PleaseEnterTitle
                    , "제목을 넣어 주세요");
        }
        else if (null == callData.Contents
            || string.Empty == callData.Contents)
        {
            arReturn.ApiResultInfoSet(
                    GlobalInfo.BoardCont.PostWrite_PleaseEnterContents
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

                string sContents = newBPC.Contents;

                int nSuccess 
                    = this.FileListSave_ContentChanges(
                        rmReturn.idBoardPost
                        , newBPC.idBoardPostContents
                        , dtNow
                        , callData.FileList
                        , ref sContents);

                newBPC.Contents = sContents;

                if(nSuccess != callData.FileList.Count)
                {
                    arReturn.ApiResultInfoSet(
                        GlobalInfo.BoardCont.PostWrite_FileSave_Fail
                        , "일부 첨부파일을 저장하는데 실패 했습니다.(글 저장은 성공)");
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
        //첨부된 파일 리스트
        List<FileDbInfo>? findFileList = null;



        if (null == callData.Password
            || string.Empty == callData.Password)
        {
            arReturn.ApiResultInfoSet(
                    GlobalInfo.BoardCont.PostEditView_PasswordRequired_NonMember
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
                        GlobalInfo.BoardCont.PostEditView_Board_NotFound
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
                        GlobalInfo.BoardCont.PostEditView_BoardPost_NotFound
                        , "게시물이 없습니다");
                }
                else
                {
                    if (null == findPost!.Contents
                        || 0 >= findPost!.Contents.Count)
                    {//게시물 내용이 없다.
                        arReturn.ApiResultInfoSet(
                            GlobalInfo.BoardCont.PostEditView_BoardPostContents_NotFound
                            , "게시물의 내용이 없습니다");
                    }
                    else if (findPost!.Contents.First().Password != callData.Password)
                    {
                        arReturn.ApiResultInfoSet(
                            GlobalInfo.BoardCont.PostEditView_PasswordIncorrect
                            , "비밀 번호가 틀렸습니다.");
                    }
                    else
                    {//게시물 내용이 있다.
                        findPostContents = findPost.Contents.First();
                        findPostContents.FileList
                            = db3.FileDbInfo
                                .Where(w => w.idBoardPostContents == findPostContents.idBoardPostContents)
                                .ToList();

                        if (null != findPostContents.FileList)
                        {
                            findFileList = findPostContents.FileList.ToList();
                        }
                    }
                }

            }//end using db3
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
    /// 게시물 수정 요청
    /// </summary>
    /// <param name="callData"></param>
    /// <returns></returns>
    [HttpPatch]
    public ActionResult<PostEditApplyResultModel> PostEditApply([FromBody] PostEditApplyCallModel callData)
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

        //컨탠츠 임시 저장
        string sContentsTemp = string.Empty;

        if (null == callData.Password
            || string.Empty == callData.Password)
        {
            arReturn.ApiResultInfoSet(
                    GlobalInfo.BoardCont.PostEditApply_PasswordRequired_NonMember
                    , "비회원 글에서 비밀번호는 필수 있습니다.");
        }
        else if (null == callData.Title
            || string.Empty == callData.Title)
        {
            arReturn.ApiResultInfoSet(
                    GlobalInfo.BoardCont.PostEditApply_PleaseEnterTitle
                    , "제목을 넣어 주세요");
        }
        else if (null == callData.Contents
            || string.Empty == callData.Contents)
        {
            arReturn.ApiResultInfoSet(
                    GlobalInfo.BoardCont.PostEditApply_PleaseEnterContents
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
                        GlobalInfo.BoardCont.PostEditApply_Board_NotFound
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
                        GlobalInfo.BoardCont.PostEditApply_BoardPost_NotFound
                        , "게시물이 없습니다");
                }
                else
                {
                    if (null == findPost!.Contents
                        || 0 >= findPost!.Contents.Count)
                    {//게시물 내용이 없다.
                        arReturn.ApiResultInfoSet(
                            GlobalInfo.BoardCont.PostEditApply_BoardPostContents_NotFound
                            , "게시물의 내용이 없습니다");
                    }
                    else if (findPost!.Contents.First().Password != callData.Password)
                    {
                        arReturn.ApiResultInfoSet(
                            GlobalInfo.BoardCont.PostEditApply_PasswordIncorrect
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

                //컨탠츠 임시 저장
                sContentsTemp = callData.Contents!;
                findPostContents!.Contents = sContentsTemp;
                


                db3.BoardPost.Update(findPost);
                db3.BoardPostContents.Update(findPostContents);

                db3.SaveChanges();
            }//end using db2


            if (null != callData.FileList
                && 0 < callData.FileList.Count)
            {
                //새로운 파일 리스트
                List<FileItemModel> listNewFile = new List<FileItemModel>();
                
                using (ModelsDbContext db4 = new ModelsDbContext())
                {
                    foreach (FileItemModel fileItem in callData.FileList)
                    {
                        string sTypeStr = string.Empty;
                        if ("image" == fileItem.Type.Substring(0, 5))
                        {
                            sTypeStr = string.Empty;
                        }
                        else
                        {//나머지는 파일
                            sTypeStr = "file:";
                        }

                        FileDbInfo? findFDI = null;
                        if (0 < fileItem.idFileInfo)
                        {//기존 정보가 있다.

                            findFDI
                                = db4.FileDbInfo
                                    .Where(w => w.idFileInfo == fileItem.idFileInfo)
                                    .FirstOrDefault();
                        }
                            

                        if (true == fileItem.DeleteIs)
                        {//파일 삭제

                            if(null != findFDI)
                            {//대상이 있다.

                                //상태 변경
                                findFDI.FileDbState 
                                    = ModelsDB_partial.FileDb.FileDbStateType.Delete;
                            }

                            //연결된 본문 치환자 제거
                            sContentsTemp
                                = sContentsTemp
                                .Replace($"![{sTypeStr}{fileItem.Name}]"
                                        , "");
                        }
                        else if (true == fileItem.EditIs)
                        {//파일 수정

                            if (null != findFDI)
                            {//대상이 있다.

                                //파일 수정은 설명 수정과 대표이미지 수정 뿐이 없다.
                                findFDI.Description = fileItem.Description;
                            }
                        }
                        else if (0 >= fileItem.idFileInfo)
                        {//파일 추가
                            listNewFile.Add(fileItem);
                        }
                        else
                        {//기존 파일 수정없음
                        }
                    }//end foreach fileItem

                    int nSuccess
                        = this.FileListSave_ContentChanges(
                            findPost.idBoardPost
                            , findPostContents.idBoardPostContents
                            , dtNow
                            , listNewFile
                            , ref sContentsTemp);

                    if (nSuccess != callData.FileList.Count)
                    {
                        arReturn.ApiResultInfoSet(
                            GlobalInfo.BoardCont.PostEditApply_FileSave_Fail
                            , "일부 첨부파일을 저장하는데 실패 했습니다.(글 저장은 성공)");
                    }

                    //수정할 대상을 다시 찾고
                    BoardPostContents findBPC
                        = db4.BoardPostContents
                            .Where(w => w.idBoardPostContents 
                                        == findPostContents.idBoardPostContents)
                            .First();

                    //수정할 데이터 저장
                    findBPC.Contents = sContentsTemp;

                    //db에 적용
                    db4.SaveChanges();
                }//end using db4
            }
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
                    GlobalInfo.BoardCont.PostDelete_PasswordRequired_NonMember
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
                        GlobalInfo.BoardCont.PostDelete_Board_NotFound
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
                        GlobalInfo.BoardCont.PostDelete_BoardPost_NotFound
                        , "게시물이 없습니다");
                }
                else
                {
                    if (null == findPost!.Contents
                        || 0 >= findPost!.Contents.Count)
                    {//게시물 내용이 없다.
                        arReturn.ApiResultInfoSet(
                            GlobalInfo.BoardCont.PostDelete_BoardPostContents_NotFound
                            , "게시물의 내용이 없습니다");
                    }
                    else if (findPost!.Contents.First().Password != callData.Password)
                    {
                        arReturn.ApiResultInfoSet(
                            GlobalInfo.BoardCont.PostDelete_PasswordIncorrect
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
                findPost!.PostState = ModelsDB_partial.Board.PostStateType.Delete;

                //아직은 회원 기능이 없으므로 0으로 넣는다.
                findPost.idUser_Edit = 0;
                findPost.EditTime = dtNow;

                db3.BoardPost.Update(findPost);

                db3.SaveChanges();
            }//end using db2
        }


        return arReturn.ToResult();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idBoardPost"></param>
    /// <param name="idBoardPostContents"></param>
    /// <param name="dtCallDate"></param>
    /// <param name="FileDataList"></param>
    /// <param name="PostContents">수정할 본문 내용</param>
    /// <returns>업로드 성공 파일 수</returns>
    private int FileListSave_ContentChanges(
        long idBoardPost
        , long idBoardPostContents
        , DateTime dtCallDate
        , List<FileItemModel> FileDataList
        , ref string PostContents)
    {
        int nSuccess
            = GlobalStatic.FileDbProc
                .Save(
                    idBoardPost
                    , idBoardPostContents
                    , dtCallDate
                    , FileDataList);

        
        //내용물에서 고유번호로 바꿔야 하는 대상을 찾아 바꾼다.
        for (int i = 0; i < FileDataList.Count; ++i)
        {
            FileItemModel item = FileDataList[i];

            //변경에 사용될 추가 문자열
            string sAddData = string.Empty;

            if ("image" == item.Type.Substring(0, 5))
            {
                sAddData = string.Empty;
            }
            else
            {//나머지는 파일
                sAddData = "file:";
            }


            //추가 문자열에 새로 생성된 이름 붙이기
            sAddData += item.FileInfoName;

            PostContents
                = PostContents
                    .Replace(
                        $"![{item.Name}, {item.idLocal}]"
                        , $"![{sAddData}]");
        }//end for i


        return nSuccess;
    }
}
