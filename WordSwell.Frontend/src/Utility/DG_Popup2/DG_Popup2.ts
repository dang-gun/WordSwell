import { JsonShowDefaultOption } from "./JsonShowDefaultOption";

/**
 * Multiple analog
 * 다중 다얄로그
 * 다중 팝업 라이브러리
 * 기존 DG_Popup에 ES6 문법을 적용과 타입스크립트를 적용한 버전
 * 
 * Name: DG_Popup2
 * Version: 1.0.0
 * */
export default class DG_Popup2
{
    private ShowDefaultOption: JsonShowDefaultOption = {
        /**
         *  인덱스 지정.
         *  0이하의 값을 지정하면 자동으로 인덱스가 지정된다.
         *  인덱스가 중복되면 창을 생성하지 않고 기존 창을 찾아 연다.
         * */
        PopupIndex: 0,

        /** 시작위치 - Y */
        top: 'center',
        /** 시작위치 - X */
        left: 'center',
        /** 가로 크기 */
        width: "auto",
        /** 세로 크기 */
        height: "auto",
        /** 페이지 시작시 보고 있는 위치(예>스크롤로 위치가 바뀌는 경우)를 기준으로 표시할지 여부  */
        StartViewWeight: true,

        /** 부모에 적용할 css */
        ParentCss: "",
        /** 팝업이 완성되면 크기를 고정할지 여부
            이 옵션이 없으면 창이동시 크기가 변경될수 있다.
        */
        SizeFixed: false,

        /** 팝업 안에 표시할 컨탠츠
         * 오브젝트도 가능하다. */
        Content: "",
        /** 컨탠츠에 적용할 css */
        ContentCss: "",
        /** 컨탠츠에 적용할 배경색 */
        ContentBackground: "#fff",

        /**
         * 오버레이 클릭시 사용할 이벤트
         * null이면 오버레이를 클릭해도 동작하지 않는다.
         * 창을 닫으려면 'DG_Popup.CloseTarget(divPopupParent);'를 넣는다.
         *
         * function (nPopupIndex, divPopupParent)
         * nPopupIndex : 생성에 사용된 인덱스
         * divPopupParent : 생성된 창의 개체
         * */
        OverlayClick: null,
        /** 오버레이용 배경색 */
        OverlayBackground: "#aaa",
        /** 오버레이 불투명 값 */
        OverlayOpacity: 0.3,
        /** 오버레이에 적용할 css */
        OverlayCss: ""
    };

    /** z-index 시작 값 */
    private ZIndexStart: number = 1000;
    /** 다음 팝업의 z-index 추가 값 */
    private ZIndexAdd: number = 10;

    /** 생성된 팝업의 고유번호 발행용 */
    private PopupIndex: number = 0;

    /** 현재 선택된 인덱스 */
    private CurrentSelectIndex: number = 0;
    /** 현재 선택된 팝업 */
    private CurrentSelectDiv: HTMLDivElement | null = null;

    /**
     * 0: 없음
     * 1: 다운
     * 2: 업
     */
    private MouseState: number = 0;
    /** 마우스 다운 계산값 X */
    private MouseDownX: number = 0;
    /** 마우스 다운 계산값 Y */
    private MouseDownY: number = 0;

    /** 팝업이 생성되면 쌓이게 될 배열 */
    private List: HTMLDivElement[] = [];

    constructor(jsonShowDefaultOption: JsonShowDefaultOption)
    {
        // 기본 옵션을 사용자 옵션으로 덮어쓴다.
        this.ShowDefaultOption
            = Object.assign(this.ShowDefaultOption, jsonShowDefaultOption);

        const PopupTitleElement = document.querySelector(".DG_PopupTitle") as HTMLDivElement;
        document.addEventListener("mousedown", this.TitleMouseDownEvent);
        document.addEventListener("mousemove", this.TitleMouseMoveEvent);
        document.addEventListener("mouseup", this.TitleMouseUpEvent);
    }

