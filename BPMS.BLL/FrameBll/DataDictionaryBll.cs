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
    public partial class DataDictionaryBll : BaseTableBLL<DataDictionary, DataDictionaryDal>
    {
        #region 构造函数
        public DataDictionaryBll() { }

        public DataDictionaryBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName, int systemId, int dictType)
        {
            try
            {
                var list = dal.GetList(t => t.SystemId == systemId && t.DictType == dictType).Cast<DataDictionary>()
                                .Select(t => new
                            {
                                t.ID,
                                t.DictType,
                                t.Name,
                                t.Code,
                                t.ParentId,
                                t.IsEnable,
                                t.Remark,
                                t.SortIndex,
                                t.AllowDelete,
                                t.AllowEdit
                            });

                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "DataDictionary";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion

                return ConvertHelper.ToDataTable(list.Cast<object>().ToArray());
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
        public DataDictionary GetModel(int id)
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
        public int Add(DataDictionary model)
        {
            int rlt = 1;
            if (IsRepeatName(model.DictType, model.Name, model.ID))
                rlt = 12;
            if (rlt == 1 && IsRepeatCode(model.DictType, model.Code, model.ID))
                rlt = 13;
            if (rlt == 1)
            {
                using (var ctx = TranContext.BeginTran())
                {
                    try
                    {
                        model.ID = this.GetNewID();
                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "DataDictionary";
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
        public int Edit(DataDictionary model)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    var oldModel = dal.GetModel(ctx, t => t.ID == model.ID);
                    if (oldModel == null)
                        rlt = 11;
                    if (rlt == 1 && IsRepeatName(model.DictType, model.Name, model.ID))
                        rlt = 12;
                    if (rlt == 1 && IsRepeatCode(model.DictType, model.Code, model.ID))
                        rlt = 13;
                    if (rlt == 1)
                    {
                        oldModel.Name = model.Name;
                        oldModel.Code = model.Code;
                        oldModel.ParentId = model.ParentId;
                        oldModel.Remark = model.Remark;
                        oldModel.IsEnable = model.IsEnable;
                        oldModel.SortIndex = model.SortIndex;
                        oldModel.ModifyDate = DateTime.Now;
                        oldModel.ModifyUserId = model.ModifyUserId;
                        oldModel.ModifyUserName = model.ModifyUserName;

                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "DataDictionary";
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
        /// 14当前数据已经使用，不允许删除
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
                        #region 判断当前数据是否使用
                        int count = 0;
                        switch ((EDictType)model.DictType)
                        {
                            case EDictType.国籍:
                                count = this.BLLProvider.EmployeeBLL.GetCount(t => t.NationId == id);
                                break;
                            case EDictType.角色分类:
                                count = this.BLLProvider.RoleInfoBLL.GetCount(t => t.Category == id);
                                break;
                            case EDictType.民族:
                                count = this.BLLProvider.EmployeeBLL.GetCount(t => t.NationalityId == id);
                                break;
                            case EDictType.学历:
                                count = this.BLLProvider.EmployeeBLL.GetCount(t => t.EducationId == id);
                                break;
                            case EDictType.学位:
                                count = this.BLLProvider.EmployeeBLL.GetCount(t => t.DegreeId == id);
                                break;
                            case EDictType.用工性质:
                                count = this.BLLProvider.EmployeeBLL.GetCount(t => t.WorkingPropertyId == id);
                                break;
                            case EDictType.政治面貌:
                                count = this.BLLProvider.EmployeeBLL.GetCount(t => t.PartyId == id);
                                break;
                            case EDictType.职称:
                                count = this.BLLProvider.EmployeeBLL.GetCount(t => t.TitleId == id);
                                break;
                            case EDictType.职称等级:
                                count = this.BLLProvider.EmployeeBLL.GetCount(t => t.TitleLevelId == id);
                                break;
                            case EDictType.职位:
                                count = this.BLLProvider.EmployeeBLL.GetCount(t => t.DutyId == id);
                                break;
                        }
                        if (count > 0)
                            rlt = 14;
                        #endregion
                    }
                    if (rlt == 1)
                    {
                        #region 系统日志
                        SysLog sysLogModel = new SysLog();
                        sysLogModel.TableName = "DataDictionary";
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
        public bool IsRepeatName(int dictType, string name, int id)
        {
            int count = 0;
            if (id == 0)
                count = dal.GetCount(t => t.DictType == dictType && t.Name == name);
            else
                count = dal.GetCount(t => t.DictType == dictType && t.Name == name && t.ID != id);
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
        public bool IsRepeatCode(int dictType, string code, int id)
        {
            int count = 0;
            if (id == 0)
                count = dal.GetCount(t => t.DictType == dictType && t.Code == code);
            else
                count = dal.GetCount(t => t.DictType == dictType && t.Code == code && t.ID != id);
            return count > 0;
        }
        #endregion

        #region 数据查询(缓存)..
        private static object _lock = new object();
        /// <summary>
        /// 字典表结合缓存对象
        /// </summary>
        public List<DataDictionary> AllDictionary
        {
            get
            {
                var obj = CacheHelper.Get(Consts.CacheKey.DICTIONARY);
                if (obj == null)
                {
                    lock (_lock)
                    {
                        obj = CacheHelper.Get(Consts.CacheKey.DICTIONARY);
                        if (obj == null)
                        {
                            obj = base.GetList(t => true).Cast<DataDictionary>().ToList();
                            CacheHelper.Add(Consts.CacheKey.DICTIONARY, obj, new TimeSpan(1, 0, 0));
                        }
                    }
                }
                return obj as List<DataDictionary>;
            }
        }
        #endregion
    }
}
