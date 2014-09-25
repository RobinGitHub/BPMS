using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.DAL;
using BPMS.Model;
using BPMS.Common;
using System.Data;
using System.Linq.Expressions;
using System.Collections;

namespace BPMS.BLL 
{ 
    /// <summary> 
    /// 
    /// </summary>  
    public partial class SysLoginLogBll : BaseTableBLL<SysLoginLog, SysLoginLogDal>
    { 
        #region 构造函数
        public SysLoginLogBll() { }

        public SysLoginLogBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName, DateTime startDate, DateTime endDate, int systemId, string account, int pageIndex, int pageSize, out int count)
        {
            count = 0;
            try
            {
                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "SysLoginLog";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion
                return dal.GetList(startDate, endDate, systemId, account, pageIndex, pageSize, out count);
            }
            catch (Exception ex)
            {
                this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
            }
            return null;
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal bool Add(BPMSEntities ctx, int systemId, string account, ELoginStatus loginStatus, string remark = "")
        {
            int rlt = 1;
            SysLoginLog model = new SysLoginLog();
            model.ID = dal.GetNewID();
            model.Account = account;
            model.SystemId = systemId;
            model.CreateDate = DateTime.Now;
            model.Status = loginStatus.GetHashCode();
            var location = IPHelper.GetLocation();
            model.IPAddress = location.GetIPAddress.ToString();
            model.IPAddressName = location.ToString();
            model.Remark = remark;
            if (!dal.Insert(ctx, model))
                rlt = 0;
            return rlt == 1;
        }
        #endregion
        
        #region 删除
        /// <summary>
        /// 删除 将在此时间之前的日志删除
        /// </summary>
        /// <param name="endDate">截止日期</param>
        /// <returns></returns>
        public int Delete(int logUserId, string logUserName, int systemId, DateTime endDate)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    #region 系统日志
                    SysLog sysLogModel = new SysLog();
                    sysLogModel.TableName = "SysLoginLog";
                    sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                    sysLogModel.CreateUserId = logUserId;
                    sysLogModel.CreateUserName = logUserName;
                    sysLogModel.OperationType = EOperationType.删除.GetHashCode();
                    this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, null);
                    #endregion
                    if (!dal.Delete(ctx, t => t.SystemId == systemId && t.CreateDate <= endDate))
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
    }
} 
