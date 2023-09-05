import "./css/main.css";
import "@/Utility/DG_MessageBox2/DG_MessageBox2.css";

import NavigoProvider from "./Faculty/Router/Providers/Navigo/NavigoProvider";
import GlobalStatic from "./Global/GlobalStatic";
import Home from "./Pages/Home/Home";
import NotFound from "./Pages/NotFound/NotFound";
import Page from "./Pages/Page";
import AxeView from "./Utility/AxeView/AxeView";
import Board from "./Pages/Board/Board";
import Detail from "./Pages/Detail/Detail";
import Write from "./Pages/Write/Write";

export default class App
{
    public DomThis: Element;
    public Router: NavigoProvider;

    public AxeView: AxeView;

    constructor()
    {
        this.DomThis = document.getElementById('_app') as Element;
        GlobalStatic.app = this;

        this.AxeView = new AxeView();
        this.Router = new NavigoProvider(Page);

        this.ConfigureRoutes();
    }

    // #region 라우팅 관련

    /**
     * 라우터를 설정하는 함수
     * @returns {void}
     */
    private ConfigureRoutes(): void
    {
        this.Router.on(
            "/",
            this.Router.ContentRender({
                Page,
                Component: Home,
            })
        )
            .on(
                "/board/:idBoard",
                this.Router.ContentRender({
                    Page,
                    Component: Board,
                })
            )
            .on(
                "/board/:idBoard/write",
                this.Router.ContentRender({
                    Page,
                    Component: Write
                })
            )
            .on(
                "/board/:idBoard/:idBoardPost",
                this.Router.ContentRender({
                    Page,
                    Component: Detail
                })
            )
            .on(
                "/404",
                this.Router.ContentRender({
                    Page,
                    Component: NotFound,
                })
            )
            .notFound(
                this.Router.ContentRender({
                    Page,
                    Component: NotFound
                })
            )

        this.Router.resolve();

        this.AnchorNavigate();
    }

    /**
     * HTML Anchor 태그를 사용할 때 기존 이벤트를 제거하고
     * Router의 Navigate 함수를 호출하는 함수
     */
    private AnchorNavigate(): void
    {
        this.DomThis.addEventListener("click", (e: MouseEvent) =>
        {
            const target = e.target as HTMLElement;
            const targetParent = target.parentElement as HTMLElement;

            if (!target || !targetParent)
            {
                return;
            }

            const href =
                target.getAttribute("href") ||
                targetParent.getAttribute("href");
            const unset =
                target.getAttribute("data-unset") ||
                targetParent.getAttribute("data-unset");
            const CurrentUrl = this.Router.getCurrentLocation().url;
            const PaginateButton = target.classList.contains("paginate_button");

            // 만약 data unset이 true라면 원래 a 태그의 기능을 사용한다.
            if (unset === "true" || PaginateButton)
            {
                return;
            }

            if (target.tagName === "A" || targetParent.tagName === "A")
            {
                // 현재 페이지와 같은 페이지라면 이동하지 않는다.
                if (href === CurrentUrl)
                {
                    return;
                }

                e.preventDefault();

                if (href)
                {
                    GlobalStatic.PageNowUrl = href;
                    this.Router.navigate(href);
                }
            }
        })
    }

    // #endregion

    // #region UI 관련

    // #endregion

}

/** App 인스턴스 생성 */
const app = new App();
