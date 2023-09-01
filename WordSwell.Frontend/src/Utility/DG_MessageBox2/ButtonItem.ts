import { ButtonType } from "./ButtonEnumType";

export interface ButtonItem
{
    // 버튼 css
    ButtonCss: string | string[],
    // 버튼 타입
    ButtonType: ButtonType,
    // 버튼 텍스트
    ButtonText: string;
}