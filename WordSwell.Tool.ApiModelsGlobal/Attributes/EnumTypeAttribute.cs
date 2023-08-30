using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSwell.Tool.ApiModelsGlobal.Attributes;

/// <summary>
/// 열거형 추가 처리 속성
/// </summary>
/// <remarks>
/// 타입 스크립트와 같이 필요에 따라 const를 붙여야 할지 말아야 할지를 결정해야 할때
/// 이 속성으로 처리한다.
/// </remarks>
[System.AttributeUsage(System.AttributeTargets.Enum)]
public class EnumTypeAttribute : System.Attribute
{
    /// <summary>
    /// 타입스크립트로 변환하는 경우 const를 붙이지 말지 여부.
    /// </summary>
    /// <remarks>
    /// true이면 붙이지 않는다.
    /// </remarks>
    public bool TypeScript_EnumNoConstIs;

    public EnumTypeAttribute()
    {
        this.TypeScript_EnumNoConstIs = true;
    }
}

/// <summary>
/// EnumTypeAttribute가 있는지 확인하고 있으면 개체를 리턴해주는 클래스
/// </summary>
public class EnumTypeAttributeCheck
{
    public EnumTypeAttribute? Check(Type type)
    {
        EnumTypeAttribute? etReturn =
            type.GetCustomAttributes(typeof(EnumTypeAttribute), false)
                    .Cast<EnumTypeAttribute>()
                    .FirstOrDefault();
        return etReturn;
    }
}