using BPMS.Common;
using BPMS.Model;
using System;
using System.Data;

namespace BPMS.Services
{
    public partial class Service
    {
        public string RoleGetList(string xmlCredentials, int systemId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RoleMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.RoleInfoBLL.GetList(objCredentials.UserId, objCredentials.UserName, systemId);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public string RoleGetModel(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RoleMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.RoleInfoBLL.GetModel(id).ToXmlString();
        }

        public int RoleAdd(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RoleMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<RoleInfo>();
            model.CreateUserId = objCredentials.UserId;
            model.CreateUserName = objCredentials.UserName;
            model.CreateDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.RoleInfoBLL.Add(model);
        }

        public int RoleEdit(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RoleMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<RoleInfo>();
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.RoleInfoBLL.Edit(model);
        }

        public int RoleDelete(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RoleMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.RoleInfoBLL.Delete(objCredentials.UserId, objCredentials.UserName, id);
        }

        public bool RoleNameIsRepeat(string xmlCredentials, int systemId, string name, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RoleMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.RoleInfoBLL.IsRepeatName(systemId, name, id);
        }

        public bool RoleCodeIsRepeat(string xmlCredentials, int systemId, string code, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.RoleMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.RoleInfoBLL.IsRepeatCode(systemId, code, id);
        }

    }
}
