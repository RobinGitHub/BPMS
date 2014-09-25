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
        /// 用户权限列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        string UserGetPurviewList(string xmlCredentials, int systemId, int roleId, int userId);
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="lstModel">用户权限对象</param>
        /// <returns></returns>
        [OperationContract]
        int UserPurviewSet(string xmlCredentials, List<string> lstModel);
        /// <summary>
        /// 重置权限
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        int UserPurviewReset(string xmlCredentials, int systemId, int roleId, int userId);

    }
} 


