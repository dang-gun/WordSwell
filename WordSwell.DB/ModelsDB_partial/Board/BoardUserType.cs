

using DGU_ModelToOutFiles.Global.Attributes;

namespace WordSwell.DB.ModelsDB_partial.Board;

/// <summary>
/// 보드 사용자 타입
/// </summary>
/// <remarks>
/// 개시판 사용자를 나누기위한 대분류.
/// 타입스크립트 - 역참조 해야 한다.
/// </remarks>
[EnumType(TypeScript_EnumNoConstIs = true)]
[Flags]
public enum BoardUserType
{
    /// <summary>
    /// 상태 없음
    /// </summary>
    None = 0,

    /// <summary>
    /// 비회원
    /// </summary>
    NonUser = 1 << 0,

    /// <summary>
    /// 회원
    /// </summary>
    User = 1 << 1,

    /// <summary>
    /// 관리자(개발자 포함)
    /// </summary>
    Admin = 1 << 2,

    /// <summary>
    /// 이 게시판 관리자
    /// </summary>
    BoardAdmin = 1 << 20,
}
