import ContentComponent from "@/Faculty/Base/ContentComponent";
import "./Board.css";
import GlobalStatic from "@/Global/GlobalStatic";
import dayjs from "dayjs";
import Pagination from "@/Global/Pagination";
import { OverwatchingOutputType, OverwatchingType } from "@/Utility/AxeView/OverwatchingType";
import { PostListResultModel } from "@/Faculty/Backend/BoardCont/PostListResultModel";
import { BoardSearchTargetType } from "@/Faculty/Backend/BoardCont/BoardSearchTargetType";
import { FetchPostList } from "@/Faculty/Api/Board";

interface InputValue
{
    SearchTargetType: number;
    Search: string;
}

/**
 * Home Component를 생성하는 Class
 * Index가 되는 페이지이다.
 */
export default class Board extends ContentComponent
{
    /** Home Component의 html 파일 주소 */
    private readonly PagePath: string =
        "Pages/Board/Board.html";

    private readonly idBoard: number = Number(GlobalStatic.app.Router.getParams("idBoard"));
    private readonly PageNumber: number = Number(GlobalStatic.app.Router.getParams("page")) || 1;

    private readonly SearchTargetType: number = Number(GlobalStatic.app.Router.getParams("t")) || BoardSearchTargetType.None;
    private readonly Search: string = GlobalStatic.app.Router.getParams("q") || "";

    private InputValue: InputValue = {
        SearchTargetType: BoardSearchTargetType.Title,
        Search: ""
    };

    private PostList: PostListResultModel;

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

        this.UseOverwatchMonitoringString("searchQuery", "temp");
        this.UseOverwatchMonitoringString("isSearchEmptyList", "d-none");
        this.UseOverwatchMonitoringString("isBoardEmptyList", "d-none");

