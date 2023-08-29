using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.ModelToFrontend;

/// <summary>
/// 최종 출력을 위해 텍스트를 정리해둔 모델
/// </summary>
public class ModelToStringModel
{
    /// <summary>
    /// 임포트 영역
    /// </summary>
    /// <remarks>
    /// C#은 using 영역
    /// </remarks>
    public List<string> Import = new List<string>();
    /// <summary>
    /// 임포트 영역 추가 영역
    /// </summary>
    public List<string> ImportAdd = new List<string>();


    /// <summary>
    /// 머리 주석
    /// </summary>
    public List<string> HeadSummary = new List<string>();
    /// <summary>
    /// 머리 이름
    /// </summary>
    public List<string> HeadName = new List<string>();

    /// <summary>
    /// 몸통 시작
    /// </summary>
    public List<string> BodyStart = new List<string>();

    /// <summary>
    /// 요소
    /// </summary>
    public List<ModelToStringItemModel> ItemList = new List<ModelToStringItemModel>();

    /// <summary>
    /// 몸통 끝
    /// </summary>
    public List<string> BodyEnd = new List<string>();

    /// <summary>
    /// 바닥
    /// </summary>
    public List<string> Footer = new List<string>();

    
}

/// <summary>
/// 최종 출력을 위해 텍스트를 정리해둔 모델의 아이템
/// </summary>
public class ModelToStringItemModel
{
    /// <summary>
    /// 주석
    /// </summary>
    public List<string> Summary = new List<string>();

    /// <summary>
    /// 요소 선언
    /// </summary>
    public List<string> Item = new List<string>();
}