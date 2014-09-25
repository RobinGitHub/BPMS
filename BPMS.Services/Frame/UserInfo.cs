using BPMS.Common;
using BPMS.Model;
using System;
using System.Data;

namespace BPMS.Services
{
    public partial class Service
    {
        public string UserGetList(string xmlCredentials, int searchType, string keyWord, int isEnable, int pageIndex, int pageSize, out int count)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.UserMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable rltDt = this.BLLProvider.UserInfoBLL.GetList(objCredentials.UserId, objCredentials.UserName, searchType, keyWord, isEnable, pageIndex, pageSize, out count);
            return ZipHelper.CompressDataTable(rltDt);
        }

        public string UserGetModel(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.UserMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.UserInfoBLL.GetModel(id).ToXmlString();
        }

        public int UserAdd(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.UserMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<UserInfo>();
            model.CreateUserId = objCredentials.UserId;
            model.CreateUserName = objCredentials.UserName;
            model.CreateDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.UserInfoBLL.Add(model);
        }

        public int UserEdit(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.UserMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<UserInfo>();
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.UserInfoBLL.Edit(model);
        }

        public bool UserAccountIsRepeat(string xmlCredentials, string account, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.UserMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.UserInfoBLL.IsRepeatAccount(account, id);
        }

        public bool UserCodeIsRepeat(string xmlCredentials, string code, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.UserMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.UserInfoBLL.IsRepeatCode(code, id);
        }

        public bool UserPasswordReset(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.PurviewMng, EFunctions.UserMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.UserInfoBLL.PasswordReset(objCredentials.UserId, objCredentials.UserName, id);
        }
    }
}
