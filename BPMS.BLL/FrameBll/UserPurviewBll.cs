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
    public partial class UserPurviewBll : BaseTableBLL<UserPurview, UserPurviewDal>
    { 
        #region 构造函数
        public UserPurviewBll() { }

        public UserPurviewBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 列表
        /// <summary>
        /// 用户权限列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName, int systemId, int roleId, int userId)
        {
            try
            {

                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "UserPurview";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion
                return GetUserPurview(systemId, roleId, userId);
            }
            catch (Exception ex)
            {
                this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
            }
            return null;
        }
        #endregion

        #region 获取用户权限
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserPurview(int systemId, int roleId, int userId)
        {
            DataTable rltDt = new DataTable();
            using (var ctx = new BPMSEntities())
            {
                rltDt = ConvertHelper.ToDataTable(ctx.SP_GetUserPurview(systemId, userId).ToList());
                if (roleId != 0)
                {
                    rltDt.DefaultView.RowFilter = "roleId=" + roleId + " ";
                    rltDt = rltDt.DefaultView.ToTable();
                }
            }
            return rltDt;
        }
        #endregion

        #region 设置用户权限
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <returns>
        /// 0 失败
        /// 1 成功
        /// </returns>
        public int UserPurviewSet(int logUserId, string logUserName, List<UserPurview> modelList)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    #region 系统日志
                    SysLog sysLogModel = new SysLog();
                    sysLogModel.TableName = "UserPurview";
                    sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                    sysLogModel.CreateUserId = logUserId;
                    sysLogModel.CreateUserName = logUserName;
                    sysLogModel.OperationType = EOperationType.修改.GetHashCode();
                    sysLogModel.Remark = "分配用户权限";
                    if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, null))
                        rlt = 0;
                    #endregion

                    if (rlt == 1 && !dal.Delete(ctx, t => t.UserId == modelList.First().UserId && t.RoleId == modelList.First().RoleId))
                        rlt = 0;
                    if (rlt == 1 && !dal.Insert(ctx, modelList))
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

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idList"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// </returns>
        public int Delete(int logUserId, string logUserName, int systemId, int roleId, int userId)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    if (rlt == 1)
                    {
                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "UserPurview";
                        sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                        sysLogModel.CreateUserId = logUserId;
                        sysLogModel.CreateUserName = logUserName;
                        sysLogModel.OperationType = EOperationType.删除.GetHashCode();
                        sysLogModel.Remark = "重置用户权限";
                        if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, null))
                            rlt = 0;
                        #endregion

                        dal.Delete(ctx, systemId, roleId, userId);
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


    }
} 
