import { User } from './../../Faculty/Backend/ModelsDB/User/User';
import ContentComponent from "@/Faculty/Base/ContentComponent";
import "./Write.css";
import GlobalStatic from "@/Global/GlobalStatic";
import dayjs from "dayjs";
import { OverwatchingOutputType, OverwatchingType } from "@/Utility/AxeView/OverwatchingType";
import Editor from "@/Faculty/CustomEditor/Editor/Editor";
import { PostEditViewResultModel } from "@/Faculty/Backend/BoardCont/PostEditViewResultModel";
import { FetchPostEdit, FetchPostEditView, FetchPostWrite } from "@/Faculty/Api/Board";
import { PostWriteCallModel } from "@/Faculty/Backend/BoardCont/PostWriteCallModel";

import "@ckeditor/ckeditor5-build-classic/build/translations/ko";
import { PostEditApplyCallModel } from "@/Faculty/Backend/BoardCont/PostEditApplyCallModel";

interface InputValue
{
    Title: string;
    UserName?: string;
    Password?: string;
    Update: {
        Password: string;
    }
}

/**
 * Home Component를 생성하는 Class
 * Index가 되는 페이지이다.
 */
export default class Write extends ContentComponent
{
    /** Home Component의 html 파일 주소 */
    private readonly PagePath: string =
        "Pages/Write/Write.html";

    private readonly idBoard: number = Number(GlobalStatic.app.Router.getParams("idBoard"));
    private readonly idBoardPost: number = Number(GlobalStatic.app.Router.getParams("idBoardPost"));

    private Editor: Editor = new Editor();

    private InputValue: InputValue = {
        Title: "",
        UserName: "",
        Password: "",
        Update: {
            Password: ""
        }
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

        this.UseOverwatchMonitoringString("idBoardPost", this.idBoardPost.toString());

        this.UseOverwatchMonitoringString("onHidePostEditValidateForm", "d-none");
        this.UseOverwatchMonitoringString("isValidate", "d-none");

        /** 목록으로 버튼 클릭 이벤트 */
        this.UseOverwatchAll({
            Name: "onClickBackButton",
            FirstData: this.onClickBackButton,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        // 게시글 정보들

        // 게시글 제목
        this.UseOverwatchMonitoringString("postEditTitle", "");
        this.UseOverwatchMonitoringString("postEditUserName", "");

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
            Name: "onChangeUserNameInput",
            FirstData: this.onChangeUserNameInput,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 패스워드 인풋 감시 이벤트 */
        this.UseOverwatchAll({
            Name: "onChangePasswordInput",
            FirstData: this.onChangePasswordInput,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 게시글 수정 비밀번호 인풋 감시 이벤트 */
        this.UseOverwatchAll({
            Name: "onChangeUpdatePasswordInput",
            FirstData: this.onChangeUpdatePasswordInput,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 게시글 수정 진행 버튼 클릭 이벤트 */
        this.UseOverwatchAll({
            Name: "onClickUpdateButton",
            FirstData: this.onClickUpdateButton,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });
    }

    private onChangeUpdatePasswordInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLInputElement;
        this.InputValue.Update.Password = Input.value;
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
        // 작성 모드
        this.InputValue.Title = Input.value;
    };

    private onChangeUserNameInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;

        // 작성 모드
        this.InputValue.UserName = Input.value;
    };

    private onChangePasswordInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;
        if (this.idBoardPost)
        {
            // 수정 모드
            this.InputValue.Update.Password = Input.value;
        }
        else
        {
            // 작성 모드
            this.InputValue.Password = Input.value;
        }
    };

    private onClickUpdateButton = async (): Promise<void> =>
    {
        await this.SetPostEditView();

    }

    private async SetPostEditView(): Promise<void>
    {
        const EditViewData = await this.GetPostEditView();

        if (!EditViewData || !EditViewData.Post || !EditViewData.PostContents)
        {
            return;
        }

        const onHidePostEditValidateForm = this.AxeSelectorByName("onHidePostEditValidateForm");
        const isValidate = this.AxeSelectorByName("isValidate");

        onHidePostEditValidateForm.data = "d-none";
        isValidate.data = "temp";

        const postEditTitle = this.AxeSelectorByName("postEditTitle");
        postEditTitle.data = EditViewData.Post.Title;
        this.InputValue.Title = EditViewData.Post.Title;

        const postEditUserName = this.AxeSelectorByName("postEditUserName");
        postEditUserName.data = EditViewData.Post.UserName;
        this.InputValue.UserName = EditViewData.Post.UserName;

        const replaceContent = GlobalStatic.LoadedFileAndImageReplace(EditViewData.PostContents.Contents, EditViewData.Post.idBoardPost, true);

        console.log(replaceContent);

        this.Editor.SetData(replaceContent);

        if (EditViewData.FileList && EditViewData.FileList.length > 0)
        {
            this.Editor.FileAdd(EditViewData.FileList);
        }

    }

    private async GetPostEditView(): Promise<PostEditViewResultModel>
    {
        try
        {
            const response = await FetchPostEditView({
                idBoard: this.idBoard,
                idBoardPost: this.idBoardPost,
                Password: this.InputValue.Update.Password
            });

            if (response.InfoCode === "0")
            {
                console.log(response)
                return response;
            }
            else
            {
                GlobalStatic.MessageBox_Error({
                    sTitle: "오류",
                    sMsg: "게시글 정보를 불러오는데 실패하였습니다."
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

    private async CreatePost({
        idBoard,
        Title,
        Contents,
        UserName,
        Password,
        FileList
    }: PostWriteCallModel): Promise<void>
    {
        if (!Title)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "제목을 입력해주세요."
            });

            return;
        }

        if (!Contents)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "내용을 입력해주세요."
            });

            return;
        }

        if (!UserName)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "닉네임을 입력해주세요."
            });

            return;
        }

