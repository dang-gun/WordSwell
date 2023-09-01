/** 선택된 버튼의 타입 */
export enum ButtonShowType
{
    /** 없음 */
    None = 0,
    /** OK 버튼 */
    Ok = 1,
    /** OK 버튼과 Cancel 버튼 */
    OkCancel = 2,
    /** Cancel 버튼 */
    Cancel = 3,
    /** Yes 버튼과 No 버튼 */
    YesNo = 4
}

/** 각 버튼별 타입 */
export enum ButtonType
{
    /** 없음 버튼 */
    None = 0,
    /** OK 타입 버튼 */
    Ok = 1,
    /** Cancel 타입 버튼 */
    Cancel = 2,
    /** Yes 타입 버튼 */
    Yes = 3,
    /** No 타입 버튼 */
    No = 4
}

/** 빅아이콘 타입 */
export enum BigIconType
{
    /** 없음 */
    None = 0,
    /** 인포 */
    Info = 1,
    /** 워닝(경고) */
    Warning = 2,
    /** 에러(오류) */
    Error = 3,
    /** 질문 */
    Question = 4,
    /** 성공 */
    Success = 5,
    /** 도움말 */
    Help = 6,
    /** 숨기기 */
    Invisible = 7
}
