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
    /// 角色权限
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 设置权限
        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="purviewIdList"></param>
        /// <returns>
        /// 0 失败
        /// 1 成功
        /// </returns>
        public int RolePurviewSet(int roleId, List<int> purviewIdList)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.RolePurviewSet(User.Current.Credentials.ToXmlString(), roleId, purviewIdList.ToArray());
            });
        }
        #endregion

        #region 获取角色权限
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable RoleGetPurviewList(int systemId, int roleId)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.RoleGetPurviewList(User.Current.Credentials.ToXmlString(), systemId, roleId);
            });
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 角色成员
        /// <summary>
        /// 角色成员
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>用户列表</returns>
        public DataTable RoleMembers(int roleId)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.RoleMembers(User.Current.Credentials.ToXmlString(), roleId);
            });
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 角色成员添加
        /// <summary>
        /// 角色成员添加
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIdList"></param>
        /// <returns>
        /// 0 成功
        /// 1 失败</returns>
        public int RoleMemberAdd(int roleId, List<int> userIdList)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.RoleMemberAdd(User.Current.Credentials.ToXmlString(), roleId, userIdList.ToArray());
            });
        }
        #endregion

        #region 角色成员删除
        /// <summary>
        /// 角色成员删除
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns>
        /// 0
        /// 1
        /// 15 该成员不能删除，因为该成员只有一个角色
        /// </returns>
        public int RoleMemberDelete(int roleId, int userId)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.RoleMemberDelete(User.Current.Credentials.ToXmlString(), roleId, userId);
            });
        }
        #endregion
    }
}