        if (!Password)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "비밀번호를 입력해주세요."
            });

            return;
        }

        try
        {
            const response = await FetchPostWrite({
                idBoard,
                Title,
                Contents,
                UserName,
                Password,
                FileList: FileList || null
            });

            if (response.InfoCode === "0")
            {
                console.log(response);
                GlobalStatic.app.Router.navigate(
                    `/board/${this.idBoard}/${response.idBoardPost}`
                );
            }
        }
        catch (e)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "게시글을 작성하는데 실패하였습니다."
            });

            console.error(e);
        }
    }

    private async EditPost({
        idBoard,
        idBoardPost,
        Title,
        Contents,
        Password,
        FileList
    }: PostEditApplyCallModel): Promise<void>
    {
        if (!Title)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "제목을 입력해주세요."
            });

            return;
        }

        if (!Contents)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "내용을 입력해주세요."
            });

            return;
        }

        if (!Password)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "비밀번호를 입력해주세요."
            });

            return;
        }

        try
        {
            const response = await FetchPostEdit({
                idBoard,
                idBoardPost,
                Title,
                Contents,
                Password,
                FileList
            });

            if (response.InfoCode === "0")
            {
                console.log(response);
                GlobalStatic.app.Router.navigate(
                    `/board/${this.idBoard}/${this.idBoardPost}`
                );
            }
        }
        catch (e)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "게시글을 작성하는데 실패하였습니다."
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
        // if (!this.BoardTableId)
        // {
        //     GlobalStatic.app.Router.navigate('/404');
        // }
        const onHidePostEditValidateForm = this.AxeSelectorByName("onHidePostEditValidateForm");
        const isValidate = this.AxeSelectorByName("isValidate");
        const EditorContainer = this.DomThis.querySelector(".editor-container") as HTMLElement;

        if (this.idBoardPost)
        {
            const UserNameInput = this.DomThis.querySelector('.username-input') as HTMLInputElement;
            UserNameInput.disabled = true;
            // 수정 모드
            onHidePostEditValidateForm.data = "temp";
            this.Editor.CreateEditor(EditorContainer, async (data) =>
            {
                await this.EditPost({
                    idBoard: this.idBoard,
                    idBoardPost: this.idBoardPost,
                    Title: this.InputValue.Title.trim(),
                    Contents: data.Content,
                    Password: this.InputValue.Update.Password,
                    FileList: data.Files
                });
            })
        }
        else
        {
            // 작성 모드
            isValidate.data = "temp";
            this.Editor.CreateEditor(EditorContainer, async (data) =>
            {
                await this.CreatePost({
                    idBoard: this.idBoard,
                    Title: this.InputValue.Title.trim(),
                    Contents: data.Content,
                    UserName: this.InputValue.UserName,
                    Password: this.InputValue.Password,
                    FileList: data.Files
                });
            });
        }


    }
}
