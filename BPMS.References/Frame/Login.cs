using BPMS.Common;
using BPMS.Model;
using BPMS.References.BPMService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace BPMS.References
{
    /// <summary>
    /// 登录
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="systemCode"></param>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <param name="systemId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="roleList"></param>
        /// <returns>
        /// 0登录失败
        /// 1登录成功
        /// 2账号不存在
        /// 3用户已禁用
        /// 4密码不正确
        /// 5当前IP不许登录
        /// 6当前用户没有任何权限
        /// </returns>
        public int Login(string systemCode, string account, string pwd, out int systemId, out int userId, out string userName, out List<RoleInfo> roleList)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            systemId = 0;
            userId = 0;
            userName = "";
            roleList = null;
            string xmlRoleList = "";
            try
            {
                int rlt = _proxy.Login(out systemId, out userId, out userName, out xmlRoleList, systemCode, account, pwd);
                roleList = xmlRoleList.ToList<RoleInfo>();
                return rlt;
            }
            catch (EndpointNotFoundException endPointEx)
            {
                throw endPointEx.InnerException;
            }
            catch (Exception ex)
            {
                if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                    CreateOpenClient();
                throw ex;
            }
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns>
        /// 0 操作失败，请联系管理员
        /// 1 操作成功
        /// 2 账号不存在
        /// 4 密码错误！
        /// </returns>
        public int LoginChangePassword(string oldPwd, string newPwd)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.LoginChangePassword(User.Current.ID, oldPwd, newPwd);
            });
        }
        #endregion

        #region 用户菜单
        /// <summary>
        /// 用户菜单
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable LogGetUserMenuList()
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.LogGetUserMenuList(User.Current.SystemId, User.Current.RoleId, User.Current.ID);
            });
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 系统退出
        /// <summary>
        /// 系统退出
        /// </summary>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 2账号不存在
        /// </returns>
        public int LogOut()
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.LogOut(User.Current.SystemId, User.Current.ID);
            });
        }
        #endregion
    }
}
