import GlobalStatic from "./GlobalStatic";

interface PaginationOptions
{
    Dom: HTMLElement;
    TotalItems: number;
    ItemsPerPage: number;
    CurrentPage: number;
    VisiblePages: number;
}

interface PaginationData
{
    CurrentPage: number;
    TotalPages: number;
}

export default class Pagination
{
    private Options: PaginationOptions;

    constructor(options: PaginationOptions)
    {
        this.Options = options;

        this.RenderPagiationDom();
    }

    private RenderPagiationDom(): void
    {
        const TotalPages = this.GetPageData().TotalPages;
        const VisiblePages = this.Options.VisiblePages || 5;

        const PaginationContainer = document.createElement('div');
        PaginationContainer.classList.add('pagination_container');

        this.Options.Dom.innerHTML = '';

        let StartPage: number, EndPage: number;
        if (this.Options.CurrentPage <= VisiblePages)
        {
            // 현재 페이지가 VisiblePages보다 작거나 같다면
            StartPage = 1;
            EndPage = TotalPages;
        }
        else
        {
            StartPage = Math.floor((this.Options.CurrentPage - 1) / VisiblePages) * VisiblePages + 1;
            EndPage = Math.min(StartPage + VisiblePages - 1, TotalPages);
        }

        // 이전 버튼 생성
        const PrevButton = this.CreatePrevButton();
        if (this.Options.CurrentPage > 1)
        {
            PaginationContainer.appendChild(PrevButton);
        }

        // 페이지 버튼 생성
        this.CreatePageButtons(
            TotalPages,
            VisiblePages,
            PaginationContainer,
            StartPage,
            EndPage
        );

        // 다음 버튼 생성
        const NextButton = this.CreateNextButton();
        if (this.Options.CurrentPage < TotalPages)
        {
            PaginationContainer.appendChild(NextButton);
        }

        this.Options.Dom.appendChild(PaginationContainer);
    }

    private CreatePrevButton(): HTMLAnchorElement
    {
        const PrevButton = document.createElement('a');

        PrevButton.classList.add('pagination_button');
        PrevButton.classList.add('pagination_button-prev');
        PrevButton.textContent = '이전';

        const CurrentUrl = GlobalStatic.app.Router.getMatch().url;
        const CurrentQueryString = GlobalStatic.app.Router.getMatch().queryString
        const NonPageQueryStrings = this.findNonPageQueryStrings(CurrentQueryString);
        const HasQueryString = NonPageQueryStrings.length > 0;

        if (this.Options.CurrentPage > 1)
        {
            PrevButton.setAttribute('href', `${CurrentUrl}${HasQueryString ? '?' : ''}${HasQueryString ? NonPageQueryStrings.join('&') + '&' : '?'}page=${this.Options.CurrentPage - 1}`);
        }

        return PrevButton;
    }

    private CreatePageButtons(
        TotalPages: number,
        VisiblePages: number,
        Container: HTMLElement,
        StartPage: number = 1,
        EndPage: number = TotalPages
    ): void
    {
        // 페이지 버튼 생성
        for (let i = StartPage; i <= EndPage; i++)
        {
            const PageButton = document.createElement('a');
            PageButton.classList.add('pagination_button');
            PageButton.textContent = i.toString();

            const CurrentUrl = GlobalStatic.app.Router.getMatch().url;
            const CurrentQueryString = GlobalStatic.app.Router.getMatch().queryString
            const NonPageQueryStrings = this.findNonPageQueryStrings(CurrentQueryString);
            const HasQueryString = NonPageQueryStrings.length > 0;

            if (i !== this.Options.CurrentPage)
            {
                PageButton.setAttribute('href', `${CurrentUrl}${HasQueryString ? '?' : ''}${HasQueryString ? NonPageQueryStrings.join('&') + '&' : '?'}page=${i}`);
            }
            else
            {
                PageButton.classList.add('pagination_button-active');
                PageButton.setAttribute('onclick', 'return false;');
            }

            Container.appendChild(PageButton);

            if (i === VisiblePages)
            {
                break;
            }
        }
    }

    private CreateNextButton(): HTMLAnchorElement
    {
        const NextButton = document.createElement('a');

        NextButton.classList.add('pagination_button');
        NextButton.classList.add('pagination_button-next');
        NextButton.textContent = '다음';

        const CurrentUrl = GlobalStatic.app.Router.getMatch().url;
        const CurrentQueryString = GlobalStatic.app.Router.getMatch().queryString
        const NonPageQueryStrings = this.findNonPageQueryStrings(CurrentQueryString);
        const HasQueryString = NonPageQueryStrings.length > 0;

        if (this.Options.CurrentPage < this.GetPageData().TotalPages)
        {
            NextButton.setAttribute('href', `${CurrentUrl}${HasQueryString ? '?' : ''}${HasQueryString ? NonPageQueryStrings.join('&') + '&' : '?'}page=${this.Options.CurrentPage + 1}`);
        }

        return NextButton;
    }

    private GetPageData(): PaginationData
    {
        const totalPages = Math.ceil(this.Options.TotalItems / this.Options.ItemsPerPage);

        return {
            CurrentPage: this.Options.CurrentPage,
            TotalPages: totalPages,
        }
    }

    private findNonPageQueryStrings(queryString: string): string[]
    {
        const pattern = /(?:^|&)(?!page=\d)([^&]+)/g;
        const matches = [];
        let match: RegExpExecArray | null;

        while ((match = pattern.exec(queryString)))
        {
            matches.push(decodeURIComponent(match[1]));
        }

        return matches;
    }
}