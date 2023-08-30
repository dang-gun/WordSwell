
namespace DGU_ModelToOutFiles.App.Faculty;

/// <summary>
/// 네임스페이스에 소속된 개체 리스트
/// </summary>
public class NamespaceTargetModel
{
    /// <summary>
    /// 로드할 어셈블리 이름
    /// </summary>
    public string AssemblyName { get; set; } = string.Empty;
    
    /// <summary>
    /// 허용할 네임스페이스 리스트
    /// </summary>
    public string[] NamespaceList { get; set; } = new string[0];

}
