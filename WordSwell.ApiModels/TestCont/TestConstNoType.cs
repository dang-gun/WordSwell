
using WordSwell.Tool.ApiModelsGlobal.Attributes;

namespace WordSwell.ApiModels.TestCont;

/// <summary>
/// 테스트용 타입 - 타입스크립트=역참조 가능
/// </summary>
[EnumType(TypeScript_EnumNoConstIs = true)]
[Flags]
public enum TestConstNoType
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
