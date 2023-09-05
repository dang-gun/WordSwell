import ContentComponent from "@/Faculty/Base/ContentComponent";
import "./Write.css";
import GlobalStatic from "@/Global/GlobalStatic";
import dayjs from "dayjs";
import { OverwatchingOutputType, OverwatchingType } from "@/Utility/AxeView/OverwatchingType";
import Editor from "@/Faculty/CustomEditor/Editor/Editor";
import { PostEditViewResultModel } from "@/Faculty/Backend/BoardCont/PostEditViewResultModel";
import { FetchPostEditView, FetchPostWrite } from "@/Faculty/Api/Board";
import { PostWriteCallModel } from "@/Faculty/Backend/BoardCont/PostWriteCallModel";

import "@ckeditor/ckeditor5-build-classic/build/translations/ko";

interface InputValue
{
    Title: string;
    UserName?: string;
    Password?: string;
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
        Password: ""
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
        this.UseOverwatchMonitoringString("boardTableId", this.idBoard.toString());

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
        this.UseOverwatchMonitoringString("boardPostTitle", "");

        // 게시글 작성자
        this.UseOverwatchMonitoringString("boardPostWriter", "");

        // 게시글 카테고리
        this.UseOverwatchMonitoringString("boardPostCategory", "");

        // 게시글 작성일
        this.UseOverwatchMonitoringString("boardPostDate", "");

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
    }

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

    private onChangeUserNameInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;
        this.InputValue.UserName = Input.value;
    };

    private onChangePasswordInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;
        this.InputValue.Password = Input.value;
    };

    private async GetBoardPostDetail(): Promise<PostEditViewResultModel>
    {
        try
        {
            const response = await FetchPostEditView({
                idBoard: this.idBoard,
                idBoardPost: this.idBoardPost,
                Password: this.InputValue.Password
            });

            if (response.InfoCode === "0")
            {
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
                FileList
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

        const EditorContainer = this.DomThis.querySelector(".editor-container") as HTMLElement;
        this.Editor.CreateEditor(EditorContainer, async (data) =>
        {
            console.log(data);
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
