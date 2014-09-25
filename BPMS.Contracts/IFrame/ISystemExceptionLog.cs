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
        /// 获取异常日志
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string SystemExceptionGetList(string xmlCredentials, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, out int count);

        /// <summary>
        /// 清除日志 清空数据库
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        bool SystemExceptionClear(string xmlCredentials);


        
    }
} 


