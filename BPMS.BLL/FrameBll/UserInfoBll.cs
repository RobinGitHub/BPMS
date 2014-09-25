using BPMS.Common;
using BPMS.DAL;
using BPMS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using System.Web.Security;

namespace BPMS.BLL
{
    /// <summary> 
    /// 
    /// </summary>  
    public partial class UserInfoBll : BaseTableBLL<UserInfo, UserInfoDal>
    {
        #region 构造函数
        public UserInfoBll() { }

        public UserInfoBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="searchType">
        /// 1=编号、2=账号、3=姓名
        /// </param>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName, int searchType, string keyWord, int isEnable, int pageIndex, int pageSize, out int count)
        {
            count = 0;
            try
            {
                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "UserInfo";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion
                DataTable rltDt = dal.GetList(searchType, keyWord, isEnable, pageIndex, pageSize, out count);
                var datas = this.BLLProvider.DataDictionaryBLL.AllDictionary;
                rltDt.Columns.Add("DutyName");
                rltDt.Columns.Add("TitleName");
                foreach (DataRow item in rltDt.Rows)
                {
                    item["DutyName"] = datas.Find(t => t.ID == int.Parse(item["DutyId"].ToString())).Name;
                    item["TitleName"] = datas.Find(t => t.ID == int.Parse(item["TitleId"].ToString())).Name;
                }
                return rltDt;
            }
            catch (Exception ex)
            {
                this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
            }
            return null;
        }
        #endregion

