/** DB에 저장된 파일정보를 주고, 받기위한 모델 */
export interface FileItemInterface 
{
    /** 파일 이름 */
    Name: string,
    /** 확장자 */
    Extension: string,
    /** 파일 크기 - Size -> Length */
    Length: number,
    /** 파일 타입 정보 */
    Type: string,
    /** 파일에 대한 설명 */
    Description: string,
    /** 에디터에서 사용될 파일 구분값 */
    EditorDivision: string,
    /** 바이너리 정보를 사용할지 여부.
            이것이 true이면 동적으로 바이너리 정보를 읽어 미리보기이미지로 출력하게 된다.
            이미 처리된 데이터인경우 이것이 false가 되어야 한다. */
    BinaryIs: boolean,
    /** 바이너리 정보를 사용할때 바이너리 데이터가 준비가 끝났는지 여부 */
    BinaryReadyIs: boolean,
    /** 로컬파일인 경우 파일의 바이너리 정보 */
    Binary: ReadableStream<Uint8Array> | ArrayBuffer | string,
    /** 로컬 고유 번호. 
            프론트앤드에서 업로드되지 않은 파일을 구분하기위한 고유값 */
    idLocal: number,
    /** 파일이 업로드되어 있을때 고유 번호(idFileInfo, 업로드된 파일이 아니면 0) */
    idFileInfo: number,
    /** 파일을 업로드하기위해 생성한 고유 이름 */
    FileInfoName: string,
    /** 파일이 업로드 되어 있는 상태에 가지고 있는 url */
    Url: string,
    /** 어떤 이유에서든 서버에 파일이 업로드가 실패한 파일이다. */
    ErrorIs: boolean,
    /** 수정 여부 */
    EditIs: boolean,
    /** 삭제 여부 */
    DeleteIs: boolean,
}