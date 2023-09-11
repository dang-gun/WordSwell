import EditorBase from "@/Faculty/CustomEditor/EditorBase/EditorBase";
import GlobalStatic from "@/Global/GlobalStatic";
import { CustomUploadAdapterPlugin } from "../MyUploadAdapter/MyUploadAdapter";
import DG_jsFileSelector2 from "@/Utility/FileSelector/DG_jsFileSelector2";
import { marked } from "marked";
import { FileItemInterface } from "@/Utility/FileSelector/FileItemInterface";
import { FileDbInfo } from "@/Faculty/Backend/ModelsDB/FileDb/FileDbInfo";

interface EditorWriteData
{
    Content: string;
    Files: FileItemInterface[] | null;
}

export default class Editor
{
    private FileSelector: DG_jsFileSelector2;
    private WriteFn: (data: EditorWriteData) => void;

    constructor() { }

    public CreateEditor(element: HTMLElement, writeFn: (data: EditorWriteData) => void): void
    {
        this.WriteFn = writeFn;

        const EditorWrapper = document.createElement("div");
        EditorWrapper.classList.add("editor-wrapper");

        const EditorBox = document.createElement("div");
        EditorBox.classList.add("app-card");
        EditorBox.classList.add("editor-box");

        const EditorElement = document.createElement("div");
        EditorElement.classList.add("ckeditor-feature");

        const ActionsBox = this.CreateActionsBoxElement();
        // const PreviewElement = this.CreatePreviewElement();

        EditorBase.create(EditorElement, {
            extraPlugins: [CustomUploadAdapterPlugin]
        }).then((editor) =>
        {
            GlobalStatic.Editor = editor;

            // File Selector 등록
            this.FileSelector = new DG_jsFileSelector2({
                Editor: editor,
                Area: document.querySelector(".file-dnd-box"),
                Area_ItemList: document.querySelector(".file-dnd-box"),
                Debug: true,
                MaxFileCount: 2
            });

            GlobalStatic.FileSelector = this.FileSelector;

            editor.model.document.on("change:data", (event) =>
            {
                const content = editor.getData();
                // this.UpdatePreview();
            });

        });

        EditorBox.appendChild(EditorElement);
        EditorBox.appendChild(ActionsBox);

        EditorWrapper.appendChild(EditorBox);
        // EditorWrapper.appendChild(PreviewElement);

        element.appendChild(EditorWrapper);
    }

    private CreatePreviewElement(): HTMLElement
    {
        const PreviewWrapper = document.createElement("div");
        PreviewWrapper.classList.add("preview-box");

        const PreviewTitle = document.createElement("h1");
        PreviewTitle.classList.add("preview-title");
        PreviewTitle.textContent = "미리보기";

        const PreviewIframe = document.createElement("iframe");
        PreviewIframe.classList.add("preview-iframe");

        PreviewWrapper.appendChild(PreviewTitle);
        PreviewWrapper.appendChild(PreviewIframe);

        return PreviewWrapper;
    }

    private CreateActionsBoxElement(): HTMLElement
    {
        const ActionsBox = document.createElement("div");
        ActionsBox.classList.add("actions-box");

        const LeftBox = document.createElement("div");
        LeftBox.classList.add("left-box");

        const RightBox = document.createElement("div");
        RightBox.classList.add("right-box");

        const MarkdownToggleButton = document.createElement("button");
        MarkdownToggleButton.classList.add("btn");
        MarkdownToggleButton.classList.add("btn-dark");
        MarkdownToggleButton.classList.add("markdown-toggle-button");
        MarkdownToggleButton.textContent = "에디터";
        MarkdownToggleButton.addEventListener(
            "click",
            this.MarkdownToggleButtonEvent
        );

        // const PreviewButton = document.createElement('button');
        // PreviewButton.classList.add('btn');
        // PreviewButton.classList.add('preview-button');
        // PreviewButton.textContent = '미리보기';
        // PreviewButton.addEventListener('click', this.PreviewButtonEvent);

        const WriteButton = document.createElement("button");
        WriteButton.classList.add("btn");
        WriteButton.classList.add("btn-primary");
        WriteButton.classList.add("write-button");
        WriteButton.textContent = "글작성";
        WriteButton.addEventListener("click", () => this.WriteFn(this.GetData()));

        LeftBox.appendChild(MarkdownToggleButton);
        // RightBox.appendChild(PreviewButton);
        RightBox.appendChild(WriteButton);

        ActionsBox.appendChild(LeftBox);
        ActionsBox.appendChild(RightBox);

        return ActionsBox;
    }

