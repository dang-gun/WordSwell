import { BigIconType, ButtonShowType, ButtonType } from "./ButtonEnumType";
import { JsonShowDefaultOption } from "../DG_Popup2/JsonShowDefaultOption";
import { ButtonItem } from "./ButtonItem";

export interface MessageBoxShowDefaultOption extends JsonShowDefaultOption
{
    //제목
    Title?: string,

    //큰 아이콘 타입
    BigIconType?: BigIconType,

    //버튼 타입
    ButtonShowType?: ButtonShowType,

    // 버튼 정보 배열
    Buttons?: ButtonItem[];

    //버튼 이벤트
    //function (DG_MessageBox.ButtonType)
    //DG_MessageBox.ButtonType : 클릭된 버튼 정보
    ButtonEvent?: (ButtonType: ButtonType) => void,

    // BigIcon css
    BigIconCss?: string,
}