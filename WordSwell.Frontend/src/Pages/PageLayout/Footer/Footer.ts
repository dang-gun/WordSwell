import ContentComponent from "@/Faculty/Base/ContentComponent";
import "./Footer.css";

/**
 * Footer Component를 생성하는 Class
 */
export default class Footer extends ContentComponent
{
    /** Footer Component의 html 파일 주소 */
    private readonly PagePath: string = "Pages/PageLayout/Footer/Footer.html";

    constructor()
    {
        /** 베이스가 되는 부모 Class인 ContentComponent 상속 */
        super();
    }

    public get GetPagePath(): string
    {
        return this.PagePath;
    }

    /**
     * Dom이 생성되고 나서 실행되는 함수
     * @returns {void}
     */
    public RenderingComplete(): void
    {
        console.log('푸터 렌더링');
    }
}