        #region 获取对象
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserInfo GetModel(int id)
        {
            return GetModel(t => t.ID == id);
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0 失败
        /// 1 成功
        /// 17 账号重复
        /// 13 编码重复
        /// </returns>
        public int Add(UserInfo model)
        {
            int rlt = 1;
            if (IsRepeatAccount(model.Account, model.ID))
                rlt = 17;
            if (rlt == 1 && IsRepeatCode(model.Code, model.ID))
                rlt = 13;
            if (rlt == 1)
            {
                using (var ctx = TranContext.BeginTran())
                {
                    try
                    {
                        model.ID = this.GetNewID();

                        string md5Pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Account, "MD5").Substring(0, 20);
                        model.Password = md5Pwd;

                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "UserInfo";
                        sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                        sysLogModel.CreateUserId = model.ModifyUserId;
                        sysLogModel.CreateUserName = model.ModifyUserName;
                        sysLogModel.ObjectId = model.ID;
                        sysLogModel.OperationType = EOperationType.新增.GetHashCode();

                        var entry = ctx.Entry(model);
                        if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, entry))
                            rlt = 0;
                        #endregion
                        if (rlt == 1 && !dal.Insert(ctx, model))
                            rlt = 0;
                        if (rlt == 1)
                            TranContext.Commit(ctx);
                        else
                            TranContext.Rollback(ctx);
                    }
                    catch (Exception ex)
                    {
                        rlt = 0;
                        TranContext.Rollback(ctx);
                        this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
                    }
                }
            }
            return rlt;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 11当前对象已不存在
        /// 17账号重复
        /// 13编码重复
        /// </returns>
        public int Edit(UserInfo model)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    var oldModel = dal.GetModel(ctx, t => t.ID == model.ID);
                    if (oldModel == null)
                        rlt = 11;
                    if (rlt == 1 && IsRepeatAccount(model.Account, model.ID))
                        rlt = 17;
                    if (rlt == 1 && IsRepeatCode(model.Code, model.ID))
                        rlt = 13;
                    if (rlt == 1)
                    {
                        #region Model
                        oldModel.Name = model.Name;
                        oldModel.Spell = model.Spell;
                        oldModel.Alias = model.Alias;
                        oldModel.RoleId = model.RoleId;
                        oldModel.Gender = model.Gender;
                        oldModel.Mobile = model.Mobile;
                        oldModel.Telephone = model.Telephone;
                        oldModel.Birthday = model.Birthday;
                        oldModel.Email = model.Email;
                        oldModel.OICQ = model.OICQ;
                        oldModel.DutyId = model.DutyId;
                        oldModel.TitleId = model.TitleId;
                        oldModel.CompanyId = model.CompanyId;
                        oldModel.DepartmentId = model.DepartmentId;
                        oldModel.WorkgroupId = model.WorkgroupId;
                        oldModel.Remark = model.Remark;
                        oldModel.IsEnable = model.IsEnable;
                        oldModel.SortIndex = model.SortIndex;
                        oldModel.ModifyDate = DateTime.Now;
                        oldModel.ModifyUserId = model.ModifyUserId;
                        oldModel.ModifyUserName = model.ModifyUserName;
                        #endregion

                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "UserInfo";
                        sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                        sysLogModel.CreateUserId = model.ModifyUserId;
                        sysLogModel.CreateUserName = model.ModifyUserName;
                        sysLogModel.ObjectId = model.ID;
                        sysLogModel.OperationType = EOperationType.修改.GetHashCode();

                        var entry = ctx.Entry(oldModel);
                        if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, entry))
                            rlt = 0;
                        #endregion

                        if (rlt == 1 && !dal.Insert(ctx, oldModel))
                            rlt = 0;
                    }
                    if (rlt == 1)
                        TranContext.Commit(ctx);
                    else
                        TranContext.Rollback(ctx);
                }
                catch (Exception ex)
                {
                    rlt = 0;
                    TranContext.Rollback(ctx);
                    this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
                }
            }
            return rlt;
        }
        #endregion

        #region 重置密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PasswordReset(int logUserId, string logUserName, int id)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    var model = dal.GetModel(ctx, t => t.ID == id);
                    string md5Pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Account, "MD5").Substring(0, 20);
                    model.Password = md5Pwd;

                    #region 系统日志
                    SysLog sysLogModel = new SysLog();
                    sysLogModel.TableName = "UserInfo";
                    sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                    sysLogModel.CreateUserId = logUserId;
                    sysLogModel.CreateUserName = logUserName;
                    sysLogModel.ObjectId = model.ID;
                    sysLogModel.OperationType = EOperationType.修改.GetHashCode();
                    sysLogModel.Remark = "重置用户密码";
                    if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, null))
                        rlt = 0;
                    #endregion
                    if (rlt == 1 && !dal.Update(ctx, model))
                        rlt = 0;
                    if (rlt == 1)
                        TranContext.Commit(ctx);
                    else
                        TranContext.Rollback(ctx);
                    return rlt == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    rlt = 0;
                    TranContext.Rollback(ctx);
                    this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
                }
            }
            return rlt == 1 ? true : false;
        }
        #endregion

        #region 账号是否重复
        /// <summary>
        /// 账号是否重复
        /// </summary>
        /// <param name="account"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool IsRepeatAccount(string account, int id)
        {
            int count = 0;
            if (id == 0)
                count = dal.GetCount(t =>  t.Account == account);
            else
                count = dal.GetCount(t => t.Name == account && t.ID != id);
            return count > 0;
        }
        #endregion

        #region 编码是否重复
        /// <summary>
        /// 编码是否重复
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool IsRepeatCode(string code, int id)
        {
            int count = 0;
            if (id == 0)
                count = dal.GetCount(t => t.Code == code);
            else
                count = dal.GetCount(t => t.Code == code && t.ID != id);
            return count > 0;
        }
        #endregion

        #region 登录
        private static object CheckPurviewLock = new object();
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <param name="userId"></param>
        /// <returns>
        /// 0登录失败
        /// 1登录成功
        /// 2账号不存在
        /// 3用户已禁用
        /// 4密码不正确
        /// 5当前IP不许登录
        /// 6当前用户没有任何权限
        /// </returns>
        public int Login(string systemCode, string account, string pwd, out int systemId, out int userId, out string userName, out List<RoleInfo> roleList)
        {
            int rlt = 1;
            systemId = 0;
            userId = 0;
            userName = string.Empty;
            roleList = new List<RoleInfo>();
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    UserInfo userInfo = this.BLLProvider.UserInfoBLL.GetModel(ctx, t => t.Account == account);
                    int sysId = this.BLLProvider.SystemInfoBLL.GetModel(t => t.Code == systemCode).ID;
                    string md5Pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5").Substring(0, 20);
                    if (userInfo == null)
                    {
                        this.BLLProvider.SysLoginLogBLL.Add(ctx, sysId, account, ELoginStatus.登录失败, "账号不存在！");
                        rlt = 2;
                    }
                    if (rlt == 1 && !userInfo.IsEnable)
                    {
                        this.BLLProvider.SysLoginLogBLL.Add(ctx, sysId, account, ELoginStatus.登录失败, "账号已禁用！");
                        rlt = 3;
                    }
                    if (rlt == 1 && md5Pwd != userInfo.Password)
                    {
                        this.BLLProvider.SysLoginLogBLL.Add(ctx, sysId, account, ELoginStatus.登录失败, "密码错误！");
                        rlt = 4;
                    }
                    #region IP黑名单比对
                    ///IP黑名单比对
                    string ip = IPHelper.GetClientIP();
                    List<IPBlacklist> ipList = this.BLLProvider.IPBlacklistBLL.GetList(t => t.IsEnable && t.Failuretime > DateTime.Now).Cast<IPBlacklist>().ToList();
                    List<string> ipRangList = new List<string>();
                    foreach (var item in ipList)
                    {
                        ipRangList.Add(item.StartIp + "-" + item.EndIp);
                    }
                    if (rlt == 1 && ipRangList.Count() > 0 && !IPHelper.TheIpIsRange(ip, ipRangList.ToArray()))
                    {
                        this.BLLProvider.SysLoginLogBLL.Add(ctx, sysId, account, ELoginStatus.登录失败, "IP不允许访问！");
                        rlt = 5;
                    }
                    #endregion

                    #region 获取设置 权限缓存
                    //没有任何权限
                    if (rlt == 1 && userInfo.Account != Consts.ConstValue.SuperAdminAccount && GetUserPurview(sysId, 0, userInfo.ID).Rows.Count <= 0)
                    {
                        this.BLLProvider.SysLoginLogBLL.Add(ctx, sysId, account, ELoginStatus.登录失败, "当前用户没有任何权限！");
                        rlt = 6;
                    }
                    #endregion
                    if (rlt == 1)
                    {
                        systemId = sysId;
                        userId = userInfo.ID;
                        userName = userInfo.Name;
                        int[] userRoleIDList = this.BLLProvider.UserRoleBLL.GetList(t => t.RoleId, t => t.UserId == userInfo.ID).Cast<int>().ToArray();
                        roleList = this.BLLProvider.RoleInfoBLL.GetList(t => t.SystemId == sysId && t.IsEnable && userRoleIDList.Contains(t.ID)).Cast<RoleInfo>().ToList();

                        if (userInfo.FirstVisit == null)
                        {
                            userInfo.FirstVisit = DateTime.Now;
                            userInfo.PreviousVisit = DateTime.Now;
                            userInfo.LastVisit = DateTime.Now;
                            userInfo.IPAddress = ip;
                            userInfo.MACAddress = IPHelper.GetMacBySendARP(ip);
                            userInfo.LogOnCount = 1;
                        }
                        else
                        {
                            userInfo.PreviousVisit = userInfo.LastVisit;
                            userInfo.LastVisit = DateTime.Now;
                            userInfo.LogOnCount += 1;
                        }
                    }

                    ///读取缓存信息权限&菜单 根据系统ID&用户Account组成KEY
                    ///过滤当前角色所用的信息
                    ///更新用户表登录相关信息
                    ///

                    if (rlt == 1 && !this.BLLProvider.UserInfoBLL.Update(ctx, userInfo))
                        rlt = 0;
                    if (rlt == 1 && !this.BLLProvider.SysLoginLogBLL.Add(ctx, sysId, account, ELoginStatus.登录成功, ""))
                        rlt = 0;
                    if (rlt == 1)
                        TranContext.Commit(ctx);
                    else
                        TranContext.Rollback(ctx);
                }
                catch (Exception ex)
                {
                    rlt = 0;
                    TranContext.Rollback(ctx);
                    this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
                }
            }
            return rlt;
        }
        #endregion

        #region 系统退出
        /// <summary>
        /// 系统退出
        /// </summary>
        /// <param name="account"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 2账号不存在
        /// </returns>
        public int LogOut(int systemId, int userId)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                UserInfo userInfo = dal.GetModel(ctx, t => t.ID == userId);
                if (userInfo == null)
                    rlt = 2;
                if (rlt == 1 && !this.BLLProvider.SysLoginLogBLL.Add(ctx, systemId, userInfo.Account, ELoginStatus.成功退出, ""))
                {
                    rlt = 0;
                }
            }
            return rlt;
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns>
        /// 0 操作失败，请联系管理员
        /// 1 操作成功
        /// 2 账号不存在
        /// 4 密码错误！
        /// </returns>
        public int ChangePassword(int userId, string oldPwd, string newPwd)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {

                    UserInfo userInfo = dal.GetModel(ctx, t => t.ID == userId);
                    if (userInfo == null)
                        rlt = 2;
                    if (rlt == 1)
                    {
                        string oldmd5Pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(oldPwd, "MD5").Substring(0, 20);
                        if (oldmd5Pwd != userInfo.Password)
                            rlt = 4;
                    }
                    if (rlt == 1)
                    {
                        string newmd5Pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(newPwd, "MD5").Substring(0, 20);
                        userInfo.Password = newmd5Pwd;
                        userInfo.ChangePasswordDate = DateTime.Now;

                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.BusinessName = "修改密码";
                        sysLogModel.CreateUserId = userId;
                        sysLogModel.CreateUserName = userInfo.Name;
                        sysLogModel.ObjectId = userId;
                        sysLogModel.OperationType = EOperationType.修改.GetHashCode();
                        sysLogModel.TableName = "UserInfo";
                        var entry = ctx.Entry(userInfo);
                        if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, entry))
                            rlt = 0;
                        #endregion
                        if (rlt == 1 && !dal.Update(ctx, userInfo))
                            rlt = 0;
                    }
                    if (rlt == 1)
                        TranContext.Commit(ctx);
                    else
                        TranContext.Rollback(ctx);
                }
                catch (Exception ex)
                {
                    rlt = 0;
                    TranContext.Rollback(ctx);
                    this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
                }
            }
            return rlt;
        }
        #endregion

        #region 用户菜单
        /// <summary>
        /// 用户菜单
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetUserMenu(int systemId, int roleId, int userId)
        {
            UserInfo userInfo = dal.GetModel(t => t.ID == userId);
            RoleInfo roleInfo = this.BLLProvider.RoleInfoBLL.GetModel(roleId);
            string menuCacheKey = Consts.CacheKey.BaseService_UserMenus + "_" + roleInfo.Code + "_" + userInfo.Account;
            if (CacheHelper.Get(menuCacheKey) == null)
            {
                lock (CheckPurviewLock)
                {
                    if (CacheHelper.Get(menuCacheKey) == null)
                    {
                        DataTable menuDt = this.BLLProvider.MenuInfoBLL.GetUserMenu(systemId, roleId, userId);
                        CacheHelper.Add(menuCacheKey, menuDt, new TimeSpan(1, 0, 0));
                    }
                }
            }
            return (CacheHelper.Get(menuCacheKey) as DataTable);
        }
        #endregion

        #region 获取用户权限
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetUserPurview(int systemId, int roleId, int userId)
        {
            UserInfo userInfo = dal.GetModel(t => t.ID == userId);
            RoleInfo roleInfo = this.BLLProvider.RoleInfoBLL.GetModel(roleId);
            string purviewCacheKey = Consts.CacheKey.BaseService_Purview + "_" + systemId + "_" + roleInfo.Code + "_" + userInfo.Account;
            if (CacheHelper.Get(purviewCacheKey) == null)
            {
                lock (CheckPurviewLock)
                {
                    if (CacheHelper.Get(purviewCacheKey) == null)
                    {
                        CacheHelper.Add(purviewCacheKey, this.BLLProvider.UserPurviewBLL.GetUserPurview(systemId, roleId, userId), new TimeSpan(1, 0, 0));
                    }
                }
            }
            return (CacheHelper.Get(purviewCacheKey) as DataTable);
        }
        #endregion

    }
}
