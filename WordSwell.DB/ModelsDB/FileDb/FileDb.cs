
using System.ComponentModel.DataAnnotations;

using ModelsDB_partial.FileDb;

namespace ModelsDB.FileDb;

/// <summary>
/// 파일DB 처리
/// </summary>
public class FileDb
{
    /// <summary>
    /// 파일DB 고유번호
    /// </summary>
    [Key]
    public long idFileDb { get; set; }

    /// <summary>
    /// 종속된 게시물의 고유 번호
    /// </summary>
    public long idBoardPost { get; set; }

    /// <summary>
    /// 원본 이름
    /// </summary>
    /// <remarks>
    /// 리눅스의 파일 이름 최대크기는 255이다.
    /// </remarks>
    [MaxLength(255)]
    public string NameOri { get; set; } = string.Empty;
    /// <summary>
    /// 원본 파일 용량
    /// </summary>
    /// <remarks>
    /// 나중에 파일의 가로세로 크기를 따로 저장할때 size로 이름을 붙이면
    /// 혼란이 있을거 같아 길이(length)로 정함
    /// </remarks>
    public long LengthOri { get; set; }

    /// <summary>
    /// 파일 타입
    /// </summary>
    /// <remarks>
    /// 확장자를 말하는게 아니라 프론트엔드에서 판단된 파일의 종류를 말한다.
    /// </remarks>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 파일 설명
    /// </summary>
    public string Description { get; set; } = string.Empty;


    /// <summary>
    /// 썸네일로 사용할 주소.
    /// 파일 크기별로 다른 이미지를 사용할 경우 
    /// 이 이름을 기반으로 크기 이름을 정해 사용한다.
    /// </summary>
    public string ThumbnailName { get; set; } = string.Empty;

    /// <summary>
    /// 최종 생성된 주소.(상대 주소)
    /// 이미지인경우 표시 주소, 파일인 경우 다운로드 주소가 된다.
    /// </summary>
    public string Url { get; set; } = string.Empty;
    /// <summary>
    /// 최종 저장된 파일의 물리위치(상대 주소)
    /// </summary>
    /// <remarks>
    /// 프론트엔드의 root를 기준으로 작성되어야 한다.
    /// 그래야 나중에 프론트엔드의 위치가 변경되도 바로 적용할 수 있다.
    /// </remarks>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 파일 생성 날짜
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 파일 상태
    /// </summary>
    public FileDbStateType FileDbState { get; set; } = FileDbStateType.None;

    /// <summary>
    /// 어떤 이유에서든 서버에 파일이 업로드가 실패한 파일이다.
    /// </summary>
    /// <remarks>
    /// 자동으로 삭제할지 여부는 나중에 판단한다.
    /// </remarks>
    public bool ErrorIs { get; set; }

    /// <summary>
    /// 마지막으로 수정한 유저 번호
    /// </summary>
    /// <remarks>
    /// 0 = 비회원<br />
    /// 관리자에 의한 삭제, 수정, 블럭의 경우 다른 사람의 고유번호가 들어갈 수 있다.
    /// </remarks>
    public long? idUser_Edit { get; set; }
    /// <summary>
    /// 수정 시간
    /// </summary>
    /// <remarks>
    /// 파일은 수정개념이 없으므로 이건 상태변경을 의미한다.
    /// </remarks>
    public DateTime? EditTime { get; set; }
}
