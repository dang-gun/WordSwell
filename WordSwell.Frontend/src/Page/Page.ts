import PageComponent from "../Faculty/Base/PageComponent";
import Footer from "./PageLayout/Footer/Footer";
import Header from "./PageLayout/Header/Header";

export default class Page extends PageComponent
{
    /** Page Component html ���� �ּ�  */
    private readonly PagePath: string = "Page/Page.html";

    constructor()
    {
        super([
            { position: "divHeader", component: new Header() },
            { position: "divFooter", component: new Footer() }
        ]);

        /** this.PagePath�� ���ؼ� ������ ���� */
        super.RenderingStart(
            this.PagePath
        );
    }

    /**
     * Dom�� �����ǰ� ���� ����Ǵ� �Լ�
     * @returns {void}
     */
    public RenderingComplete(): void
    {
        console.log('Page Rendering Complete');
    }
}