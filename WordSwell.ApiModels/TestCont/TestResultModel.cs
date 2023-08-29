namespace WordSwell.ApiModels.TestCont;

/// <summary>
/// 테스트용 결과 모델
/// </summary>
public class TestResultModel
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
