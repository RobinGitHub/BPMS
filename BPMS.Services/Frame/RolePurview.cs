using BPMS.Common;
using BPMS.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace BPMS.Services
{
    public partial class Service
    {
        public int RolePurviewSet(string xmlCredentials, int roleId, List<int> purviewIdList)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RolePurviewMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.RolePurviewBLL.PurviewSet(objCredentials.UserId, objCredentials.UserName, roleId, purviewIdList);
        }

        public string RoleGetPurviewList(string xmlCredentials, int systemId, int roleId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RolePurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable rltDt = this.BLLProvider.RolePurviewBLL.GetPurviewList(objCredentials.UserId, objCredentials.UserName, systemId, roleId);
            return ZipHelper.CompressDataTable(rltDt);
        }

        /// <summary>
        /// 角色成员
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <returns>用户列表</returns>
        public string RoleMembers(string xmlCredentials, int roleId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RolePurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable rltDt = this.BLLProvider.RolePurviewBLL.RoleMembers(roleId);
            return ZipHelper.CompressDataTable(rltDt);
        }
        /// <summary>
        /// 角色成员添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        public int RoleMemberAdd(string xmlCredentials, int roleId, List<int> userIdList)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RolePurviewMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.RolePurviewBLL.Add(objCredentials.UserId, objCredentials.UserName, roleId, userIdList);
        }
        /// <summary>
        /// 角色成员删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int RoleMemberDelete(string xmlCredentials, int roleId, int userId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RolePurviewMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.RolePurviewBLL.Delete(objCredentials.UserId, objCredentials.UserName, roleId, userId);
        }


    }
}
