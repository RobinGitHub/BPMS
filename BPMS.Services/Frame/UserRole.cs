using BPMS.Common;
using BPMS.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace BPMS.Services
{
    public partial class Service
    {
        public int UserRoleSet(string xmlCredentials, int systemId, int userId, List<int> lstRoleId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.UserMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.UserRoleBLL.UserRoleSet(objCredentials.UserId, objCredentials.UserName, systemId, userId, lstRoleId);
        }

        public string UserRoleGetList(string xmlCredentials, int systemId, int userId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.UserMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable rltDt = this.BLLProvider.UserRoleBLL.GetList(objCredentials.UserId, objCredentials.UserName, systemId, userId);
            return ZipHelper.CompressDataTable(rltDt);
        }
    }
}
