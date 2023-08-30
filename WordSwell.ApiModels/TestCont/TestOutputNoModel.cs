using DGU_ModelToOutFiles.Global.Attributes;

namespace WordSwell.ApiModels.TestCont;

/// <summary>
/// 출력 안함
/// </summary>
[OutputNo]
public class TestOutputNoModel
{
    /// <summary>
    /// 숫자
    /// </summary>
    public int Int { get; set; }

    /// <summary>
    /// 문자열
    /// </summary>
    public string String { get; set; } = string.Empty;

    /// <summary>
    /// 날짜
    /// </summary>

    public DateTime? DateTime { get; set; }
}
