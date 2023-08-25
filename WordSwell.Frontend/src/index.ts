import "./css/main.css";



export default class App
{

    constructor()
    {
        let txtMessage: HTMLInputElement = document.querySelector("#txtMessage");
        let divLog: HTMLDivElement = document.querySelector("#divLog");

        (document.querySelector("#btnSend") as HTMLButtonElement)
            .onclick = (event) =>
            {
                let divItem: HTMLElement = document.createElement("div");
                divItem.innerHTML = `<label>${txtMessage.value}</label>`;
                divLog.appendChild(divItem);
            };
    }

    // #region UI 관련

    // #endregion

}

(window as any).app = new App();