        /** 검색 인풋 감시 이벤트 */
        this.UseOverwatchAll({
            Name: "onChangeSearchInput",
            FirstData: this.onChangeSearchInput,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 검색 타겟 셀렉트 박스 감시 이벤트 */
        this.UseOverwatchAll({
            Name: "onChangeSearchTarget",
            FirstData: this.onChangeSearchTarget,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

        /** 검색 버튼 클릭 이벤트 */
        this.UseOverwatchAll({
            Name: "onClickSearchButton",
            FirstData: this.onClickSearchButton,
            OverwatchingOutputType:
                OverwatchingOutputType.Function_NameRemoveOn,
            OverwatchingType: OverwatchingType.Monitoring,
            OverwatchingOneIs: true
        });

    }

    private onChangeSearchInput = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;
        this.InputValue.Search = Input.value;

    };

    private onChangeSearchTarget = (
        event: Event
    ): void =>
    {
        const Input = event.target as HTMLTextAreaElement;
        this.InputValue.SearchTargetType = Number(Input.value);
    };

    private onClickSearchButton = (
        event: Event
    ): void =>
    {
        event.preventDefault();

        if (this.InputValue.Search.length <= 0)
        {
            return;
        }

        GlobalStatic.app.Router.navigate(
            `/board/${this.idBoard}?t=${this.InputValue.SearchTargetType}&q=${this.InputValue.Search}`
        );

    };

    private async GetBoardPostList(page?: number): Promise<PostListResultModel>
    {
        try
        {
            const response = await FetchPostList({
                PageNumber: page || 1,
                ShowCount: 10,
                idBoard: this.idBoard,
                SearchTargetType: this.SearchTargetType,
                Search: this.Search
            });

            if (response.InfoCode === "0")
            {
                this.PostList = response;

                return response;
            }
        }
        catch (e)
        {
            console.error(e);
        }
    }

    private SetBoardName(): void
    {
        const boardName = this.AxeSelectorByName("boardName");
        const CurrentUrl = GlobalStatic.app.Router.getMatch().url;
        const HeaderNavElement = document.querySelector("ul.navbar-nav") as HTMLUListElement;
        const LinkElements = HeaderNavElement.querySelectorAll("li.nav-item a.nav-link") as NodeListOf<HTMLAnchorElement>;

        LinkElements.forEach((item) =>
        {
            const href = item.getAttribute("href");
            if (href === `/${CurrentUrl}`)
            {
                boardName.data = item.textContent;
            }
        })
    }

    private async SetBoardList(page?: number): Promise<void>
    {
        const PostList = await this.GetBoardPostList(page || 1);

        if (!PostList || !PostList.PostList || PostList.PostList.length <= 0)
        {
            if (this.Search && this.SearchTargetType)
            {
                // 검색 결과가 없을 경우
                const searchQuery = this.AxeSelectorByName("searchQuery");
                searchQuery.data = this.Search;

                const isSearchEmptyList = this.AxeSelectorByName("isSearchEmptyList");
                isSearchEmptyList.data = "temp";
            }
            else
            {
                // 게시글이 없을 경우
                const isBoardEmptyList = this.AxeSelectorByName("isBoardEmptyList");
                isBoardEmptyList.data = "temp";
            }

            return;
        }

        if (this.SearchTargetType !== BoardSearchTargetType.None)
        {
            const searchTargetSelectBox = this.DomThis.querySelector(".search-target-select") as HTMLSelectElement;
            searchTargetSelectBox.value = this.SearchTargetType.toString();
        }

        const TableElement = this.DomThis.querySelector("#boardTable") as HTMLTableElement;
        const TbodyElement = TableElement.querySelector("tbody") as HTMLTableSectionElement;

        TbodyElement.innerHTML = "";

        PostList.PostList.forEach((item) =>
        {
            const trElement = document.createElement("tr");

            // 번호 ID
            const idTdElement = document.createElement("td");
            const idSpanElement = document.createElement("span");
            idSpanElement.textContent = item.idBoardPost.toString();
            idTdElement.appendChild(idSpanElement);

            // 타이틀
            const titleTdElement = document.createElement("td");
            const titleWrapperElement = document.createElement("div");
            titleWrapperElement.classList.add("board-data-title");

            const titleLinkElement = document.createElement("a");

            const QueryString = GlobalStatic.app.Router.getMatch().queryString;
            if (QueryString)
            {
                titleLinkElement.setAttribute("href", `/board/${this.idBoard}/${item.idBoardPost}?${QueryString}`);
            }
            else
            {
                titleLinkElement.setAttribute("href", `/board/${this.idBoard}/${item.idBoardPost}`);
            }
            titleLinkElement.textContent = item.Title;

            titleWrapperElement.appendChild(titleLinkElement);
            titleTdElement.appendChild(titleWrapperElement);

            // 작성자
            const writerTdElement = document.createElement("td");
            const writerWrapperElement = document.createElement("div");
            writerWrapperElement.classList.add("board-data-writer");

            const writerSpanElement = document.createElement("span");
            writerSpanElement.classList.add("board-data-user");
            writerSpanElement.textContent = item.UserName;

            writerWrapperElement.appendChild(writerSpanElement);
            writerTdElement.appendChild(writerWrapperElement);

            // 작성일
            const dateTdElement = document.createElement("td");
            const dateSpanElement = document.createElement("span");
            dateSpanElement.textContent = dayjs(item.WriteTime).format("YYYY-MM-DD");
            dateTdElement.appendChild(dateSpanElement);

            trElement.appendChild(idTdElement);
            trElement.appendChild(titleTdElement);
            trElement.appendChild(writerTdElement);
            trElement.appendChild(dateTdElement);

            TbodyElement.appendChild(trElement);
        });

        this.SetPagination();
    }

    private SetPagination(): void
    {
        const visibleCount = 5;
        const pagination = new Pagination({
            Dom: this.DomThis.querySelector(".pagination_wrapper"),
            TotalItems: this.PostList.TotalCount,
            ItemsPerPage: this.PostList.ShowCount,
            CurrentPage: this.PageNumber,
            VisiblePages: visibleCount
        });

    }

    /**
     * Dom이 생성되고 나서 실행되는 함수
     * @returns {void}
     */
    public async RenderingComplete(): Promise<void>
    {
        this.SetBoardName();
        await this.SetBoardList(this.PageNumber);

    }
}
