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
        /// 设置角色权限
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <param name="xmlPurviewIdList"></param>
        /// <returns></returns>
        [OperationContract]
        int RolePurviewSet(string xmlCredentials, int roleId, List<int> purviewIdList);
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        string RoleGetPurviewList(string xmlCredentials, int systemId, int roleId);
        /// <summary>
        /// 角色成员
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <returns>用户列表</returns>
        [OperationContract]
        string RoleMembers(string xmlCredentials, int roleId);
        /// <summary>
        /// 角色成员添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        [OperationContract]
        int RoleMemberAdd(string xmlCredentials, int roleId, List<int> userIdList);
        /// <summary>
        /// 角色成员删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        int RoleMemberDelete(string xmlCredentials, int roleId, int userId);
    }
} 


