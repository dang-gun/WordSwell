

namespace Utility.ModelToFrontend;

/// <summary>
/// 모델의 멤버 정보를 검색하기 쉽게 저장한다.
/// </summary>
public class TypeScriptModelMember
{
	/// <summary>
	/// 맴버 이름 
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// 네임스페이스를 포함한 전체 이름
	/// </summary>
    public string NameFull { get; set; } = string.Empty;

    /// <summary>
    /// 맴버의 타입
    /// </summary>
    public string Type { get; set; } = string.Empty;
	/// <summary>
	/// 맴버 타입이 배열타입일때 배열이 가지고 있는 타입
	/// </summary>
	/// <remarks>
	/// 1개만, 1댑스까지만 관리된다.
	/// </remarks>
	public string ArrayType { get; set; } = string.Empty;

	/// <summary>
	/// 널 입력 가능 여부
	/// </summary>
	public bool NullableIs { get; set; } = false;
}
