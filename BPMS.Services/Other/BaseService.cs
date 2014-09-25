using BPMS.BLL;
using BPMS.Common;
using BPMS.Model;
using System;
using System.Data;
using System.Linq;
using System.ServiceModel;

namespace BPMS.Services
{
    public class BaseService
    {
        #region 数据库访问
        private BllProvider _BLLProvider;
        protected BllProvider BLLProvider
        {
            get
            {
                if (_BLLProvider == null)
                {
                    _BLLProvider = new BllProvider();
                }
                return _BLLProvider;
            }
            set
            {
                _BLLProvider = value;
            }
        }
        #endregion

        #region 验证权限
        private static object CheckPurviewLock = new object();


        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="objCredentials"></param>
        /// <returns>
        /// 1-有权限, -1DataProvider错误, -2密码错误, -3帐号未启用, -4没有权限
        /// </returns>
        protected int CheckPurview(ClientCredentials objCredentials, EModules moduleCode, EFunctions funCode, EActions actionCode)
        {
            return this.CheckPurview(objCredentials.Account, objCredentials.Password, objCredentials.SystemId, objCredentials.RoleId, moduleCode, funCode, actionCode);
        }
        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="objCredentials"></param>
        /// <returns>
        /// 1-有权限, -1DataProvider错误, -2密码错误, -3帐号未启用, -4没有权限
        /// </returns>
        protected int CheckPurview(string xmlCredentials, EModules moduleCode, EFunctions funCode, EActions actionCode)
        {
            return this.CheckPurview(xmlCredentials.ToModel<ClientCredentials>(), moduleCode, funCode, actionCode);
        }
        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns>
        /// 1-有权限, -1账号不存在, -2密码错误, -3帐号未启用, -4没有权限
        /// </returns>
        protected int CheckPurview(string account, string password, int systemId, int roleId, EModules moduleCode, EFunctions funCode, EActions actionCode)
        {
            int iRlt = 1;
            UserInfo userInfo = this.BLLProvider.UserInfoBLL.GetModel(t => t.Account == account);
            if (userInfo == null)
                return -1;
            string MD5PWD = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
            if (userInfo.Password != MD5PWD)
                return -2; //密码错误
            if (!userInfo.IsEnable)
                return -3;//帐号未启用  
            //超级管理员
            if (account == Consts.ConstValue.SuperAdminAccount)
                return 1;

            string cacheKey = Consts.CacheKey.BaseService_Purview + "_" + account.ToUpper();
            DataTable dt = null;
            if (iRlt == 1)
            {
                if (CacheHelper.Get(cacheKey) == null)
                {
                    lock (CheckPurviewLock)
                    {
                        if (CacheHelper.Get(cacheKey) == null)
                        {
                            CacheHelper.Add(cacheKey, this.BLLProvider.UserInfoBLL.GetUserPurview(systemId, roleId, userInfo.ID), new TimeSpan(1, 0, 0));
                        }
                    }
                }
                dt = CacheHelper.Get(cacheKey) as DataTable;
            }

            int modulePurviewID = 0;
            int functionPurviewID = 0;
            if (moduleCode != EModules.UnKnow)
            {
                DataRow[] arrRow = dt.Select(string.Format("code='{0}' AND PurviewType='{1}'",
                    moduleCode.ToString(), EPurviewType.模块.GetHashCode().ToString()));

                if (arrRow.Count() == 0)
                    return -4; //没有权限
                else
                    modulePurviewID = ConvertHelper.ObjectToInt(arrRow[0]["PurviewID"]);
            }

            if (funCode != EFunctions.UnKnow)
            {
                DataRow[] arrRow = dt.Select(string.Format("code='{0}' AND PurviewType='{1}' and parentID={2}",
                    funCode.ToString(), EPurviewType.功能.GetHashCode().ToString(), modulePurviewID));

                if (arrRow.Count() == 0)
                    return -4; //没有权限
                else
                    functionPurviewID = ConvertHelper.ObjectToInt(arrRow[0]["PurviewID"]);
            }
            if (actionCode != EActions.UnKnow)
            {
                if (dt.Select(string.Format("code='{0}' AND PurviewType='{1}' and parentID={2}",
                    actionCode.ToString(), EPurviewType.操作.GetHashCode().ToString(), functionPurviewID)).Count() == 0)
                    return -4; //没有权限
            }
            return iRlt;
        }
        #endregion

        /// <summary>
        /// 获取当前服务契约的方法名
        /// </summary>
        /// <returns></returns>
        protected string GetActionName()
        {
            string actionUrl = OperationContext.Current.IncomingMessageHeaders.Action;
            string actionName = actionUrl.Substring(actionUrl.LastIndexOf('/') + 1);
            return actionName;
        }
    }
}