    private PreviewButtonEvent = (event: MouseEvent): void =>
    {
        const PreviewElement = document.querySelector(
            ".preview-box"
        ) as HTMLElement;
        PreviewElement.classList.toggle("preview-box--show");
        this.UpdatePreview();
    };

    private WriteButtonEvent = (event: MouseEvent): void =>
    {
        const EditorInstance = GlobalStatic.Editor;
        const { EditorMode } = GlobalStatic;
        const data = marked(EditorInstance.getData(), {
            mangle: false,
            headerIds: false
        });

        if (EditorMode === "wysiwyg")
        {
            const NewData = GlobalStatic.ImageToSeparator(data);

            console.log({
                body: NewData ?? data,
                files: this.FileSelector.GetItemList() ?? null
            });
        }
        else
        {
            console.log(data);
        }

    };

    public FileAdd(itemList: FileDbInfo[])
    {
        const FileItemList: FileItemInterface[] = [];

        for (const file of itemList)
        {
            const Extension = file.Name.split(".")[1];
            console.log(file);

            FileItemList.push({
                Name: file.NameOri,
                Extension: `.${Extension}`,
                Length: file.LengthOri,
                Type: file.Type,
                Description: file.Description,
                EditorDivision: "",
                BinaryIs: false,
                BinaryReadyIs: false,
                Binary: "",
                idFileInfo: file.idFileInfo,
                idLocal: 0,
                FileInfoName: file.Name,
                Url: file.Url,
                ErrorIs: false,
                EditIs: false,
                DeleteIs: false
            })
        }

        this.FileSelector.FileAdd_CompleteList(FileItemList);
    }

    public SetData(data: string): void
    {
        const EditorInstance = GlobalStatic.Editor;
        EditorInstance.setData(data);
    }

    public GetData(): EditorWriteData
    {
        const EditorInstance = GlobalStatic.Editor;
        const { EditorMode } = GlobalStatic;
        const data = marked(EditorInstance.getData(), {
            mangle: false,
            headerIds: false
        });

        const NewData = GlobalStatic.ImageToSeparator(data);

        if (EditorMode === "wysiwyg")
        {
            return {
                Content: NewData ?? data,
                Files: this.FileSelector.GetItemList() ?? null
            };
        }
        else
        {
            return {
                Content: NewData ?? data,
                Files: this.FileSelector.GetItemList() ?? null
            };
        }
    }

    private UpdatePreview(): void
    {
        const { EditorMode, Editor: EditorInstance } = GlobalStatic;
        const data = EditorInstance.getData();
        const PreviewIframe = document.querySelector(
            ".preview-iframe"
        ) as HTMLIFrameElement;
        let PreviewContent = GlobalStatic.Editor.getData();

        if (EditorMode === "wysiwyg")
        {
            PreviewContent = marked(data, { mangle: false, headerIds: false });
        }
        else
        {
            PreviewContent = data;
        }

        const PreviewDocument = PreviewIframe.contentWindow.document;
        PreviewDocument.open();
        PreviewDocument.write(PreviewContent);
        PreviewDocument.close();
    }

    private MarkdownToggleButtonEvent(event: MouseEvent): void
    {
        const target = event.target as HTMLButtonElement;

        if (GlobalStatic.EditorMode === "wysiwyg")
        {
            GlobalStatic.EditorMode = "markdown";
            target.textContent = "마크다운";
            console.log(GlobalStatic.EditorMode);
        }
        else
        {
            GlobalStatic.EditorMode = "wysiwyg";
            target.textContent = "에디터";
            console.log(GlobalStatic.EditorMode);
        }
    }
}
