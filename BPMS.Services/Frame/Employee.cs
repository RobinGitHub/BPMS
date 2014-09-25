using BPMS.Common;
using BPMS.Model;
using System;
using System.Data;
namespace BPMS.Services
{
    public partial class Service
    {
        public string EmployeeGetList(string xmlCredentials, int category, int orgaId, int searchType, string keyWord, int pageIndex, int pageSize, out int count)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.EmployeeMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable rltDt = this.BLLProvider.EmployeeBLL.GetList(objCredentials.UserId, objCredentials.UserName, (EOrgaCategory)category, orgaId, searchType, keyWord, pageIndex, pageSize, out count);
            return ZipHelper.CompressDataTable(rltDt);
        }

        public string EmployeeGetModel(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.EmployeeMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.EmployeeBLL.GetModel(id).ToXmlString();
        }

        public int EmployeeAdd(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.EmployeeMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<Employee>();
            model.CreateUserId = objCredentials.UserId;
            model.CreateUserName = objCredentials.UserName;
            model.CreateDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.EmployeeBLL.Add(model);
        }

        public int EmployeeEdit(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.EmployeeMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<Employee>();
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.EmployeeBLL.Edit(model);

        }

        public bool EmployeeCodeIsRepeat(string xmlCredentials, string code, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.EmployeeMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.EmployeeBLL.IsRepeatCode(code, id);
        }
    }
}
