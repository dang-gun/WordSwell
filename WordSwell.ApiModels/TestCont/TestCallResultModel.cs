namespace WordSwell.ApiModels.TestCont;

/// <summary>
/// 테스트용 결과 모델
/// </summary>
public class TestCallResultModel
{
    /// <summary>
    /// 개체 테스트 1
    /// </summary>
    public TestCallModel? Call { get; set; }

    /// <summary>
    /// 개체 테스트 2
    /// </summary>
    public TestResultModel? Result { get; set; }
}
