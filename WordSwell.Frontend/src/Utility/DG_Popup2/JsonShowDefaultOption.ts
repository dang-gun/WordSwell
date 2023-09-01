export interface JsonShowDefaultOption
{
    /**
     * 인덱스 지정
     * 0 이하의 값을 지정하면 자동으로 인덱스가 지정됩니다.
     * 인덱스가 중복되면 창을 생성하지 않고 기존 창을 찾아서 열어줍니다.
     */
    PopupIndex?: number;

    /** 시작위치 - Y */
    top?: number | 'center',

    /** 시작위치 - X */
    left?: number | 'center',

    /** 가로 크기 */
    width?: string,

    /** 세로 크기 */
    height?: string,

    /** 페이지 시작시 보고 있는 위치(예>스크롤로 위치가 바뀌는 경우)를 기준으로 표시할지 여부  */
    StartViewWeight?: boolean,

    /** 부모에 적용할 css */
    ParentCss?: string,

    /**
     * 팝업이 완성되면 크기를 고정할지 여부
     * 이 옵션이 없으면 창 이동시 크기가 변경될 수 있다.
     */
    SizeFixed?: boolean,

    /**
     * 팝업 안에 표시할 컨텐츠
     * 오브젝트도 가능하다.
     */
    Content?: string | DocumentFragment | HTMLElement,

    /** 컨탠츠에 적용할 css */
    ContentCss?: string,

    /** 컨탠츠에 적용할 배경색 */
    ContentBackground?: string,

    /**
     * 오버레이 클릭시 사용할 이벤트
     * null이면 오버레이를 클릭해도 동작하지 않는다.
     * 창을 닫으려면 'DG_Popup.CloseTarget(divPopupParent);'를 넣는다.
     * 
     * function (nPopupIndex, divPopupParent)
     * nPopupIndex : 생성에 사용된 인덱스
     * divPopupParent : 생성된 창의 개체 
     * */
    OverlayClick?: (nPopupIndex: number, divPopupParent: HTMLDivElement) => void,

    /** 오버레이용 배경색 */
    OverlayBackground?: string,

    /** 오버레이 불투명 값 */
    OverlayOpacity?: number,

    /** 오버레이에 적용할 css */
    OverlayCss?: string;
}
