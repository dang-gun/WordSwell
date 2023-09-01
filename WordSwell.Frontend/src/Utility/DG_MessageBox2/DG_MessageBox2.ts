import { MessageBoxShowDefaultOption } from './MessageBoxShowDefaultOption';
import DG_Popup2 from '../DG_Popup2/DG_Popup2';
import { ButtonItem } from './ButtonItem';
import { BigIconType, ButtonShowType, ButtonType } from './ButtonEnumType';

export default class DG_MessageBox2
{
    private ShowDefaultOption: MessageBoxShowDefaultOption = {
        /** 시작위치 - Y */
        top: "center",
        /** 시작위치 - X */
        left: "center",

        /** 팝업이 완성되면 크기를 고정할지 여부 
            이 옵션이 없으면 창이동시 크기가 변경될수 있다.
        */
        SizeFixed: true,

        //제목
        Title: "",
        //내용
        Content: "",

        //큰 아이콘 타입
        BigIconType: BigIconType.None,

        //버튼 타입
        ButtonShowType: ButtonShowType.Ok,

        //버튼 이벤트
        //function (DG_MessageBox.ButtonType)
        //DG_MessageBox.ButtonType : 클릭된 버튼 정보
        ButtonEvent: null,

        //컨탠츠에 적용할 css
        ContentCss: "DG_MessageBoxContentCss"
    };

    // 사용할 div element
    private MessageBoxElement: HTMLDivElement | null = null;

    public DG_Popup: DG_Popup2 = new DG_Popup2({});

    constructor(jsonShowDefaultOption: MessageBoxShowDefaultOption)
    {
        // 기본 옵션을 사용자 옵션으로 덮어쓴다.
        this.ShowDefaultOption = Object.assign({}, this.ShowDefaultOption, jsonShowDefaultOption);
    }

