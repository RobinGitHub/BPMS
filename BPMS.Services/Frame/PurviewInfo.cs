using BPMS.Common;
using BPMS.Model;
using System;
using System.Data;

namespace BPMS.Services
{
    /// <summary>
    /// 权限
    /// </summary>
    public partial class Service
    {
        public string PurviewGetModuleList(string xmlCredentials, int systemId, string moduleName, string moduleCode, int isEnable, int pageIndex, int pageSize, out int count)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.PurviewInfoBLL.GetModuleList(objCredentials.UserId, objCredentials.UserName, systemId, moduleName, moduleCode, isEnable, pageIndex, pageSize, out count);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public string PurviewGetFunctionList(string xmlCredentials, int systemId, int moduleId, string functionName, string functionCode, int isEnable, int pageIndex, int pageSize, out int count)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.PurviewInfoBLL.GetFunctionList(objCredentials.UserId, objCredentials.UserName, systemId, moduleId, functionName, functionCode, isEnable, pageIndex, pageSize, out count);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public string PurviewGetActionList(string xmlCredentials, int systemId, int moduleId, int functionId, string actionName, string actionCode, int isEnable, int pageIndex, int pageSize, out int count)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.PurviewInfoBLL.GetActionList(objCredentials.UserId, objCredentials.UserName, systemId, moduleId, functionId, actionName, actionCode, isEnable, pageIndex, pageSize, out count);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public string PurviewGetListByParentId(string xmlCredentials, int systemId, int parentId)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.PurviewInfoBLL.GetListByParentId(systemId, parentId);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public string PurviewGetModel(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.PurviewInfoBLL.GetModel(id).ToXmlString();
        }

        public int PurviewAdd(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<PurviewInfo>();
            model.CreateUserId = objCredentials.UserId;
            model.CreateUserName = objCredentials.UserName;
            model.CreateDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.PurviewInfoBLL.Add(model);
        }

        public int PurviewEdit(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<PurviewInfo>(); 
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.PurviewInfoBLL.Edit(model);
        }

        public int PurviewDelete(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.PurviewInfoBLL.Delete(objCredentials.UserId, objCredentials.UserName, id);
        }

        public bool PurviewNameIsRepeat(string xmlCredentials, int systemId, int parentId, string name, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.PurviewInfoBLL.IsRepeatName(objCredentials.SystemId, parentId, name, id);
        }

        public bool PurviewCodeIsRepeat(string xmlCredentials, int systemId, int parentId, string code, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.PurviewMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.PurviewInfoBLL.IsRepeatCode(objCredentials.SystemId, parentId, code, id);
        }
    }
}
