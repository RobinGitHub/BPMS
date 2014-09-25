using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.Model;
using System.ServiceModel;

namespace BPMS.Contracts 
{ 
    /// <summary> 
    /// 
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取系统登录日志列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string SysLoginLogGetList(string xmlCredentials, DateTime startDate, DateTime endDate, string account, int pageIndex, int pageSize, out int count);

        /// <summary>
        /// 清除登录日志
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract]
        bool SysLoginLogClear(string xmlCredentials, DateTime endDate);
    }
} 