    /**
     * 미리 만들어진 메세지 박스를 출력한다.
     * @param {JsonShowDefaultOption} jsonOption 사용자 정의 옵션
     */
    public Show(jsonOption: MessageBoxShowDefaultOption)
    {
        // 기본 옵션을 사용자 옵션으로 덮어쓴다.
        const NewJsonOption: MessageBoxShowDefaultOption = Object.assign({}, this.ShowDefaultOption, jsonOption);
        const JsonOutPut: MessageBoxShowDefaultOption = {
            /** 시작위치 - Y */
            top: NewJsonOption.top,
            /** 시작위치 - X */
            left: NewJsonOption.left,

            /** 팝업이 완성되면 크기를 고정할지 여부 
                이 옵션이 없으면 창이동시 크기가 변경될수 있다.
            */
            SizeFixed: NewJsonOption.SizeFixed,

            //제목
            Title: NewJsonOption.Title,
            //내용
            Content: NewJsonOption.Content,

            //버튼 정보 배열
            //ButtonCss : 추가할 css
            //ButtonType : 버튼의 타입, DG_MessageBox.ButtonType
            //ButtonText : 표시할 텍스트
            Buttons: NewJsonOption.Buttons,

            //버튼 이벤트
            //function (DG_MessageBox.ButtonType)
            //DG_MessageBox.ButtonType : 클릭된 버튼 정보
            ButtonEvent: NewJsonOption.ButtonEvent,

            //컨탠츠에 적용할 css
            ContentCss: NewJsonOption.ContentCss
        };

        // BicIconType
        switch (NewJsonOption.BigIconType)
        {
            case BigIconType.Info:
                JsonOutPut.BigIconCss = "DG_MessageBoxBigIcon_info";
                break;
            case BigIconType.Warning:
                JsonOutPut.BigIconCss = "DG_MessageBoxBigIcon_Warning";
                break;
            case BigIconType.Error:
                JsonOutPut.BigIconCss = "DG_MessageBoxBigIcon_Error";
                break;
            case BigIconType.Question:
                JsonOutPut.BigIconCss = "DG_MessageBoxBigIcon_Question";
                break;
            case BigIconType.Success:
                JsonOutPut.BigIconCss = "DG_MessageBoxBigIcon_Success";
                break;
            case BigIconType.Help:
                JsonOutPut.BigIconCss = "DG_MessageBoxBigIcon_Help";
                break;
            case BigIconType.Invisible:
                JsonOutPut.BigIconCss = "DG_MessageBoxBigIcon_Invisible";
                break;
            case BigIconType.None:
            default:
                JsonOutPut.BigIconCss = "DG_MessageBoxBigIcon_None";
                break;
        }

        if (NewJsonOption.Buttons.length > 0)
        {
            this.ShowBox(JsonOutPut);
            return;
        }

        // 표시 버튼 타입
        switch (NewJsonOption.ButtonShowType)
        {
            // 표시 버튼 타입이 OkCacnel
            case ButtonShowType.OkCancel:
                // Ok 버튼 추가
                JsonOutPut.Buttons.push({
                    ButtonCss: "BtnBlue",
                    ButtonType: ButtonType.Ok,
                    ButtonText: "OK",
                });
                // Cancel 버튼 추가
                JsonOutPut.Buttons.push({
                    ButtonCss: "BtnRed",
                    ButtonType: ButtonType.Cancel,
                    ButtonText: "Cancel",
                });
                break;
            case ButtonShowType.Cancel:
                // Cancel 버튼 추가
                JsonOutPut.Buttons.push({
                    ButtonCss: "BtnRed",
                    ButtonType: ButtonType.Cancel,
                    ButtonText: "Cancel",
                });
                break;
            case ButtonShowType.YesNo:
                // Yes 버튼 추가
                JsonOutPut.Buttons.push({
                    ButtonCss: "BtnLightGreen",
                    ButtonType: ButtonType.Yes,
                    ButtonText: "Yes",
                });
                // No 버튼 추가
                JsonOutPut.Buttons.push({
                    ButtonCss: "BtnOrange",
                    ButtonType: ButtonType.No,
                    ButtonText: "No",
                });
                break;
            case ButtonShowType.Ok:
                JsonOutPut.Buttons.push({
                    ButtonCss: "BtnBlue",
                    ButtonType: ButtonType.Ok,
                    ButtonText: "OK",
                });
                break;
            default:
                // Ok 버튼 추가
                JsonOutPut.Buttons.push({
                    ButtonCss: "BtnBlue",
                    ButtonType: ButtonType.Ok,
                    ButtonText: "OK",
                });
                break;
        }

        this.ShowBox(JsonOutPut);
    }

