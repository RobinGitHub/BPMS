using BPMS.Common;
using BPMS.Model;
using System;
using System.Data;

namespace BPMS.Services
{
    /// <summary>
    /// 菜单
    /// </summary>
    public partial class Service
    {
        public string MenuGetList(string xmlCredentials, int systemId, int parentId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.MenuMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.MenuInfoBLL.GetList(objCredentials.UserId, objCredentials.UserName, systemId, parentId);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public string MenuGetModel(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.MenuMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.MenuInfoBLL.GetModel(id).ToXmlString();
        }

        public int MenuAdd(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.MenuMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<MenuInfo>();
            model.CreateUserId = objCredentials.UserId;
            model.CreateUserName = objCredentials.UserName;
            model.CreateDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.MenuInfoBLL.Add(model);
        }

        public int MenuEdit(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.MenuMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<MenuInfo>();
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.MenuInfoBLL.Edit(model);
        }

        public int MenuDelete(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.MenuMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.MenuInfoBLL.Delete(objCredentials.UserId, objCredentials.UserName, id);
        }

        public bool MenuNameIsRepeat(string xmlCredentials, int systemId,string name, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.MenuMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.MenuInfoBLL.IsRepeatName(systemId, name, id);
        }

        public bool MenuCodeIsRepeat(string xmlCredentials, int systemId, string code, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.MenuMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.MenuInfoBLL.IsRepeatCode(systemId, code, id);
        }
    }
}
