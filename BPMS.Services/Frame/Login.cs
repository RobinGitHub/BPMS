using BPMS.Common;
using BPMS.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace BPMS.Services
{
    public partial class Service
    {
        public int Login(string systemCode, string account, string pwd, out int systemId, out int userId, out string userName, out string xmlRoleList)
        {
            List<RoleInfo> roleList = null;
            int rlt = this.BLLProvider.UserInfoBLL.Login(systemCode, account, pwd, out systemId, out userId, out userName, out roleList);
            xmlRoleList = roleList.ToXmlString();
            return rlt;
        }

        public int LoginChangePassword(int userId, string oldPwd, string newPwd)
        {
            return this.BLLProvider.UserInfoBLL.ChangePassword(userId, oldPwd, newPwd);
        }

        public string LogGetUserMenuList(int systemId, int roleId, int userId)
        {
            DataTable rltDt = this.BLLProvider.UserInfoBLL.GetUserMenu(systemId, roleId, userId);
            return ZipHelper.CompressDataTable(rltDt);
        }

        public int LogOut(int systemId, int userId)
        {
            return this.BLLProvider.UserInfoBLL.LogOut(systemId, userId);
        }
    }
}
