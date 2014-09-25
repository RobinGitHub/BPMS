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
        /// 用户权限设置
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int UserRoleSet(string xmlCredentials, int systemId, int userId, List<int> lstRoleId);
        /// <summary>
        /// 用户权限设置
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        string UserRoleGetList(string xmlCredentials, int systemId, int userId);

    }
} 


