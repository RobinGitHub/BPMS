using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.DAL;
using BPMS.Model;
using BPMS.Common;
using System.Data;
using System.Linq.Expressions;

namespace BPMS.BLL
{
    /// <summary> 
    /// 
    /// </summary>  
    public partial class SystemExceptionLogBll : BaseTableBLL<SystemExceptionLog, SystemExceptionLogDal>
    {
        #region 构造函数
        public SystemExceptionLogBll() { }

        public SystemExceptionLogBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, out int count)
        {
            count = 0;
            try
            {
                DateTime tmpStartDate = ConvertHelper.ObjectToDateTime(startDate.ToShortDateString() + " 00:00:00");
                DateTime tmpEndDate = ConvertHelper.ObjectToDateTime(endDate.AddDays(1).ToShortDateString() + " 00:00:00");
                Expression<Func<SystemExceptionLog, bool>> condition = t => t.CreateDate >= tmpStartDate && t.CreateDate <= endDate;
                List<SystemExceptionLog> list = dal.GetList(pageIndex, pageSize, out count, condition, t => t.CreateDate, false).Cast<SystemExceptionLog>().ToList();

                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "SystemExceptionLog";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion
                return ConvertHelper.ToDataTable<SystemExceptionLog>(list);
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
        internal int Add(string source, string exception, string desction)
        {
            return dal.Insert(source, exception, desction);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public int Delete(int logUserId, string logUserName)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    #region 系统日志
                    SysLog sysLogModel = new SysLog();
                    sysLogModel.TableName = "SystemExceptionLog";
                    sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                    sysLogModel.CreateUserId = logUserId;
                    sysLogModel.CreateUserName = logUserName;
                    sysLogModel.OperationType = EOperationType.删除.GetHashCode();
                    this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, null);
                    #endregion
                    if (!dal.Delete(ctx, t => true))
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
