import "./Page.css";

import PageComponent from "../Faculty/Base/PageComponent";
import Footer from "./PageLayout/Footer/Footer";
import Header from "./PageLayout/Header/Header";

export default class Page extends PageComponent
{
    /** Page Component html 파일 주소  */
    private readonly PagePath: string = "Pages/Page.html";

    constructor()
    {
        /** 베이스가 되는 부모 Class인 PageComponent 상속 */
        super([
            { position: "divHeader", component: new Header() },
            { position: "divFooter", component: new Footer() },
        ]);

        /** this.PagePath를 통해서 렌더링 시작 */
        super.RenderingStart(
            this.PagePath
        );

    }

    /**
     * Dom이 생성되고 나서 실행되는 함수
     * @returns {void}
     */
    public RenderingComplete(): void
    {

        console.log('Page Rendering Complete');
    }
}