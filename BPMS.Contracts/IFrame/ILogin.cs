using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BPMS.Contracts
{
    /// <summary>
    /// 登录
    /// </summary>
    public partial interface IService
    {
        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="account">用户</param>
        /// <param name="pwd">密码</param>
        /// <param name="userId">用户ID</param>
        /// <returns>
        /// 1登录成功
        /// 2用户不存在
        /// 3用户已禁用
        /// 4密码不正确
        /// 5未设置系统角色，当前用户没有登录系统的权限
        /// 6当前IP不许登录
        /// </returns>
        [OperationContract]
        int Login(string systemCode, string account, string pwd, out int systemId, out int userId, out string userName, out string xmlRoleList);
        /// <summary>
        /// 系统退出
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [OperationContract]
        int LogOut(int systemId, int userId);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        [OperationContract]
        int LoginChangePassword(int userId, string oldPwd, string newPwd);
        /// <summary>
        /// 登陆获取所有有权限菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        string LogGetUserMenuList(int systemId, int roleId, int userId);


    }
}
