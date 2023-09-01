import NavigoProvider from "./Faculty/Router/Providers/Navigo/NavigoProvider";
import GlobalStatic from "./Global/GlobalStatic";
import Home from "./Page/Home/Home";
import Page from "./Page/Page";
import AxeView from "./Utility/AxeView/AxeView";
import "./css/main.css";

export default class App
{
    public DomThis: Element;
    public Router: NavigoProvider;

    public AxeView: AxeView;

    constructor()
    {
        this.DomThis = document.querySelector('#_app') as Element;
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
        const { on, ContentRender } = this.Router;

        on("/", ContentRender({ Page, Component: Home }))
            .resolve();

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

                event.preventDefault();

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
