import { FileDbStateType } from '@/Faculty/Backend/ModelsDB_partial/FileDb/FileDbStateType';

/** 파일DB 처리 */
export interface FileDb 
{
    /** 파일DB 고유번호 */
    idFileDb: number,
    /** 종속된 게시물의 고유 번호 */
    idBoardPost: number,
    /** 원본 이름 */
    NameOri: string,
    /** 원본 파일 용량 */
    LengthOri: number,
    /** 파일 타입 */
    Type: string,
    /** 파일 설명 */
    Description: string,
    /** 썸네일로 사용할 주소.
            파일 크기별로 다른 이미지를 사용할 경우 
            이 이름을 기반으로 크기 이름을 정해 사용한다. */
    ThumbnailName: string,
    /** 최종 생성된 주소.(상대 주소)
            이미지인경우 표시 주소, 파일인 경우 다운로드 주소가 된다. */
    Url: string,
    /** 최종 저장된 파일의 물리위치(상대 주소) */
    Location: string,
    /** 파일 생성 날짜 */
    CreateDate: Date,
    /** 파일 상태 */
    FileDbState: FileDbStateType,
    /** 어떤 이유에서든 서버에 파일이 업로드가 실패한 파일이다. */
    ErrorIs: boolean,
    /** 마지막으로 수정한 유저 번호 */
    idUser_Edit?: number,
    /** 수정 시간 */
    EditTime?: Date,
}