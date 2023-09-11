import { FileDbInfo } from "@/Faculty/Backend/ModelsDB/FileDb/FileDbInfo";
import App from "..";
import HeosabiComponent from "../Faculty/Base/HeosabiComponent";
import PageComponent from "../Faculty/Base/PageComponent";
import EditorBase from "../Faculty/CustomEditor/EditorBase/EditorBase";
import { ButtonShowType, ButtonType } from "../Utility/DG_MessageBox2/ButtonEnumType";
import DG_MessageBox2 from "../Utility/DG_MessageBox2/DG_MessageBox2";
import DG_jsFileSelector2 from "../Utility/FileSelector/DG_jsFileSelector2";
import { FileItemInterface } from "@/Utility/FileSelector/FileItemInterface";


export default class GlobalStatic
{
    static DG_MessageBox: DG_MessageBox2 = new DG_MessageBox2({});

    /** ���� ���� */

    /** root */
    static app: App | null = null;
    /** ������� ������ ��ü */
    static PageLayout: PageComponent | null = null;
    /** ���� ���� �ִ� ������ ��ü */
    static PageNow: HeosabiComponent | null = null;
    /** ���� ���� �ִ� ������ �ּ� */
    static PageNowUrl: string = '';

    /** ������ Instance */
    static Editor: EditorBase;
    /** ������ Placholder */
    static EditorPlaceholder: string = "";
    /** ������ Mode */
    static EditorMode: 'wysiwyg' | 'markdown' = 'wysiwyg';
    /** ���� ������ */
    static FileSelector: DG_jsFileSelector2;

    static ImageToSeparator = (data: string): string =>
    {
        const regex = /\<img\s+src="([^"]*)"\s+alt="([^"]*)"/g;
        const matches = [];

        let match;
        while ((match = regex.exec(data)) !== null)
        {
            const src = match[1];
            const fileName = (match[2] as string).split('/')[0];
            const editorDivision = (match[2] as string).split('/')[1];
            matches.push([src, fileName, editorDivision]);
        }

        // ��ȯ�� ���ڿ� ����
        let convertedHtmlString = data;
        for (const [src, fileName, editorDivision] of matches)
        {
            let replacement = `![${fileName}]`;

            if (!editorDivision)
            {
                // �̹� ��ϵ� ������ ���
                const splitedFileName = src.split('/');
                const savedFileName = splitedFileName[splitedFileName.length - 1];
                replacement = `![${savedFileName}]`;

                convertedHtmlString = convertedHtmlString.replace(
                    `<img src="${src}" alt="${fileName}">`,
                    replacement
                )

                continue;
            }

            convertedHtmlString = convertedHtmlString.replace(
                `<img src="${src}" alt="${fileName}/${editorDivision}">`,
                replacement
            );
        }

        return convertedHtmlString;
    }

    static LoadedFileAndImageReplace = (content: string, idBoardPost: number, fileList: FileDbInfo[], isEditCall: boolean = false): string =>
    {
        const regex = /\!\[([^\]]+)\]/g;
        const match = content.match(regex);

        if (!match)
        {
            return content;
        }

        for (const item of match)
        {
            const fileName = item.substring(2).slice(0, -1);
            const full = `![${fileName}]`;

            const [year, month, day] = GlobalStatic.getDateArray();

            if (fileName.includes("file"))
            {
                const fileNameGUID = fileName.split(":")[1];
                const findFile = fileList.find((file) => file.Name === fileNameGUID);

                if (isEditCall)
                {
                    const replaceFileName = `![file:${findFile?.Name}]`;
                    content = content.replace(full, replaceFileName);
                    break;
                }

                const fileTag = `
                    <div class="content-in-file-wrapper">
                        <a class="content-in-file" data-unset="true" href="/wwwroot/production/UploadFile/${year}/${month}/${day}/${idBoardPost}/${fileNameGUID}" download>
                            ${findFile?.NameOri}
                        </a>
                    </div>
                `

                content = content.replace(full, fileTag);
            }
            else
            {
                // �̹����� ���
                const findFile = fileList.find((file) => file.Name === fileName || file.NameOri === fileName);

                const imageTag = `
                    <img src="/wwwroot/production/UploadFile/${findFile.Url}" alt="${findFile?.idFileInfo}" />
                `

                content = content.replace(full, imageTag);
            }
        }

        return content;
    }

    /**
     * ���� ��¥�� �迭�� ��ȯ�ϴ� �Լ�
     */
    static getDateArray(): string[]
    {
        const date = new Date();
        const year = (date.getFullYear()).toString();
        let month = (date.getMonth() + 1).toString();
        let day = (date.getDate()).toString();

        if (Number(month) < 10)
        {
            month = `0${month}`;
        }

        if (Number(day) < 10)
        {
            day = `0${day}`;
        }

        return [year, month, day];
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
            sTitleTemp = "����";
        }

        GlobalStatic.DG_MessageBox.Show({
            Title: sTitleTemp,
            Content: sMsg,

            top: "center",

            ButtonShowType: ButtonShowType.Ok,

            Buttons: [{
                ButtonCss: "BtnLlightGreen",
                ButtonType: ButtonType.Ok,
                ButtonText: "Ȯ��",
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
                ButtonText: "Ȯ��",
            }, {
                ButtonCss: ['BtnGray'],
                ButtonType: ButtonType.Cancel,
                ButtonText: "���",
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
            sTitleTemp = "����";
        }

        GlobalStatic.DG_MessageBox.Show({
            Title: sTitleTemp,
            Content: sMsg,

            top: "center",

            ButtonShowType: ButtonShowType.Ok,
            Buttons: [{
                ButtonCss: ['BtnRed'],
                ButtonType: ButtonType.Ok,
                ButtonText: "Ȯ��",
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
     * html ���ڿ��� ���ڷ� �޾Ƽ�
     * DOM���� �����ؼ� Return ���ִ� �Լ��̴�.
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