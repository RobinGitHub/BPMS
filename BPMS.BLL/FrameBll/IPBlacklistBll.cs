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
    public partial class IPBlacklistBll : BaseTableBLL<IPBlacklist, IPBlacklistDal>
    { 
        #region 构造函数
        public IPBlacklistBll() { }

        public IPBlacklistBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName)
        {
            try
            {
                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "IPBlacklist";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion

                var list = dal.GetList(t => true).Cast<IPBlacklist>().ToList();

                DataTable rltDt = ConvertHelper.ToDataTable(list);
                DataColumn col = new DataColumn("CategoryName");
                rltDt.Columns.Add(col);
                foreach (DataRow item in rltDt.Rows)
                {
                    item["CategoryName"] = (EIPBlacklistCategory)int.Parse(item["Category"].ToString());
                }
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
        public IPBlacklist GetModel(int id)
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
        /// 1 成功
        /// 0 失败
        /// </returns>
        public int Add(IPBlacklist model)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    model.ID = this.GetNewID();
                    model.Category = EIPBlacklistCategory.登录.GetHashCode();
                    #region 系统日志
                    SysLog sysLogModel = new SysLog();
                    sysLogModel.TableName = "IPBlacklist";
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
        /// </returns>
        public int Edit(IPBlacklist model)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    var oldModel = dal.GetModel(ctx, t => t.ID == model.ID);
                    if (oldModel == null)
                        rlt = 11;
                    if (rlt == 1)
                    {
                        oldModel.StartIp = model.StartIp;
                        oldModel.EndIp = model.EndIp;
                        oldModel.Failuretime = model.Failuretime;
                        oldModel.Remark = model.Remark;
                        oldModel.IsEnable = model.IsEnable;
                        oldModel.SortIndex = model.SortIndex;
                        oldModel.ModifyDate = DateTime.Now;
                        oldModel.ModifyUserId = model.ModifyUserId;
                        oldModel.ModifyUserName = model.ModifyUserName;

                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "IPBlacklist";
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

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idList"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 11当前对象已不存在
        /// </returns>
        public int Delete(int logUserId, string logUserName, int id)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    var model = dal.GetModel(ctx, t => t.ID == id);
                    if (model == null)
                        rlt = 11;
                    if (rlt == 1)
                    {
                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "IPBlacklist";
                        sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                        sysLogModel.CreateUserId = logUserId;
                        sysLogModel.CreateUserName = logUserName;
                        sysLogModel.ObjectId = id;
                        sysLogModel.OperationType = EOperationType.删除.GetHashCode();

                        var entry = ctx.Entry(model);
                        if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, entry))
                            rlt = 0;
                        #endregion

                        if (!dal.Delete(ctx, model))
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
    }
} 
