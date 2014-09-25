using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.Model;
using System.ServiceModel;

namespace BPMS.Contracts 
{ 
    /// <summary> 
    /// 系统日志
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string SysLogGetList(string xmlCredentials, DateTime startDate, DateTime endDate, int operationType, string moduleName, string account, int pageIndex, int pageSize, out int count);

        /// <summary>
        /// 清除日志
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract]
        bool SysLogClear(string xmlCredentials, DateTime endDate);
    }
} 


