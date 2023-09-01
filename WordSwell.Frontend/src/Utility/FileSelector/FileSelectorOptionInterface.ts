import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { FileItemInterface } from "./FileItemInterface";

export interface FileSelectorOptionInterface
{
    /** 디버그 활성화 여부 */
    Debug: boolean,

    /** 드롭다운 영역이자 아이템 표시에 사용할 영역*/
    Area: null | HTMLElement,

    /** 아이템이 그려질 완성된 영역 */
    Area_ItemList?: null | HTMLElement,

    /** 파일 갯수 제한.
     * -1이면 무제한
     */
    MaxFileCount: number,

    /**
     * 에디터 인스턴스
     */
    Editor: ClassicEditor,

    /**
     * 허용된 확장자 리스트(소문자로 입력).
     * 전체허용은 "*.*"
     *  이미지 : ".bmp", ".dib", ".jpg", ".jpeg", ".jpe", ".gif", ".png", ".tif", ".tiff", ".raw"
     */
    ExtAllow?: (".bmp" | ".dib" | ".jpg" | ".jpeg" | ".jpe" | ".gif" | ".png" | ".tif" | ".tiff" | ".raw" | "*.*")[],

    /** 로딩 이미지에 사용할 이미지 */
    LoadingSrc?: string,

    /** 별도 처리 없는(확장자 검사에서 빈값) 이미지에 사용할 이미지 */
    NoneFileImgUrl?: string,

    /** 파일 사이즈를 문자열로 바꿀때 SI(국제 단위계) 사용 여부 */
    FileSizeToStringUseSI?: boolean,

    ExtToImg?: (sExt: string) => string,

    closeBtn?: string,

    /**
     * 선택한 파일의 모든 처리가 끝났을때 이벤트
     */
    LoadComplete?: () => void;

    FileSizeToString?: (nSizeLength: number, bSI: boolean) => string;

    DeleteComplete?: (file: FileItemInterface, dom: HTMLLIElement) => void;
}