interface ApiEndpoint
{
    readonly Base: string;
    readonly Board: {
        readonly BoardList: string;
        readonly PostWrite: string;
        readonly PostList: string;
        readonly PostView: string;
        readonly PostEditView: string;
        readonly PostEditApply: string;
        readonly PostDelete: string;
    };

}

export const Endpoint: ApiEndpoint = {
    // base
    get Base()
    {
        //return 'http://localhost:3065/api';
        return 'http://localhost:7250/api';
    },

    // board
    get Board()
    {
        return {
            BoardList: `${this.Base}/Board/BoardList`,
            PostWrite: `${this.Base}/Board/PostWrite`,
            PostList: `${this.Base}/Board/PostList`,
            PostView: `${this.Base}/Board/PostView`,
            PostEditView: `${this.Base}/Board/PostEditView`,
            PostEditApply: `${this.Base}/Board/PostEditApply`,
            PostDelete: `${this.Base}/Board/PostDelete`,
        }
    },

}