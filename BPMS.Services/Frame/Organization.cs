using BPMS.Common;
using BPMS.Model;
using System;
using System.Data;

namespace BPMS.Services
{
    public partial class Service
    {
        public string OrganGetList(string xmlCredentials)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.OrganizationMng, EFunctions.OrganMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.OrganizationBLL.GetList(objCredentials.UserId, objCredentials.UserName);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public string OrganGetModel(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.OrganizationMng, EFunctions.OrganMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.OrganizationBLL.GetModel(id).ToXmlString();
        }

        public int OrganAdd(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.OrganizationMng, EFunctions.OrganMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<Organization>();
            model.CreateUserId = objCredentials.UserId;
            model.CreateUserName = objCredentials.UserName;
            model.CreateDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.OrganizationBLL.Add(model);
        }

        public int OrganEdit(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.OrganizationMng, EFunctions.OrganMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<Organization>();
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.OrganizationBLL.Edit(model);
        }

        public int OrganDelete(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.OrganizationMng, EFunctions.OrganMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.OrganizationBLL.Delete(objCredentials.UserId, objCredentials.UserName, id);
        }

        public bool OrganNameIsRepeat(string xmlCredentials, int parentId, string name, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.OrganizationMng, EFunctions.OrganMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.OrganizationBLL.IsRepeatName(parentId, name, id);
        }

        public bool OrganCodeIsRepeat(string xmlCredentials, int parentId, string code, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.OrganizationMng, EFunctions.OrganMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.OrganizationBLL.IsRepeatCode(parentId, code, id);
        }


    }
}
