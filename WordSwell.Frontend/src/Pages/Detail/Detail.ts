import ContentComponent from "@/Faculty/Base/ContentComponent";
import "./Detail.css";
import GlobalStatic from "@/Global/GlobalStatic";
import dayjs from "dayjs";
import { OverwatchingOutputType, OverwatchingType } from "@/Utility/AxeView/OverwatchingType";
import { Overwatch } from "@/Utility/AxeView/Overwatch";
import { PostViewResultModel } from "@/Faculty/Backend/BoardCont/PostViewResultModel";
import { FetchPostDelete, FetchPostView } from "@/Faculty/Api/Board";

interface InputValue
{
    delete: {
        password: string;
    };
    update: {
        password: string;
    };
}

/**
 * Home Component를 생성하는 Class
 * Index가 되는 페이지이다.
 */
export default class Detail extends ContentComponent
{
    /** Home Component의 html 파일 주소 */
    private readonly PagePath: string =
        "Pages/Detail/Detail.html";

    private readonly idBoard: number = Number(GlobalStatic.app.Router.getParams("idBoard"));
    private readonly idCategory: number = Number(GlobalStatic.app.Router.getParams("c")) || 0;
    private readonly idBoardPost: number = Number(GlobalStatic.app.Router.getParams("idBoardPost"));

    private BoardPostDetail: PostViewResultModel;

