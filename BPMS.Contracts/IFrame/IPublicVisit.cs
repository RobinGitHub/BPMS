using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace BPMS.Contracts
{
    /// <summary>
    /// 公共方法访问 不验证权限
    /// </summary>
    public partial interface IService
    {
        /// <summary>
        /// 获取启用的系统列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string PubGetSystemList();
        /// <summary>
        /// 获取系统启用的角色列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string PubGetRoleList(int systemId);
        /// <summary>
        /// 获取系统所有的权限
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        [OperationContract]
        string PubGetAllPurview(int systemId);
        /// <summary>
        /// 用户权限列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        string PubGetUserPurviewList(int systemId, int roleId, int userId);
        /// <summary>
        /// 查询部门下的员工
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="organId"></param>
        /// <returns></returns>
        [OperationContract]
        string PubGetUserByDeptId(int deptId);

    }
}
