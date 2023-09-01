import "./Header.scss";
import ContentComponent from "@/Faculty/Base/ContentComponent";

/**
 * Header Component를 생성하는 Class
 */
export default class Header extends ContentComponent
{
    /** Header Component의 html 파일 주소 */
    private readonly PagePath: string = "Page/PageLayout/Header/Header.html";

    constructor()
    {
        /** 베이스가 되는 부모 Class인 ContentComponent 상속 */
        super();
    }

    /**
     * Dom이 생성되고 나서 실행되는 함수
     * @returns {void}
     */
    public async RenderingComplete(): Promise<void>
    {

    }
}
