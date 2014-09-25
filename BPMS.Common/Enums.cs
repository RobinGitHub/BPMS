using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.Common
{
    #region WCF的绑定协议
    /// <summary>
    /// WCF的绑定协议
    /// </summary>
    public enum EBinding
    {
        BasicHttpBinding = 0,
        WsHttpBinding = 1,
        NetTcpBinding = 2,
    }
    #endregion
}
