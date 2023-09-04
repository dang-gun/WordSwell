export interface FileItemInterface
{
    /** 파일 이름 */
    Name: string,
    /** 파일 확장자 */
    Extension: string,
    /** 파일 크기 */
    Length: number,
    /** 파일 타입 정보 */
    Type: string,
    /** 파일에 대한 설명 */
    Description: string,

    /** 에디터에서 사용될 파일 구분값 */
    EditorDivision: string,

    /** 
     *  바이너리 정보를 사용할지 여부.
     *  이것이 true이면 동적으로 바이너리 정보를 읽어 미리보기이미지로 출력하게 된다.
     *  이미 처리된 데이터인경우 이것이 false가 되어야 한다.
     * */
    BinaryIs: boolean,

    /** 바이너리 정보를 사용할때 바이너리 데이터가 준비가 끝났는지 여부 */
    BinaryReadyIs: boolean,

    /** 로컬파일인 경우 파일의 바이너리 정보 */
    Binary: ReadableStream<Uint8Array> | ArrayBuffer | string,

    /** 파일이 업로드되어 있을때 고유 번호  */
    idFile: number,

    /** 로컬 고유 번호 */
    idLocal: number,

    /** 파일이 업로드 되어 있는 상태에 가지고 있는 url */
    Url: string,

    /** 수정 여부 */
    EditIs: boolean,

    /** 삭제 여부 */
    DeleteIs: boolean,
}
