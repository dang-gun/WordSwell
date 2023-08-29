using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.ProjectXml;

namespace WordSwell.Tool.ApiModels.Faculty.ObjectToOut;

internal interface ObjectToOutInterface
{
    /// <summary>
    /// 대상으로 변환하고 지정된 위치에 저장한다.
    /// </summary>
    public void ToTargetSave();
}
