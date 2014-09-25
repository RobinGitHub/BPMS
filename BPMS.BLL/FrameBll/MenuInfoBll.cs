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
    public partial class MenuInfoBll : BaseTableBLL<MenuInfo, MenuInfoDal>
    {
        #region 构造函数
        public MenuInfoBll() { }

        public MenuInfoBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName, int systemId, int parentId)
        {
            try
            { 
                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "MenuInfo";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion

                return dal.GetList(systemId, parentId);
            }
            catch (Exception ex)
            {
                this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
            }
            return null;
        }
        #endregion

        #region 获取用户有权限的菜单
        /// <summary>
        /// 获取用户有权限的菜单
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserMenu(int systemId, int roleId, int userId)
        {
            DataTable rltDt = new DataTable();
            using (var ctx = new BPMSEntities())
            {
                rltDt = ConvertHelper.ToDataTable(ctx.SP_GetUserMenu(systemId, userId).ToList());
                if (roleId != 0)
                {
                    rltDt.DefaultView.RowFilter = "roleId=" + roleId + " ";
                    rltDt = rltDt.DefaultView.ToTable();
                }
            }
            return rltDt;
        }
        #endregion

        #region 获取对象
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuInfo GetModel(int id)
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
        /// 12 名称重复
        /// 13 编码重复
        /// </returns>
        public int Add(MenuInfo model)
        {
            int rlt = 1;
            if (IsRepeatName(model.SystemId, model.Name, model.ID))
                rlt = 12;
            if (rlt == 1 && IsRepeatCode(model.SystemId, model.Code, model.ID))
                rlt = 13;
            if (rlt == 1)
            {
                using (var ctx = TranContext.BeginTran())
                {
                    try
                    {
                        model.ID = this.GetNewID();
                        model.CreateDate = DateTime.Now;
                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "MenuInfo";
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
        /// 12名称重复
        /// 13编码重复
        /// </returns>
        public int Edit(MenuInfo model)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    var oldModel = dal.GetModel(ctx, t => t.ID == model.ID);
                    if (oldModel == null)
                        rlt = 11;
                    if (rlt == 1 && IsRepeatName(model.SystemId, model.Name, model.ID))
                        rlt = 12;
                    if (rlt == 1 && IsRepeatCode(model.SystemId, model.Code, model.ID))
                        rlt = 13;
                    if (rlt == 1)
                    {
                        oldModel.Name = model.Name;
                        oldModel.Code = model.Code;
                        oldModel.ParentId = model.ParentId;
                        oldModel.Category = model.Category;
                        oldModel.PurviewId = model.PurviewId;
                        oldModel.Icon = model.Icon;
                        oldModel.IconUrl = model.IconUrl;
                        oldModel.NavigateUrl = model.NavigateUrl;
                        oldModel.FormName = model.FormName;
                        oldModel.IsSplit = model.IsSplit;
                        oldModel.Remark = model.Remark;
                        oldModel.IsEnable = model.IsEnable;
                        oldModel.SortIndex = model.SortIndex;
                        oldModel.ModifyDate = DateTime.Now;
                        oldModel.ModifyUserId = model.ModifyUserId;
                        oldModel.ModifyUserName = model.ModifyUserName;

                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "MenuInfo";
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
        /// 16当前数据有子集，不允许删除
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
                    if (rlt == 1 && this.BLLProvider.MenuInfoBLL.GetCount(t => t.ParentId == id) > 0)
                        rlt = 16;
                    if (rlt == 1)
                    {
                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "MenuInfo";
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

        #region 名称是否重复
        /// <summary>
        /// 名称是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool IsRepeatName(int systemId, string name, int id)
        {
            int count = 0;
            if (id == 0)
                count = dal.GetCount(t => t.SystemId == systemId && t.Name == name);
            else
                count = dal.GetCount(t => t.SystemId == systemId && t.Name == name && t.ID != id);
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
        public bool IsRepeatCode(int systemId, string code, int id)
        {
            int count = 0;
            if (id == 0)
                count = dal.GetCount(t => t.SystemId == systemId && t.Code == code);
            else
                count = dal.GetCount(t => t.SystemId == systemId && t.Code == code && t.ID != id);
            return count > 0;
        }
        #endregion
    }
}
