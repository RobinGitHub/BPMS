using BPMS.Model;
using BPMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BPMS.Services
{
    public partial class Service
    {
        public string IPBlackGetList(string xmlCredentials)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.IPBlackMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.IPBlacklistBLL.GetList(objCredentials.UserId, objCredentials.UserName);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public string IPBlackGetModel(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.IPBlackMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.IPBlacklistBLL.GetModel(id).ToXmlString();
        }

        public int IPBlackAdd(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.IPBlackMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<IPBlacklist>();
            return this.BLLProvider.IPBlacklistBLL.Add(model);
        }

        public int IPBlackEdit(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.IPBlackMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<IPBlacklist>();
            return this.BLLProvider.IPBlacklistBLL.Edit(model);
        }

        public int IPBlackDelete(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.IPBlackMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.IPBlacklistBLL.Delete(objCredentials.UserId, objCredentials.UserName, id);
        }

    }
}
