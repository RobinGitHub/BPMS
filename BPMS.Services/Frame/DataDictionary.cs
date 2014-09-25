using BPMS.Common;
using BPMS.Model;
using System;
using System.Data;

namespace BPMS.Services
{
    public partial class Service
    {
        /// <summary>
        /// 获取数据字典列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        public string DataDictGetList(string xmlCredentials, int systemId, int dictType)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.DataDictMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable rltDt = this.BLLProvider.DataDictionaryBLL.GetList(objCredentials.UserId, objCredentials.UserName, systemId, dictType);
            return ZipHelper.CompressDataTable(rltDt);
        }
        /// <summary>
        /// 获取数据字典对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DataDictGetModel(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.DataDictMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.DataDictionaryBLL.GetModel(id).ToXmlString();
        }
        /// <summary>
        /// 数据字典添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        public int DataDictAdd(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.DataDictMng, EActions.Add) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<DataDictionary>();
            model.CreateUserId = objCredentials.UserId;
            model.CreateUserName = objCredentials.UserName;
            model.CreateDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.DataDictionaryBLL.Add(model);
        }
        /// <summary>
        /// 数据字典编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        public int DataDictEdit(string xmlCredentials, string xmlModel)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.DataDictMng, EActions.Upd) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            var model = xmlModel.ToModel<DataDictionary>();
            model.ModifyDate = DateTime.Now;
            model.ModifyUserId = objCredentials.UserId;
            model.ModifyUserName = objCredentials.UserName;
            return this.BLLProvider.DataDictionaryBLL.Edit(model);
        } 
        /// <summary>
        /// 数据字典删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DataDictDelete(string xmlCredentials, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.DataDictMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.DataDictionaryBLL.Delete(objCredentials.UserId, objCredentials.UserName, id);
        }
        /// <summary>
        /// 数据字典名称是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool DataDictNameIsRepeat(string xmlCredentials, int dictType, string name, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.DataDictMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.DataDictionaryBLL.IsRepeatName(dictType, name, id);
        }
        /// <summary>
        /// 数据字典编码是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool DataDictCodeIsRepeat(string xmlCredentials, int dictType, string code, int id)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.DataDictMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.DataDictionaryBLL.IsRepeatCode(dictType, code, id);
        }
    }
}
