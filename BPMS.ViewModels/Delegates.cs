using BPMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BPMS.ViewModels
{
    #region 返回结果委托
    /// <summary>
    /// 返回结果委托
    /// </summary>
    /// <param name="rlt"></param>
    [Serializable]
    [ComVisible(true)]
    public delegate void GetResultEventHandle(Result rlt);
    #endregion
}