    /**
     * 메시지 박스를 표시한다.
     * @param {JsonShowDefaultOption} jsonOption 창옵션
     */
    public ShowBox(jsonOption: MessageBoxShowDefaultOption)
    {
        const JsonOptionDefault: MessageBoxShowDefaultOption = {
            /** 팝업이 완성되면 크기를 고정할지 여부 
            이 옵션이 없으면 창이동시 크기가 변경될수 있다.
            */
            SizeFixed: false,

            //제목
            Title: "",
            //내용
            Content: "",

            //큰 아이콘으로 사용할 css
            BigIconCss: "",

            //버튼 정보 배열
            //ButtonCss : 추가할 css
            //ButtonType : 버튼의 타입, DG_MessageBox.ButtonType
            //ButtonText : 표시할 텍스트
            Buttons: [],

            //버튼 이벤트
            //function (DG_MessageBox.ButtonType)
            //DG_MessageBox.ButtonType : 클릭된 버튼 정보
            ButtonEvent: null,

            //컨탠츠에 적용할 css
            ContentCss: "DG_MessageBoxContentCss"
        };

        // 기본 옵션을 사용자 옵션으로 덮어쓴다.
        const NewJsonOption: MessageBoxShowDefaultOption = Object.assign({}, JsonOptionDefault, jsonOption);
        const JsonTossOption: MessageBoxShowDefaultOption = {
            /** 시작위치 - Y */
            top: NewJsonOption.top,
            /** 시작위치 - X */
            left: NewJsonOption.left,
            /** 보고 있는 위치 기준 창띄우기 */
            StartViewWeight: true,

            /** 팝업이 완성되면 크기를 고정할지 여부
                이 옵션이 없으면 창이동시 크기가 변경될수 있다.
            */
            SizeFixed: NewJsonOption.SizeFixed,

            /** 팝업 안에 표시할 컨탠츠
             * 오브젝트도 가능하다. */
            Content: "",
            /** 컨탠츠에 적용할 css */
            ContentCss: NewJsonOption.ContentCss
        };

        // 엘리먼트를 추가할 비어 있는 Fragment를 생성한다.
        const Fragment: DocumentFragment = document.createDocumentFragment();

        // 타이틀 엘리먼트를 생성한다.
        const TitleElement: HTMLDivElement = document.createElement("div");
        TitleElement.classList.add("DG_PopupTitle");
        TitleElement.classList.add("DG_MessageBoxTitle");
        TitleElement.textContent = NewJsonOption.Title;

        // 컨텐츠 엘리먼트를 생성한다.
        const ContentElement: HTMLDivElement = document.createElement("div");
        ContentElement.classList.add("DG_MessageBoxContent");

        // 컨텐츠에 적용할 BigIcon을 생성한다.
        const BigIconElement: HTMLDivElement = document.createElement("div");

        // 컨텐츠 엘리먼트 기본 css
        let cssContentHtml: string = "DG_MessageBoxContentBigIconMargin";

        if (NewJsonOption.BigIconCss)
        {
            // BigIconCss가 있으면 적용한다.
            BigIconElement.classList.add(NewJsonOption.BigIconCss);

            if ("DG_MessageBoxBigIcon_None" === NewJsonOption.BigIconCss)
            {
                // 아이콘 표시가 없으면 마진을 제거
                cssContentHtml = "";
            }
        }
        else
        {
            cssContentHtml = "DG_MessageBoxContentHtml";
        }

        // BigIcon을 컨텐츠에 추가한다.
        ContentElement.appendChild(BigIconElement);

        // 컨텐츠 html 내용
        const ContentElementHtml: HTMLDivElement = document.createElement("div");
        // ContentElement.className = cssContentHtml;

        if (typeof NewJsonOption.Content === 'string')
        {
            ContentElementHtml.innerHTML = NewJsonOption.Content;
        }

        ContentElement.appendChild(ContentElementHtml);

        //푸터****************************************
        const FooterElement: HTMLDivElement = document.createElement("div");
        FooterElement.classList.add("DG_MessageBoxFooter");

        // 버튼 추가
        for (let i = 0; i < NewJsonOption.Buttons.length; i++)
        {
            // 현재 버튼 정보
            const ButtonItem: ButtonItem | string = NewJsonOption.Buttons[i];

            // 추가할 버튼
            const ButtonElement: HTMLButtonElement = document.createElement("button");

            if (typeof ButtonItem.ButtonCss === 'string')
            {
                ButtonElement.classList.add(ButtonItem.ButtonCss);
            }

            if (Array.isArray(ButtonItem.ButtonCss))
            {
                ButtonElement.classList.add(...ButtonItem.ButtonCss);
            }

            ButtonElement.dataset.buttonType = ButtonItem.ButtonType.toString();
            ButtonElement.textContent = ButtonItem.ButtonText;
            ButtonElement.addEventListener("click", (event: MouseEvent) =>
            {
                const target = event.target as HTMLButtonElement;
                NewJsonOption.ButtonEvent(Number(target.dataset.buttonType));
            });

            // 버튼을 푸터에 추가한다.
            FooterElement.appendChild(ButtonElement);
        }

        // 최종 엘리먼트를 생성하고 Fragment에 추가한다.
        Fragment.appendChild(TitleElement);
        Fragment.appendChild(ContentElement);
        Fragment.appendChild(FooterElement);

        JsonTossOption.Content = Fragment;

        // 메세지 박스를 표시한다.
        this.DG_Popup.Show(JsonTossOption);
    }
}
