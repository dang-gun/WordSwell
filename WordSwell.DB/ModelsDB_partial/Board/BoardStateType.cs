using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WordSwell.DB.ModelsDB_partial.Board;

/// <summary>
/// 보드 상태
/// </summary>
public enum BoardStateType
{
    /// <summary>
    /// 상태 없음
    /// </summary>
    None = 0,

    /// <summary>
    /// 열림
    /// </summary>
    Open = 100,

    /// <summary>
    /// 닫침
    /// </summary>
    Close = 200,
}
