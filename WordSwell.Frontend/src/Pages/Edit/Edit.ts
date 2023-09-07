import ContentComponent from "@/Faculty/Base/ContentComponent";
import "./Edit.scss";
import "@/Styles/CustomEditorStyle.scss";
import {
    CreateBoardPostParams,
    FetchBoardItem,
    FetchBoardPostDetail,
    FetchBoardPostList,
    FetchCreateBoardPost, FetchUpdateBoardPost, PatchUpdateBoardPostParams
} from "@/Faculty/Api/Board";
import GlobalStatic from "@/Global/GlobalStatic";
import {BoardItemResultModel} from "@/Faculty/Models/Board/BoardItemResultModel";
import {BoardPostDetailResultModel} from "@/Faculty/Models/Board/BoardPostDetailResultModel";
import {OverwatchingOutputType, OverwatchingType} from "@/Utility/AxeView/OverwatchingType";
import Editor from "@/Faculty/CustomEditor/Editor/Editor";
import {GetBoardViewType} from "@/Faculty/Models/Board/BoardViewType";
import {FileItemInterface} from "@/Utility/FileSelector/FileItemInterface";
import {BoardStatus} from "@/Faculty/Models/Board/BoardStatusType";

interface InputValue
{
    update: {
        AuthPassword: string;
    };
    Title: string;
    DisplayName: string;
    NewPassword: string;
}

/**
 * Home Component를 생성하는 Class
 * Index가 되는 페이지이다.
 */
export default class Edit extends ContentComponent
{
    /** Home Component의 html 파일 주소 */
    private readonly PagePath: string =
        "Pages/Edit/Edit.html";

    private readonly idBoard: number = Number(GlobalStatic.app.Router.getParams("idBoard"));
    private readonly idCategory: number = Number(GlobalStatic.app.Router.getParams("c")) || 0;
    private readonly idBoardPost: number = Number(GlobalStatic.app.Router.getParams("idBoardPost"));

    private BoardPostDetail: BoardPostDetailResultModel = null;

    private Editor: Editor = new Editor();

    private InputValue: InputValue = {
        update: {
            AuthPassword: ""
        },
        Title: "",
        DisplayName: "",
        NewPassword: ""
    };

    constructor()
    {
        /** 베이스가 되는 부모 Class인 ContentComponent 상속 */
        super();

        this.AddOverwatchState();

        /** this.PagePath를 통해서 렌더링 시작 */
        super.RenderingStart(this.PagePath);
    }

    private AddOverwatchState(): void
    {
        this.UseOverwatchMonitoringString("boardName", "");
        this.UseOverwatchMonitoringString("idBoard", this.idBoard.toString());
        this.UseOverwatchMonitoringString("idCategory", this.idCategory.toString());
        this.UseOverwatchMonitoringString("idBoardPost", this.idBoardPost.toString());

        this.UseOverwatchMonitoringString("onHideBoardPostUpdateForm", "temp");
        this.UseOverwatchMonitoringString("onHideWriteEditor", "d-none");

        /** 목록으로 버튼 클릭 이벤트 */
        this.UseOverwatchAll({
            Name: "onClickBackButton",
            FirstData: this.onClickBackButton,
            OverwatchingOutputType:
            OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 게시글 수정 버튼 클릭 이벤트 */
        this.UseOverwatchAll({
            Name: "onClickUpdateButton",
            FirstData: this.onClickUpdateButton,
            OverwatchingOutputType:
            OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        // 게시글 정보들
        this.UseOverwatchMonitoringString("loadDisplayName", "");
        this.UseOverwatchMonitoringString("loadTitle", "");

        /** 게시글 제목 인풋 감시 이벤트 */
        this.UseOverwatchAll({
            Name: "onChangeTitleInput",
            FirstData: this.onChangeTitleInput,
            OverwatchingOutputType:
            OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 닉네임 인풋 감시 이벤트 */
        this.UseOverwatchAll({
            Name: "onChangeDisplayNameInput",
            FirstData: this.onChangeDisplayNameInput,
            OverwatchingOutputType:
            OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 패스워드 인풋 감시 이벤트 */
        this.UseOverwatchAll({
            Name: "onChangeUpdateAuthPasswordInput",
            FirstData: this.onChangeUpdateAuthPasswordInput,
            OverwatchingOutputType:
            OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 변경할 패스워드 인풋 감시 이벤트 */
        this.UseOverwatchAll({
            Name: "onChangeNewPasswordInput",
            FirstData: this.onChangeNewPasswordInput,
            OverwatchingOutputType:
            OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });
    }

    private onClickUpdateButton = async (): Promise<void> =>
    {
        await this.GetBoardPostDetail();
        await this.SetBoardPostDetail();
    };

    private onClickBackButton = async (): Promise<void> =>
    {
        // 뒤로가기
        window.history.back();
    };

    private onChangeTitleInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;
        this.InputValue.Title = Input.value;
    };

    private onChangeDisplayNameInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;
        this.InputValue.DisplayName = Input.value;
    };

    private onChangeNewPasswordInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;
        this.InputValue.NewPassword = Input.value;
    };

    private onChangeUpdateAuthPasswordInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;
        this.InputValue.update.AuthPassword = Input.value;
    };

    private async GetBoardPostDetail(): Promise<void>
    {
        try
        {
            const response = await FetchBoardPostDetail({
                idBoardPost: this.idBoardPost,
                password: this.InputValue.update.AuthPassword,
                viewType: GetBoardViewType.Edit
            });

            if (response.InfoCode === "0")
            {
                const onHideBoardPostUpdateForm = this.AxeSelectorByName("onHideBoardPostUpdateForm");
                const onHideWriteEditor = this.AxeSelectorByName("onHideWriteEditor");

                onHideBoardPostUpdateForm.data = "d-none";
                onHideWriteEditor.data = "temp";

                this.BoardPostDetail = response;
            }
            else
            {
                GlobalStatic.MessageBox_Error({
                    sTitle: "오류",
                    sMsg: response.Message
                });
                console.log(response);

                return null;
            }
        }
        catch (e)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "게시글 정보를 불러오는데 실패하였습니다."
            });

            console.error(e);
        }
    }

    private async GetBoardItem(): Promise<BoardItemResultModel>
    {
        try
        {
            const response = await FetchBoardItem(this.idBoard);

            if (response.InfoCode === "0")
            {
                const boardName = this.AxeSelectorByName("boardName");
                boardName.data = response.Payload.Title;

                return response;
            }
            else
            {
                GlobalStatic.MessageBox_Error({
                    sTitle: "오류",
                    sMsg: response.Message
                });
                console.log(response);

                return null;
            }
        }
        catch (e)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "게시판 정보를 불러오는데 실패하였습니다."
            });

            console.error(e);
        }
    }

