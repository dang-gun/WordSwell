using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSwell.Tool.ApiModelsGlobal.Attributes;

/// <summary>
/// 파일 출력시 이 개체는 제외한다.
/// </summary>
[System.AttributeUsage(
    System.AttributeTargets.Class 
    | System.AttributeTargets.Enum)]
public class OutputNoAttribute : System.Attribute
{
    /// <summary>
    /// 다른 프로젝트로 변환하는 경우 이 개체를 제외할지 여부
    /// </summary>
    /// <remarks>
    /// true이면 출력하지 않는다.
    /// </remarks>
    public bool OutputNoIs;

    public OutputNoAttribute()
    {
        this.OutputNoIs = true;
    }
}

/// <summary>
/// EnumTypeAttribute가 있는지 확인하고 있으면 개체를 리턴해주는 클래스
/// </summary>
public class OutputNoAttributeCheck
{
    public OutputNoAttribute? Check(Type type)
    {
        OutputNoAttribute? etReturn =
            type.GetCustomAttributes(typeof(OutputNoAttribute), false)
                    .Cast<OutputNoAttribute>()
                    .FirstOrDefault();
        return etReturn;
    }
}