    /**
     * DG_Popup2를 사용하여 팝업을 생성한다.
     */
    public Show(jsonOption: JsonShowDefaultOption): HTMLDivElement
    {
        const jsonShowDefaultOption = this.ShowDefaultOption;

        // 기본 옵션을 사용자 옵션으로 덮어쓴다.
        const NewJsonOption: JsonShowDefaultOption = Object.assign({}, jsonShowDefaultOption, jsonOption);

        // 고유키 증가
        let nPopupIndex: number = ++this.PopupIndex;

        // 고유키 확인
        if (0 < NewJsonOption.PopupIndex)
        {
            // 지정한 인덱스가 있으면 그 인덱스를 사용한다.
            nPopupIndex = NewJsonOption.PopupIndex;
        }
        else
        {
            // 지정한 인덱스가 없으면 새로운 인덱스를 사용한다.
            nPopupIndex = ++this.PopupIndex;
            NewJsonOption.PopupIndex = nPopupIndex;
        }

        // 사용할 z-index를 계산
        const nZIndex = this.ZIndexStart + (this.ZIndexAdd * this.List.length);

        // 부모용 div를 생성한다.
        const PopupParentElement: HTMLDivElement = this.CreatePopupParent(NewJsonOption, nPopupIndex);

        // Overlay를 생성한다.
        const PopupOverlayElement: HTMLDivElement = this.CreatePopupOverlay(NewJsonOption, nPopupIndex, nZIndex);

        // 컨텐츠용 div를 생성한다.
        const PopupContentElement: HTMLDivElement = this.CreatePopupContent(NewJsonOption, nPopupIndex, nZIndex);

        // 부모 div에 Overlay와 Content를 추가한다.
        PopupParentElement.appendChild(PopupContentElement);
        PopupParentElement.appendChild(PopupOverlayElement);

        // 부모 div를 body에 추가한다.
        document.body.appendChild(PopupParentElement);

        // 새롭게 추가한 팝업 개체를 찾는다.
        const NewPopupParentElement = document.querySelector("#divDG_PopupParent" + nPopupIndex) as HTMLDivElement;
        const NewPopupOverlayElement = document.querySelector("#divDG_PopupOverlay" + nPopupIndex) as HTMLDivElement;
        const NewPopupContentElement = document.querySelector("#divDG_Popup" + nPopupIndex) as HTMLDivElement;

        if (true === NewJsonOption.SizeFixed)
        {
            // 팝업이 완성되면 크기를 고정할지 여부

            // 완성된 크기를 가져온다.
            const nSize = NewPopupContentElement.offsetWidth;
            // 완성된 크기를 고정값으로 지정한다.
            NewPopupContentElement.style.width = nSize + "px";
        }

        // 센터 여부
        if ("center" === NewJsonOption.top)
        {
            // 센터일 경우
            // 중앙값을 계산한다.
            const nTopCenter
                = (window.innerHeight / 2)
                - (NewPopupContentElement.offsetHeight / 2)
                + NewPopupContentElement.getBoundingClientRect().top;

            NewPopupContentElement.style.top = nTopCenter + "px";
        }

        if ("center" === NewJsonOption.left)
        {
            // 센터일 경우
            // 중앙값을 계산한다.
            const nLeftCenter
                = (window.innerWidth / 2)
                - (NewPopupContentElement.offsetWidth / 2)
                + (NewPopupContentElement.getBoundingClientRect().left);

            NewPopupContentElement.style.left = nLeftCenter + "px";
        }

        // 빈 곳을 클릭했을 때 이벤트 적용
        if (typeof NewJsonOption.OverlayClick === "function")
        {
            NewPopupOverlayElement.addEventListener("click", () =>
            {
                NewJsonOption.OverlayClick!(nPopupIndex, NewPopupParentElement);
            });
        }

        // 배열에 추가한다.
        this.List.push(NewPopupParentElement);

        // 완성된 팝업을 리턴한다.
        return PopupParentElement;
    }

    /**
     * Css의 데이터값에 단위를 제거해준다.
     * @param {string} sData 변환할 데이터
     * @returns {number} 변환된 값
     */
    private CutBack(sData: string): number
    {
        let nReturn: number = 0;

        if (false === isNaN(Number(sData)))
        {
            // 숫자면 숫자로 변환
            nReturn = Number(sData);
        }
        else
        {
            // 숫자형이 아니라면
            // 단위는 끝에 2자리이다.
            if (2 < sData.length)
            {
                // 2자리 이상이면 뒤에서 2자리를 잘라서 숫자로 변환한다.
                const sCut = sData.substring(0, sData.length - 2);

                if (false === isNaN(Number(sCut)))
                {
                    // 남은 글자가 숫자면 숫자로 변환
                    nReturn = Number(sCut);
                }
            }
        }

        return nReturn;
    }