    private async SetCategoryList(): Promise<void>
    {
        const BoardItem = await this.GetBoardItem();
        const CategoryList = BoardItem.Payload?.Category;

        if (!CategoryList || CategoryList.length === 0)
        {
            return;
        }

        const CategoryListElement = document.querySelector("ul.sidebar-menu") as HTMLUListElement;

        CategoryListElement.innerHTML = `
            <li>
                <a ${this.idCategory === 0 ? "class=\"active\"" : ""} data-category-id="0" href="/board/${this.idBoard}">전체</a>
            </li>
        `;

        CategoryList.forEach((item) =>
        {
            const liElement = document.createElement("li");

            const aElement = document.createElement("a");

            if (this.idCategory === item.idCategory)
            {
                aElement.classList.add("active");
            }

            aElement.setAttribute("data-category-id", item.idCategory.toString());
            aElement.setAttribute("href", `/board/${this.idBoard}?c=${item.idCategory}`);
            aElement.textContent = item.Name;

            liElement.appendChild(aElement);

            CategoryListElement.appendChild(liElement);
        });
    }

    private async SetBoardPostDetail(): Promise<void>
    {
        const BoardPostDetail = this.BoardPostDetail;

        if (!BoardPostDetail)
        {
            return;
        }

        const LoadDisplayName = this.AxeSelectorByName("loadDisplayName");
        LoadDisplayName.data = BoardPostDetail.Payload.BoardPost.DisplayName;
        this.InputValue.DisplayName = BoardPostDetail.Payload.BoardPost.DisplayName;

        const LoadTitle = this.AxeSelectorByName("loadTitle");
        LoadTitle.data = BoardPostDetail.Payload.BoardPost.Title;
        this.InputValue.Title = BoardPostDetail.Payload.BoardPost.Title;

        this.Editor.SetData(BoardPostDetail.Payload.BoardPostContent.Content);

        const BoardFiles: FileItemInterface[] = [];

        BoardPostDetail.Payload.BoardPost.BoardFile.forEach((item) =>
        {
            BoardFiles.push({
                Name: item.Name,
                Extension: item.Extension,
                Size: item.Size,
                Type: item.Type,
                Url: item.Url,
                EditorDivision: "",
                idLocal: item.idLocalFile,
                idFile: item.idBoardFile,
                Description: "",
                Binary: "",
                BinaryIs: false,
                BinaryReadyIs: false,
                Delete: item.Status === BoardStatus.Deleted,
                Edit: item.UpdatedAt !== null
            });
        });

        this.Editor.FileAdd(BoardFiles);
    }

    private async UpdateBoardPost({
        idBoardPost,
        Password,
        NewPassword,
        Title,
        DisplayName,
        Content,
        Files
    }: PatchUpdateBoardPostParams): Promise<void>
    {
        try
        {
            const response = await FetchUpdateBoardPost({
                idBoardPost,
                Password,
                NewPassword,
                Title,
                DisplayName,
                Content,
                Files
            });

            if (response.InfoCode === "0")
            {
                GlobalStatic.app.Router.navigate(
                    `/board/${this.idBoard}/${this.idBoardPost}?c=${this.idCategory}`
                );
            }
            else
            {
                GlobalStatic.MessageBox_Error({
                    sTitle: "오류",
                    sMsg: response.Message
                });
                console.log(response);
            }
        }
        catch (e)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "게시글 정보를 수정하는데 실패하였습니다."
            });

            console.error(e);
        }
    }

    /**
     * Dom이 생성되고 나서 실행되는 함수
     * @returns {void}
     */
    public async RenderingComplete(): Promise<void>
    {
        const EditorContainer = this.DomThis.querySelector(".editor-container") as HTMLElement;
        this.Editor.CreateEditor(EditorContainer, async (data) =>
        {
            await this.UpdateBoardPost({
                idBoardPost: this.idBoardPost,
                Password: this.InputValue.update.AuthPassword,
                NewPassword: this.InputValue.NewPassword,
                Title: this.InputValue.Title,
                DisplayName: this.InputValue.DisplayName,
                Content: data.Content,
                Files: data.Files

            });

        });

        await this.SetCategoryList();
    }
}