    private InputValue: InputValue = {
        delete: {
            password: ""
        },
        update: {
            password: ""
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

        // 게시글 내용
        this.UseOverwatchAll({
            Name: "boardPostContentDom",
            FirstData: "",
            OverwatchingOutputType:
                OverwatchingOutputType.Dom,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        // 파일

        // 파일 유무
        this.UseOverwatchMonitoringString("hasFileBoard", "d-none");

        // 파일 개수
        this.UseOverwatchMonitoringString("fileCount", "0");

        // 토글 파일리스트 element class
        this.UseOverwatchMonitoringString("toggleFileListElement", "d-none");

        /** 첨부파일 토글 버튼 클릭 이벤트 */
        this.UseOverwatchAll({
            Name: "onClickFilesToggle",
            FirstData: this.onClickFilesToggle,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 게시글 삭제 확인 버튼 클릭 이벤트 */
        this.UseOverwatchAll({
            Name: "onClickDeleteButtonConfirm",
            FirstData: this.onClickDeleteButtonConfirm,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 게시글 삭제 비밀번호 인풋 감시 이벤트 */
        this.UseOverwatchAll({
            Name: "onChangeDeletePasswordInput",
            FirstData: this.onChangeDeletePasswordInput,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 게시글 삭제 취소 버튼 클릭 이벤트 */
        this.UseOverwatchAll({
            Name: "onClickCancelDeletePostButton",
            FirstData: this.onClickCancelDeletePostButton,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 게시글 삭제 진행 버튼 클릭 이벤트 */
        this.UseOverwatchAll({
            Name: "onClickDeleteButton",
            FirstData: this.onClickDeleteButton,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        // 포스트 내용을 숨기는 클래스
        this.UseOverwatchMonitoringString("onHideBoardPost", "temp");

        // 삭제 폼을 보여주는 클래스
        this.UseOverwatchMonitoringString("onHideBoardPostDeleteForm", "d-none");
    }

    private onClickBackButton = async (): Promise<void> =>
    {
        // 뒤로가기
        GlobalStatic.app.Router.navigate(`/board/${this.idBoard}`);
    };

    private onClickFilesToggle = (
        event: Event
    ): void =>
    {
        const toggleFileListElement = this.AxeSelectorByName("toggleFileListElement");

        if (toggleFileListElement.data === "d-none")
        {
            toggleFileListElement.data = "temp";
        }
        else
        {
            toggleFileListElement.data = "d-none";
        }

        document.addEventListener("click", (event) =>
        {
            const Target = event.target as HTMLElement;
            const isFileListElement = Target.classList.contains("file-list");
            const isFileListToggleBtn = Target.classList.contains("file-info-button") || Target.classList.contains("file-info-name") || Target.classList.contains("file-info-title");

            if (!isFileListElement && !isFileListToggleBtn)
            {
                const toggleFileListElement = this.AxeSelectorByName("toggleFileListElement");
                toggleFileListElement.data = "d-none";
            }
        });

    };

    private onClickDeleteButtonConfirm = async (): Promise<void> =>
    {
        const HideBoardPost = this.AxeSelectorByName("onHideBoardPost");
        HideBoardPost.data = "d-none";

        const HideBoardPostDeleteForm = this.AxeSelectorByName("onHideBoardPostDeleteForm");
        HideBoardPostDeleteForm.data = "temp";
    };

    private onChangeDeletePasswordInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLInputElement;
        this.InputValue.delete.password = Input.value;
    };

    private onClickCancelDeletePostButton = async (): Promise<void> =>
    {
        const HideBoardPost = this.AxeSelectorByName("onHideBoardPost");
        HideBoardPost.data = "temp";

        const HideBoardPostDeleteForm = this.AxeSelectorByName("onHideBoardPostDeleteForm");
        HideBoardPostDeleteForm.data = "d-none";
    };

    private onClickDeleteButton = async (): Promise<void> =>
    {
        try
        {
            const response = await FetchPostDelete({
                idBoard: this.idBoard,
                idBoardPost: this.idBoardPost,
                Password: this.InputValue.delete.password
            });

            if (response.InfoCode === "0")
            {
                GlobalStatic.app.Router.navigate(`/board/${this.idBoard}`);
            }
            else
            {
                GlobalStatic.MessageBox_Error({
                    sTitle: "오류",
                    sMsg: response.Message
                });
            }
        }
        catch (e)
        {
            GlobalStatic.MessageBox_Error({
                sTitle: "오류",
                sMsg: "게시글 삭제에 실패하였습니다."
            });

            console.error(e);
        }
    };

    private async GetBoardPostDetail(): Promise<PostViewResultModel>
    {
        try
        {
            const response = await FetchPostView({
                idBoard: this.idBoard,
                idBoardPost: this.idBoardPost
            });

            if (response.InfoCode === "0")
            {
                this.BoardPostDetail = response;
                return response;
            }
            else
            {
                GlobalStatic.app.Router.navigate('/404');

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

    private async SetBoardPostDetail(): Promise<void>
    {
        const PostView = await this.GetBoardPostDetail();

        if (!PostView || !PostView.Post || !PostView.PostContents)
        {
            GlobalStatic.app.Router.navigate('/404');
            return;
        }

        const boardPostTitle = this.AxeSelectorByName("boardPostTitle");
        boardPostTitle.data = PostView.Post.Title;

        const boardPostWriter = this.AxeSelectorByName("boardPostWriter");
        boardPostWriter.data = PostView.Post.UserName;

        const boardPostDate = this.AxeSelectorByName("boardPostDate");
        boardPostDate.data = dayjs(PostView.Post.WriteTime).format("YYYY-MM-DD");

        const boardPostContent = this.AxeSelectorByName("boardPostContentDom");
        const boardPostContentDom = boardPostContent.Dom as HTMLDivElement;
        boardPostContentDom.innerHTML = PostView.PostContents.Contents;
    }

    // private SetFileList(): void
    // {
    //     const {Payload: {BoardPost}} = this.BoardPostDetail;

    //     if (BoardPost.BoardFile.length === 0)
    //     {
    //         return;
    //     }

    //     const hasFileBoard = this.AxeSelectorByName("hasFileBoard");
    //     hasFileBoard.data = "temp";

    //     const fileCount = this.AxeSelectorByName("fileCount");
    //     fileCount.data = BoardPost.BoardFile.length;

    //     const fileListElement = this.DomThis.querySelector(".file-list") as HTMLUListElement;

    //     BoardPost.BoardFile.forEach((item) =>
    //     {
    //         const liElement = document.createElement("li");

    //         const aElement = document.createElement("a");
    //         aElement.setAttribute("data-unset", "true");
    //         aElement.setAttribute("target", "_blank");
    //         aElement.setAttribute("href", `http://localhost:3065${item.Url}`);
    //         aElement.setAttribute("download", "");
    //         aElement.textContent = item.Name;

    //         liElement.appendChild(aElement);

    //         fileListElement.appendChild(liElement);
    //     });

    // }

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

        await this.SetBoardPostDetail();
        // this.SetFileList();
        // await this.SetBoardList(this.pageQuery);

    }
}
