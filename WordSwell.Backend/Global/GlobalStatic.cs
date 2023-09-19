
using DGUtility.TimeScheduler;
using Utility.ApplicationLogger;
using Utility.FileAssist;
using WordSwell.Backend.Faculty.FileDb;

namespace Game_Adosaki.Global;

/// <summary>
/// 프로그램 전역 변수
/// </summary>
public static class GlobalStatic
{
    /// <summary>
    /// 로거
    /// </summary>
    public static DotNetLogging Log = new DotNetLogging();

    /// <summary>
    /// 디버그 모드 여부
    /// </summary>
#if DEBUG
    public static bool DebugIs = true;
#else
    public static bool DebugIs = false;
#endif

    /// <summary>
    /// 스케줄러 선언
    /// </summary>
    public static TimeScheduler TimeSked = new TimeScheduler();

    /// <summary>
    /// 파일 변환 관련
    /// </summary>
    public static FileProcess FileProc = new FileProcess();
    /// <summary>
    /// 파일DB를 관리하기위한 기능
    /// </summary>
    public static FileDbProcess FileDbProc = new FileDbProcess();


}
