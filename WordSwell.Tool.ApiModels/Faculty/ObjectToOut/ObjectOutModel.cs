using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WordSwell.Tool.ApiModels.Faculty.ObjectToOut;


/// <summary>
/// 내보내기용 오브젝트 모델
/// </summary>
internal class ObjectOutModel
{
    /// <summary>
    /// 소속된 어셈블리
    /// </summary>
    public Assembly Assembly { get; set; } = Assembly.GetExecutingAssembly();

    /// <summary>
    /// 소속된 네임스페이스
    /// </summary>
    public string Namespace { get; set; } = string.Empty;
    /// <summary>
    /// 소속된 네임스페이스에서 어셈블리 네임스페이스를 제외한 네임스페이스
    /// </summary>
    public string Namespace_Cut { get; set; } = string.Empty;

    /// <summary>
    /// 클레스 이름
    /// </summary>
    public string ClassName { get; set; } = string.Empty;
    /// <summary>
    /// 모든 네임스페이스까지 포함한 클래스 이름
    /// </summary>
    public string ClassNameFull
    {
        get
        {
            return $"{this.Namespace}.{this.ClassName}";
        }
    }

    /// <summary>
    /// 개체 출력 타입
    /// </summary>
    public ObjectOutType ObjectOutType { get; set; } = ObjectOutType.None;


    /// <summary>
    /// 생성된 개체
    /// </summary>
    public object? Instance { get; set; }


    /// <summary>
    /// 내보낼 상대 물리 경로(파일이름 없음)
    /// </summary>
    public List<string> OutPhysicalPathList { get; set; } = new List<string>();
    /// <summary>
    /// 내보낼 상대 물리 경로
    /// </summary>
    public string OutPhysicalPath { get; set; } = string.Empty;
    /// <summary>
    /// 이름까지 포함된 내보낼 상대 물리 경로
    /// </summary>
    public string OutPhysicalFullPath 
    { 
        get
        {
            return Path.Combine(OutPhysicalPath, this.ClassName);
        }
    }
    

    /// <summary>
    /// 네임스페이스 기반으로 내보낼 상대 물리경로를 생성한다.
    /// </summary>
    public void OutPhysicalPath_Create()
    {
        this.OutPhysicalPath = string.Empty;

        //네임스페이스 자르기
        string[] arrNs = this.Namespace_Cut.Split('.');
        foreach (string ns in arrNs)
        {
            this.OutPhysicalPath = Path.Combine(this.OutPhysicalPath, ns);
        }
    }

    /// <summary>
    /// 논리 경로
    /// </summary>
    /// <remarks>
    /// 지정된 언어에 따라 사용될 참조 경로이다.<br />
    /// 언어에 따라 물리 경로가 될 수 있고 네임스페이스가 될 수 있다.
    /// </remarks>
    public string LogicPath { get; set; } = string.Empty;



    /// <summary>
    /// 임포트 영역에 추가될 내용
    /// </summary>
    /// <remarks>
    /// C#이면 using, 타입스크립트는 Import영영에 추가될 문자열
    /// </remarks>
    public string ImportAdd { get; set; } = string.Empty;
    /// <summary>
    /// 생성되고 마지막에 추가될 내용
    /// </summary>
    public string LastText { get; set; } = string.Empty;
}

