using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.DAL;
using BPMS.Model;
using BPMS.Common;
using System.Data;

namespace BPMS.BLL
{
    /// <summary> 
    /// 
    /// </summary>  
    public partial class RolePurviewBll : BaseTableBLL<RolePurview, RolePurviewDal>
    {
        #region 构造函数
        public RolePurviewBll() { }

        public RolePurviewBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 设置权限
        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="purviewIdList"></param>
        /// <returns>
        /// 0 失败
        /// 1 成功
        /// </returns>
        public int PurviewSet(int logUserId, string logUserName, int roleId, List<int> purviewIdList)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    if (!dal.Delete(ctx, t => t.RoleId == roleId))
                        rlt = 0;
                    if (rlt == 1)
                    {
                        List<RolePurview> list = new List<RolePurview>();
                        foreach (var item in purviewIdList)
                        {
                            RolePurview model = new RolePurview();
                            model.ID = GetNewID();
                            model.RoleId = roleId;
                            model.PurviewId = item;
                            list.Add(model);
                        }
                        if (!dal.Insert(ctx, list))
                            rlt = 0;
                    }
                    if (rlt == 1)
                    {
                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "RolePurview";
                        sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                        sysLogModel.CreateUserId = logUserId;
                        sysLogModel.CreateUserName = logUserName;
                        sysLogModel.OperationType = EOperationType.修改.GetHashCode();
                        if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, null))
                            rlt = 0;
                        #endregion
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

        #region 角色权限
        /// <summary>
        /// 角色权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetPurviewList(int logUserId, string logUserName, int systemId, int roleId)
        {
            try
            {
                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "RolePurview";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion

                return dal.GetPurviewList(systemId, roleId);
            }
            catch (Exception ex)
            {
                this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
            }
            return null;
        }
        #endregion

        #region 角色成员
        /// <summary>
        /// 角色成员
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <returns>用户列表</returns>
        public DataTable RoleMembers(int roleId)
        {
            try
            {
                return dal.RoleMembers(roleId);
            }
            catch (Exception ex)
            {
                this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
            }
            return null;
        }
        #endregion

        #region 角色成员添加
        /// <summary>
        /// 角色成员添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <param name="userIdList"></param>
        /// <returns>
        /// 0 成功
        /// 1 失败
        /// </returns>
        public int Add(int logUserId, string logUserName, int roleId, List<int> userIdList)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {

                    List<UserRole> list = new List<UserRole>();
                    foreach (var item in userIdList)
                    {
                        UserRole model = new UserRole();
                        model.ID = dal.GetNewID("UserRole");
                        model.UserId = item;
                        model.RoleId = roleId;
                        model.CreateDate = DateTime.Now;
                        model.CreateUserId = logUserId;
                        model.CreateUserName = logUserName;
                        list.Add(model);

                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "UserRole";
                        sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                        sysLogModel.CreateUserId = logUserId;
                        sysLogModel.CreateUserName = logUserName;
                        sysLogModel.OperationType = EOperationType.新增.GetHashCode();
                        var entry = ctx.Entry(model);
                        if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, entry))
                            rlt = 0;
                        #endregion
                    }

                    if (rlt == 1 && !this.BLLProvider.UserRoleBLL.Insert(ctx, list))
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

        #region 角色成员删除
        /// <summary>
        /// 角色成员删除
        /// </summary>
        /// <returns>
        /// 0
        /// 1
        /// 15 该成员不能删除，因为该成员只有一个角色
        /// </returns>
        public int Delete(int logUserId, string logUserName, int roleId, int userId)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    int count = this.BLLProvider.UserRoleBLL.GetCount(t => t.UserId == userId);
                    if (count == 1)
                        rlt = 15;
                    var model = this.BLLProvider.UserRoleBLL.GetModel(ctx, t => t.UserId == userId && t.RoleId == roleId);
                    if (rlt == 1 && !this.BLLProvider.UserRoleBLL.Delete(ctx, model))
                        rlt = 0;
                    if (rlt == 1 && !this.BLLProvider.UserPurviewBLL.Delete(ctx, t => t.RoleId == roleId && t.UserId == userId))
                        rlt = 0;

                    #region 系统日志
                    SysLog sysLogModel = new SysLog();
                    sysLogModel.TableName = "UserRole";
                    sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                    sysLogModel.CreateUserId = logUserId;
                    sysLogModel.CreateUserName = logUserName;
                    sysLogModel.OperationType = EOperationType.删除.GetHashCode();
                    var entry = ctx.Entry(model);
                    if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, entry))
                        rlt = 0;
                    #endregion
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
    }
}
