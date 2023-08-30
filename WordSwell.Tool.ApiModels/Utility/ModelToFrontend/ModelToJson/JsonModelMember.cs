

namespace DGUtility.ModelToFrontend;

/// <summary>
/// 모델의 멤버 정보를 검색하기 쉽게 저장한다.
/// </summary>
public class JsonModelMember
{
	/// <summary>
	/// 맴버 이름 
	/// </summary>
	public string Name { get; set; } = string.Empty;
	
	/// <summary>
	/// 맴버가 가지고 있는 값
	/// </summary>
	public string Value { get; set; } = string.Empty;
	
}
