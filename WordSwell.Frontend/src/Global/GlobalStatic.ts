import App from "..";
import HeosabiComponent from "../Faculty/Base/HeosabiComponent";
import PageComponent from "../Faculty/Base/PageComponent";
import EditorBase from "../Faculty/CustomEditor/EditorBase/EditorBase";
import { ButtonShowType, ButtonType } from "../Utility/DG_MessageBox2/ButtonEnumType";
import DG_MessageBox2 from "../Utility/DG_MessageBox2/DG_MessageBox2";
import DG_jsFileSelector2 from "../Utility/FileSelector/DG_jsFileSelector2";


export default class GlobalStatic
{
    static DG_MessageBox: DG_MessageBox2 = new DG_MessageBox2({});

    /** 전역 변수 */

    /** root */
    static app: App | null = null;
    /** 사용중인 페이지 개체 */
    static PageLayout: PageComponent | null = null;
    /** 지금 보고 있는 페이지 개체 */
    static PageNow: HeosabiComponent | null = null;
    /** 지금 보고 있는 페이지 주소 */
    static PageNowUrl: string = '';

    /** 에디터 Instance */
    static Editor: EditorBase;
    /** 에디터 Placholder */
    static EditorPlaceholder: string = "";
    /** 에디터 Mode */
    static EditorMode: 'wysiwyg' | 'markdown' = 'wysiwyg';
    /** 파일 셀렉터 */
    static FileSelector: DG_jsFileSelector2;

    static ImageToSeparator = (data: string): string =>
    {
        const regex = /<img\s+src="([^"]*)"\s+alt="([^\/]+)\/(\d+)"/g;
        const matches = [];
        let match;
        while ((match = regex.exec(data)) !== null)
        {
            const src = match[1];
            const fileName = match[2];
            const fileId = match[3];
            matches.push([src, fileName, fileId]);
        }

        // 변환된 문자열 생성
        let convertedHtmlString = data;
        for (const [src, fileName, fileId] of matches)
        {
            const replacement = `![${fileName}, ${fileId}]`;

            convertedHtmlString = convertedHtmlString.replace(
                `<img src="${src}" alt="${fileName}/${fileId}">`,
                replacement
            );
        }

        return convertedHtmlString;
    }

    static MessageBox_Success = ({
        sTitle,
        sMsg,
        funcOk,
    }: IMessageBoxParam): void =>
    {
        let sTitleTemp = sTitle;

        if ("" === sTitleTemp)
        {
            sTitleTemp = "오류";
        }

        GlobalStatic.DG_MessageBox.Show({
            Title: sTitleTemp,
            Content: sMsg,

            top: "center",

            ButtonShowType: ButtonShowType.Ok,

            Buttons: [{
                ButtonCss: "BtnLlightGreen",
                ButtonType: ButtonType.Ok,
                ButtonText: "확인",
            }],
            ButtonEvent: (btnType: number) =>
            {
                if (btnType === ButtonType.Ok)
                {
                    funcOk && funcOk();
                }

                GlobalStatic.DG_MessageBox.DG_Popup.Close();
            },
            ContentCss: 'CustomCss',
        })
    }

    static MessageBox_Confirm = ({
        sTitle,
        sMsg,
        funcOk,
    }: IMessageBoxParam): void =>
    {
        let sTitleTemp = sTitle;

        GlobalStatic.DG_MessageBox.Show({
            Title: sTitleTemp,
            Content: sMsg,

            top: "center",

            ButtonShowType: ButtonShowType.OkCancel,
            Buttons: [{
                ButtonCss: ['BtnBlue'],
                ButtonType: ButtonType.Ok,
                ButtonText: "확인",
            }, {
                ButtonCss: ['BtnGray'],
                ButtonType: ButtonType.Cancel,
                ButtonText: "취소",
            }],
            ButtonEvent: (btnType: number) =>
            {
                if (btnType === ButtonType.Ok)
                {
                    funcOk && funcOk();
                }

                GlobalStatic.DG_MessageBox.DG_Popup.Close();
            },
            ContentCss: 'CustomCss',
        })
    }

    static MessageBox_Error = ({
        sTitle,
        sMsg,
        funcOk,
    }: IMessageBoxParam): void =>
    {
        let sTitleTemp = sTitle;

        if ("" === sTitleTemp)
        {
            sTitleTemp = "오류";
        }

        GlobalStatic.DG_MessageBox.Show({
            Title: sTitleTemp,
            Content: sMsg,

            top: "center",

            ButtonShowType: ButtonShowType.Ok,
            Buttons: [{
                ButtonCss: ['BtnRed'],
                ButtonType: ButtonType.Ok,
                ButtonText: "확인",
            }],
            ButtonEvent: (btnType: number) =>
            {
                if (btnType === ButtonType.Ok)
                {
                    funcOk && funcOk();
                }

                GlobalStatic.DG_MessageBox.DG_Popup.Close();
            },
            ContentCss: 'CustomCss',
        })
    }

    /**
     * html 문자열을 인자로 받아서
     * DOM으로 생성해서 Return 해주는 함수이다.
     * @param {string} sHtml
     * @returns {Element}
     */
    static createDOMElement(sHtml: string): HTMLElement
    {
        const Template = document.createElement('template');
        Template.innerHTML = sHtml;

        return Template.content.children[0] as HTMLElement;
    }
}

interface IMessageBoxParam
{
    sTitle: string;
    sMsg: string;
    funcOk?: () => void;
}