    /** 제일 마지막 팝업을 닫는다. */
    public Close()
    {
        // 리스트 개수
        const nIndex = this.List.length - 1;

        this.CloseIndex(nIndex);
    }

    /**
     * 지정한 인덱스의 팝업을 닫는다.
     * @param {number} nIndex 닫을 인덱스
     */
    private CloseIndex(nIndex: number)
    {
        // 팝업 개체 삭제
        this.List[nIndex].remove();

        // 리스트에서 해당 인덱스 삭제
        this.List.splice(nIndex, 1);
    }

    /** 모든 팝업을 닫는다. */
    public CloseAll()
    {
        const nArrayLength = this.List.length;

        for (let i = 0; i < nArrayLength; i++)
        {
            this.Close();
        }
    }

    /**
     * 지정한 대상을 닫는다.
     * @param {HTMLDivElement} objTarget 닫을 대상
     */
    public CloseTarget(objTarget: HTMLDivElement)
    {
        const nArrayLength = this.List.length;

        for (let i = 0; i < nArrayLength; i++)
        {
            if (true === Object.is(this.List[i], objTarget))
            {
                // 일치하면 오브젝트를 찾아서 닫는다.
                this.CloseIndex(i);
                break;
            }
        }
    }

    /**
     * 팝업의 부모 Element를 생성한다.
     * @param {JsonShowDefaultOption} NewJsonOption 사용자 정의 옵션
     * @param {number} nPopupIndex 팝업 인덱스
     * @returns {HTMLDivElement} 생성된 부모 Element
     */
    private CreatePopupParent(NewJsonOption: JsonShowDefaultOption, nPopupIndex: number): HTMLDivElement
    {
        // 부모용 div를 생성한다.
        const PopupParentElement = document.createElement("div");
        PopupParentElement.id = "divDG_PopupParent" + nPopupIndex;
        PopupParentElement.classList.add("DG_PopupParentCss");
        PopupParentElement.dataset.popupIndex = nPopupIndex.toString();

        // 사용자 정의 CSS 추가
        if ("" !== NewJsonOption.ParentCss)
        {
            PopupParentElement.classList.add(NewJsonOption.ParentCss);
        }

        return PopupParentElement;
    }

    /**
     * 팝업의 Overlay Element를 생성한다.
     * @param {JsonShowDefaultOption} NewJsonOption 사용자 정의 옵션
     * @param {number} nPopupIndex 팝업 인덱스
     * @param {number} nZIndex z-index
     * @returns {HTMLDivElement} 생성된 Overlay Element
     */
    private CreatePopupOverlay(NewJsonOption: JsonShowDefaultOption, nPopupIndex: number, nZIndex: number): HTMLDivElement
    {
        // Overlay를 생성한다.
        const PopupOverlayElement = document.createElement("div");
        PopupOverlayElement.id = "divDG_PopupOverlay" + nPopupIndex;
        PopupOverlayElement.classList.add("DG_PopupOverlayCss");

        // 사용자 정의 CSS 추가
        if ("" !== NewJsonOption.OverlayCss)
        {
            PopupOverlayElement.classList.add(NewJsonOption.OverlayCss);
        }

        // 배경색 지정
        PopupOverlayElement.style.background = NewJsonOption.OverlayBackground;
        // 투명도 지정
        PopupOverlayElement.style.opacity = NewJsonOption.OverlayOpacity.toString();

        // 포지션 지정
        PopupOverlayElement.style.position = "fixed";
        // 포지션 위치 지정
        PopupOverlayElement.style.top = "0";
        PopupOverlayElement.style.left = "0";
        // 크기 지정
        PopupOverlayElement.style.width = "100%";
        PopupOverlayElement.style.height = "100%";

        // z-index 지정
        PopupOverlayElement.style.zIndex = nZIndex.toString();

        return PopupOverlayElement;
    }

