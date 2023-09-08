using Game_Adosaki.Global;
using ModelsContext;
using WordSwell.ApiModels.FileDb;

using ModelsDB.FileDb;
using Utility.FileAssist;

namespace WordSwell.Backend.Faculty.FileDb;


/// <summary>
/// 파일DB를 관리하기위한 기능
/// </summary>
public class FileDbProcess
{
    /// <summary>
    /// 게시판에 첨부된 파일이 저장될 경로(부분)
    /// </summary>
    /// <remarks>
    /// 게시판 첨부 파일 경로 년\월\일\게시물번호
    /// 날짜로 폴더명을 생성한다
    /// </remarks>
    public string BoardFileSaveFolderPath { get; private set; } = string.Empty;
    /// <summary>
    /// 게시판에 첨부된 파일을 읽을 수 있는 URL(부분)
    /// </summary>
    public string BoardFileSaveFolderUrl { get; private set; } = string.Empty;

    /// <summary>
    /// 개체 생성
    /// </summary>
    public FileDbProcess()
    {
        this.BoardFileSaveFolderPath_Reset();
    }


    /// <summary>
    /// 게시판에 첨부된 파일이 저장될 경로를 다시 생성한다.
    /// </summary>
    /// <remarks>
    /// 날짜가 변경되면 호출할것을 권장한다.
    /// </remarks>
    public void BoardFileSaveFolderPath_Reset()
    {
        //날짜를 기준으로 폴더를 생성
        this.BoardFileSaveFolderPath
            = Path.Combine(GlobalStatic.TimeSked.Today.Year.ToString("0000")
                            , GlobalStatic.TimeSked.Today.Month.ToString("00")
                            , GlobalStatic.TimeSked.Today.Day.ToString("00"));

        //날짜를 기준으로 url 생성
        this.BoardFileSaveFolderUrl
            = string.Format("{0:0000}/{1:00}/{2:00}/"
                , GlobalStatic.TimeSked.Today.Year
                , GlobalStatic.TimeSked.Today.Month
                , GlobalStatic.TimeSked.Today.Day);
    }

    /// <summary>
    /// 파일 저장 시작
    /// </summary>
    /// <param name="idBoardPost">기준이 되는 고유값</param>
    /// <param name="idBoardPostContents">기준이 되는 연결된 게시물 내용 번호</param>
    /// <param name="dtCallDate">업로드 요청 날짜</param>
    /// <param name="FileData"></param>
    /// <returns></returns>
    public int Save(
        long idBoardPost
        , long idBoardPostContents
        , DateTime dtCallDate
        , List<FileItemModel> FileData)
    {
        //업로드 성공 개수
        int nSuccessCount = 0;

        //저장 경로 생성
        string sSaveFolderPath
            = Path.Combine(this.BoardFileSaveFolderPath
                            , idBoardPost.ToString());

        //읽을 url 생성
        string sSaveFolderUrl
            = this.BoardFileSaveFolderUrl + idBoardPost.ToString() + "/";


        foreach (FileItemModel item in FileData)
        {
            //guid로 저장할때 파일 아이디를 지정한다.(하이픈 제거)
            string sNewFileName = Guid.NewGuid().ToString().Replace("-", "");
            //파일이름에 확장자 붙이기
            sNewFileName += item.Extension;

            //파일명이 포함된 물리 경로
            string sNewFileFullName = Path.Combine(sSaveFolderPath, sNewFileName);


            //파일 저장 성공 여부
            bool bSaveSuccess = false;

            try
            {
                //base64문자열로 넘어온 바이너리 정보를 바이너리로 변환한다.
                //첫 콤마 위치 찾기
                int nComaIdx = item.Binary.IndexOf(",") + 1;
                byte[] bytes = Convert.FromBase64String(item.Binary.Substring(nComaIdx));

                //파일 저장 시작
                GlobalStatic.FileProc.FileSave(
                    FileDirType.OutputFileDir
                    , sNewFileFullName
                    , bytes);

                //파일 저장 성공
                bSaveSuccess = true;
                ++nSuccessCount;
            }
            catch(Exception ex)
            {
                GlobalStatic.Log
                    .LogError("FileDbProcess.Save", ex.ToString());
            }
            

            //db에 정보 추가
            using (ModelsDbContext db1 = new ModelsDbContext())
            {
                FileDbInfo newFile = new FileDbInfo();
                newFile.idBoardPost = idBoardPost;
                newFile.idBoardPostContents = idBoardPostContents;

                newFile.NameOri = item.Name;
                newFile.LengthOri = item.Length;
                newFile.Type = item.Type;
                if(true == bSaveSuccess)
                {
                    newFile.Description = item.Description;
                }
                else
                {//파일 저장 실패
                    newFile.Description += "(저장 실패)";
                    item.ErrorIs = true;
                }

                //생성된 고유이름 넣기
                newFile.Name = sNewFileName;

                //생성된 url 연결
                newFile.Url = sSaveFolderUrl + sNewFileName;
                //물리 경로 지정
                newFile.Location = sNewFileFullName;

                newFile.CreateDate = dtCallDate;
                newFile.FileDbState = ModelsDB_partial.FileDb.FileDbStateType.Normal;

                //테이블에 추가
                db1.FileDbInfo.Add(newFile);
                db1.SaveChanges();


                //새로 생성된 정보 전달.
                item.idFileInfo = newFile.idFileInfo;
                item.FileInfoName = sNewFileName;
            }//end using db1
        }//end foreach item


        return nSuccessCount;
    }
}
