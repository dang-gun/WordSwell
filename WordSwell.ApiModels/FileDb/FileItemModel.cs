namespace WordSwell.ApiModels.FileDb;

/// <summary>
/// DB에 저장된 파일정보를 주고, 받기위한 모델
/// </summary>
public class FileItemModel
{

    /// <summary>
    /// 파일 이름
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 확장자
    /// </summary>
    public string Extension { get; set; } = string.Empty;
    /// <summary>
    /// 파일 크기 - Size -> Length
    /// </summary>
    public long Length { get; set; }
    /// <summary>
    /// 파일 타입 정보
    /// </summary>
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// 파일에 대한 설명
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 에디터에서 사용될 파일 구분값
    /// </summary>
    public string EditorDivision { get; set; } = string.Empty;

    /// <summary>
    /// 바이너리 정보를 사용할지 여부.
    /// 이것이 true이면 동적으로 바이너리 정보를 읽어 미리보기이미지로 출력하게 된다.
    /// 이미 처리된 데이터인경우 이것이 false가 되어야 한다.
    /// </summary>
    public bool BinaryIs { get; set; }
    /// <summary>
    /// 바이너리 정보를 사용할때 바이너리 데이터가 준비가 끝났는지 여부
    /// </summary>
    public bool BinaryReadyIs { get; set; }

    /** 로컬파일인 경우 파일의 바이너리 정보 */
    public string Binary { get; set; } = string.Empty;

    /// <summary>
    /// 파일이 업로드되어 있을때 고유 번호
    /// </summary>
    public long idFile { get; set; }
    /// <summary>
    /// 로컬 고유 번호
    /// </summary>
    public long idLocal { get; set; }

    /// <summary>
    /// 파일이 업로드 되어 있는 상태에 가지고 있는 url
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 수정 여부
    /// </summary>
    public bool EditIs { get; set; }
    /// <summary>
    /// 삭제 여부
    /// </summary>
    public bool DeleteIs { get; set; }
}

