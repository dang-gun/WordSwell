using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WordSwell.Tool.ApiModels.Faculty.ObjectToOut;

/// <summary>
/// 오브젝트 출력 타입
/// </summary>
public enum ObjectOutType
{
    /// <summary>
    /// 상태 없음
    /// </summary>
    None = 0,

    /// <summary>
    /// 클래스
    /// </summary>
    Class,

    /// <summary>
    /// 열거형
    /// </summary>
    Enum,

    /// <summary>
    /// 열거형 - const를 붙이지 않음
    /// </summary>
    Enum_ConstNo,
}
