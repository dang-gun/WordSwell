
using Utility.FileAssist;

namespace Game_Adosaki.Global;

/// <summary>
/// 프로그램 전역 변수
/// </summary>
public static class GlobalStatic
{
    /// <summary>
    /// 디버그 모드 여부
    /// </summary>
#if DEBUG
    public static bool DebugIs = true;
#else
    public static bool DebugIs = false;
#endif



    /// <summary>
    /// 파일 변환 관련
    /// </summary>
    public static FileProcess FileProc = new FileProcess();


}
