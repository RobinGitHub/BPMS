using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.DAL;
using BPMS.Model;
using BPMS.Common;
using System.Data;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;

namespace BPMS.BLL
{
    /// <summary> 
    /// 
    /// </summary>  
    public partial class SysLogBll : BaseTableBLL<SysLog, SysLogDal>
    {
        #region 构造函数
        public SysLogBll() { }

        public SysLogBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName, DateTime startDate, DateTime endDate, EOperationType operationType, string moduleName, string account, int pageIndex, int pageSize, out int count)
        {
            count = 0;
            DateTime tmpStartDate = ConvertHelper.ObjectToDateTime(startDate.ToShortDateString() + " 00:00:00");
            DateTime tmpEndDate = ConvertHelper.ObjectToDateTime(endDate.AddDays(1).ToShortDateString() + " 00:00:00");
            Expression<Func<SysLog, bool>> condition = t => t.CreateDate >= tmpStartDate && t.CreateDate <= endDate;
            if (!string.IsNullOrEmpty(moduleName))
                condition.And(t => t.BusinessName.Contains(moduleName));
            if (operationType != EOperationType.全部)
                condition.And(t => t.OperationType == operationType.GetHashCode());
            if (!string.IsNullOrEmpty(account))
                condition.And(t => t.CreateUserName.Contains(account));
            try
            {
                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "SysLog";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion

                var list = dal.GetList(pageIndex, pageSize, out count, condition, t => t.CreateDate, false).Cast<SysLog>().ToList();
                return ConvertHelper.ToDataTable<SysLog>(list);
            }
            catch (Exception ex)
            {
                this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
            }
            return null;
        }
        #endregion

        #region 获取明细
        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetDetailList(int id)
        {
            SysLogDetailsDal detailDal = new SysLogDetailsDal();
            List<SysLogDetails> list = detailDal.GetList(t => t.SyslogId == id).Cast<SysLogDetails>().ToList();
            return ConvertHelper.ToDataTable(list);
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="model"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        internal bool Add(BPMSEntities ctx, SysLog model, DbEntityEntry entry)
        {
            /// 资源
            /// http://www.cnblogs.com/oppoic/p/ef_dbpropertyvalues_toobject_clone_setvalues_changetracker_entries.html
            List<SysLogDetails> logDetailList = new List<SysLogDetails>();
            bool rlt = true;
            model.ID = GetNewID();
            var location = IPHelper.GetLocation();
            model.IPAddress = location.GetIPAddress.ToString();
            model.IPAddressName = location.ToString();
            model.CreateDate = DateTime.Now;
            model.CreateUserId = model.CreateUserId;
            model.CreateUserName = model.CreateUserName;

            if (!Insert(ctx, model))
                rlt = false;
            if (rlt == true && entry != null)
            {
                IEnumerable<string> propertyNames = null;
                if (model.OperationType == EOperationType.新增.GetHashCode() || model.OperationType == EOperationType.删除.GetHashCode())
                {
                    propertyNames = entry.CurrentValues.PropertyNames;
                }
                else if (model.OperationType == EOperationType.修改.GetHashCode())
                {
                    propertyNames = entry.CurrentValues.PropertyNames.Where(t => entry.Property(t).IsModified);
                }
                foreach (var propertyName in propertyNames)
                {
                    SysLogDetails logDetail = new SysLogDetails();
                    logDetail.ID = GetNewID("SysLogDetails");
                    logDetail.SyslogId = model.ID;
                    logDetail.FieldName = propertyName;
                    logDetail.FieldText = DatabasePDMHelper.GetColumnName(model.TableName, propertyName);
                    logDetail.NewValue = entry.Property(propertyName).CurrentValue.ToString();
                    logDetail.OldValue = entry.Property(propertyName).OriginalValue.ToString();
                    logDetailList.Add(logDetail);
                }
                SysLogDetailsDal detailDal = new SysLogDetailsDal();
                if (!detailDal.Insert(ctx, logDetailList))
                    rlt = false;
            }
            return rlt;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public int Delete(int logUserId, string logUserName, DateTime endDate)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    #region 系统日志
                    SysLog sysLogModel = new SysLog();
                    sysLogModel.TableName = "SysLog";
                    sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                    sysLogModel.CreateUserId = logUserId;
                    sysLogModel.CreateUserName = logUserName;
                    sysLogModel.OperationType = EOperationType.删除.GetHashCode();
                    this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, null);
                    #endregion
                    if (!dal.Delete(t => t.CreateDate <= endDate))
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
