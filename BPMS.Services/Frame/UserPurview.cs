using BPMS.Common;
using BPMS.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace BPMS.Services
{
    public partial class Service
    {
        public string UserGetPurviewList(string xmlCredentials, int systemId, int roleId, int userId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.UserPurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable rltDt = this.BLLProvider.UserPurviewBLL.GetList(objCredentials.UserId, objCredentials.UserName, systemId, roleId, userId);
            return ZipHelper.CompressDataTable(rltDt);
        }

        public int UserPurviewSet(string xmlCredentials, List<string> lstModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.UserPurviewMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            List<UserPurview> modelList = new List<UserPurview>();
            foreach (var item in lstModel)
            {
                modelList.Add(item.ToModel<UserPurview>());
            }
            return this.BLLProvider.UserPurviewBLL.UserPurviewSet(objCredentials.UserId, objCredentials.UserName, modelList);
        }

        public int UserPurviewReset(string xmlCredentials, int systemId, int roleId, int userId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.UserPurviewMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.UserPurviewBLL.Delete(objCredentials.UserId, objCredentials.UserName, systemId, roleId, userId);
        }

    }
}