    /**
     * 팝업의 컨텐츠 Element를 생성한다.
     * @param {JsonShowDefaultOption} NewJsonOption 사용자 정의 옵션
     * @param {number} nPopupIndex 팝업 인덱스
     * @param {number} nZIndex z-index
     * @returns {HTMLDivElement} 생성된 컨텐츠 Element
     */
    private CreatePopupContent(NewJsonOption: JsonShowDefaultOption, nPopupIndex: number, nZIndex: number): HTMLDivElement
    {
        // 컨텐츠용 div를 생성한다.
        const PopupContentElement = document.createElement("div");
        PopupContentElement.id = "divDG_Popup" + nPopupIndex;
        PopupContentElement.classList.add("DG_PopupContentCss");
        PopupContentElement.dataset.popupIndex = nPopupIndex.toString();

        // 사용자 정의 CSS 추가
        if ("" !== NewJsonOption.ContentCss)
        {
            PopupContentElement.classList.add(NewJsonOption.ContentCss);
        }

        // 포지션 지정
        PopupContentElement.style.position = "absolute";

        // 배경색 지정
        PopupContentElement.style.background = NewJsonOption.ContentBackground;

        let nTop = NewJsonOption.top;
        let nLeft = NewJsonOption.left;

        // 센터 여부
        if ("center" === NewJsonOption.top)
        {
            // 센터일 경우
            // 우선 0으로 초기화 한다.
            nTop = 0;
        }
        if ("center" === NewJsonOption.left)
        {
            // 센터일 경우
            // 우선 0으로 초기화 한다.
            nLeft = 0;
        }

        if (true === NewJsonOption.StartViewWeight)
        {
            // 페이지 시작시 보고 있는 위치(예>스크롤로 위치가 바뀌는 경우)를 기준으로 표시할지 여부
            // 스크롤 위치를 더한다.
            if (typeof nTop === "number" && typeof nLeft === "number")
            {
                nTop += (window.scrollY * 2);
                nLeft += (window.scrollX * 2);
            }
        }

        // 시작 위치 지정
        PopupContentElement.style.top = nTop + "px";
        PopupContentElement.style.left = nLeft + "px";
        // 크기 지정
        PopupContentElement.style.width = NewJsonOption.width;
        PopupContentElement.style.height = NewJsonOption.height;
        // z-index 지정
        PopupContentElement.style.zIndex = (nZIndex + 1).toString();

        // HTML 출력
        if (typeof NewJsonOption.Content === "string")
        {
            // 문자열이면 그대로 출력
            PopupContentElement.innerHTML = NewJsonOption.Content;
        }
        else if (typeof NewJsonOption.Content === "object")
        {
            // 오브젝트일 경우
            PopupContentElement.appendChild(NewJsonOption.Content);
        }

        return PopupContentElement;
    }

    /**
     * 팝업의 타이틀을 마우스로 클릭했을 때 이벤트
     * @param {MouseEvent} event 이벤트 객체
     * @returns {void}
     */
    private TitleMouseDownEvent = (event: MouseEvent): void =>
    {
        const target = event.target as HTMLDivElement;
        // 타겟이 div.DG_PopupTitle이 아닐 경우 이벤트를 발생시키지 않는다.
        if (!target.classList.contains('DG_PopupTitle'))
        {
            return;
        }

        const Parent = target.parentElement as HTMLDivElement;
        const SelectDiv = Parent.parentElement.parentElement as HTMLDivElement;

        // 좌표가 수정될 상위 DOM을 찾는다.
        // this.CurrentSelectDiv = event.target.parentElement.parentElement;
        this.CurrentSelectDiv = SelectDiv;
        this.CurrentSelectIndex = this.CurrentSelectDiv.getAttribute("data-index") as unknown as number;

        // 마우스 상태 변경
        this.MouseState = 1;

        // 마우스 다운 계산값
        this.MouseDownX = event.clientX - this.CutBack(this.CurrentSelectDiv.style.left);
        this.MouseDownY = event.clientY - this.CutBack(this.CurrentSelectDiv.style.top);
    };

    /**
     * 팝업의 타이틀을 마우스로 클릭한 상태에서 이동했을 때 이벤트
     * @param {MouseEvent} event 이벤트 객체
     * @returns {void}
     */
    private TitleMouseMoveEvent = (event: MouseEvent): void =>
    {
        if (1 === this.MouseState)
        {
            // 마우스 다운 상태
            // 창 위치 변경
            this.CurrentSelectDiv.style.left = (event.clientX - this.MouseDownX) + "px";
            this.CurrentSelectDiv.style.top = (event.clientY - this.MouseDownY) + "px";
        }
    };

    /**
     * 팝업의 타이틀을 마우스로 클릭한 상태에서 뗐을 때 이벤트
     * @param {MouseEvent} event 이벤트 객체
     * @returns {void}
     */
    private TitleMouseUpEvent = (event: MouseEvent): void =>
    {
        // 마우스 상태 변경
        this.MouseState = 2;
    };
}
