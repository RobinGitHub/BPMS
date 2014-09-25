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
    public partial class EmployeeBll : BaseTableBLL<Employee, EmployeeDal>
    { 
        #region 构造函数
        public EmployeeBll() { }

        public EmployeeBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="category"></param>
        /// <param name="searchType">1=工号、2=姓名</param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName, EOrgaCategory category, int orgaId, int searchType, string keyWord, int pageIndex, int pageSize, out int count)
        {
            count = 0;
            try
            {
                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "Employee";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion

                Expression<Func<Employee, bool>> condition = t => true;
                if (category == EOrgaCategory.集团)
                    condition.And(t => t.SubCompanyId == orgaId);
                else if (category == EOrgaCategory.公司)
                    condition.And(t => t.CompanyId == orgaId);
                else if (category == EOrgaCategory.部门)
                    condition.And(t => t.DepartmentId == orgaId);
                else if (category == EOrgaCategory.工作组)
                    condition.And(t => t.WorkgroupId == orgaId);

                if (searchType == 1)
                    condition.And(t => t.Code == keyWord);
                if (searchType == 2)
                    condition.And(t => t.Name == keyWord);


                List<Employee> list = dal.GetList(pageIndex, pageSize, out count, condition, t => t.Code, true).Cast<Employee>().ToList();
                DataTable rltDt = ConvertHelper.ToDataTable(list);
                rltDt.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("DutyName"),
                    new DataColumn("EducationName"),
                    new DataColumn("DegreeName"),
                    new DataColumn("TitleName"),
                    new DataColumn("TitleLevelName"),
                    new DataColumn("PartyName"),
                    new DataColumn("NationName"),
                    new DataColumn("NationalityName"),
                    new DataColumn("WorkingPropertyName"),
                    new DataColumn("CompetencyName"),
                });

                var dict = this.BLLProvider.DataDictionaryBLL.AllDictionary;
                foreach (DataRow item in rltDt.Rows)
                {
                    item["DutyName"] = dict.Find(t => t.ID == int.Parse(item["DutyId"].ToString())).Name;
                    item["EducationName"] = dict.Find(t => t.ID == int.Parse(item["Education"].ToString())).Name;
                    item["DegreeName"] = dict.Find(t => t.ID == int.Parse(item["Degree"].ToString())).Name;
                    item["TitleName"] = dict.Find(t => t.ID == int.Parse(item["TitleId"].ToString())).Name;
                    item["TitleLevelName"] = dict.Find(t => t.ID == int.Parse(item["DutyId"].ToString())).Name;
                    item["PartyName"] = dict.Find(t => t.ID == int.Parse(item["DutyId"].ToString())).Name;
                    item["NationName"] = dict.Find(t => t.ID == int.Parse(item["DutyId"].ToString())).Name;
                    item["NationalityName"] = dict.Find(t => t.ID == int.Parse(item["DutyId"].ToString())).Name;
                    item["WorkingPropertyName"] = dict.Find(t => t.ID == int.Parse(item["DutyId"].ToString())).Name;
                    item["CompetencyName"] = dict.Find(t => t.ID == int.Parse(item["DutyId"].ToString())).Name;
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
        public Employee GetModel(int id)
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
        /// 13 编码重复
        /// </returns>
        public int Add(Employee model)
        {
            int rlt = 1;
            if (IsRepeatCode(model.Code, model.ID))
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
                        sysLogModel.TableName = "Employee";
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
        /// 13编码重复</returns>
        public int Edit(Employee model)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    var oldModel = dal.GetModel(ctx, t => t.ID == model.ID);
                    if (rlt == 1 && IsRepeatCode(model.Code, model.ID))
                        rlt = 13;
                    if (rlt == 1)
                    {
                        #region RegionName
                        oldModel.Name = model.Name;
                        oldModel.Code = model.Code;
                        oldModel.Spell = model.Spell;
                        oldModel.Alias = model.Alias;
                        oldModel.Age = model.Age;
                        oldModel.Birthday = model.Birthday;
                        oldModel.Gender = model.Gender;
                        oldModel.SubCompanyId = model.SubCompanyId;
                        oldModel.CompanyId = model.CompanyId;
                        oldModel.DepartmentId = model.DepartmentId;
                        oldModel.WorkgroupId = model.WorkgroupId;
                        oldModel.DutyId = model.DutyId;
                        oldModel.IDCard = model.IDCard;
                        oldModel.BankCode = model.BankCode;
                        oldModel.Email = model.Email;
                        oldModel.Mobile = model.Mobile;
                        oldModel.ShortNumber = model.ShortNumber;
                        oldModel.Telephone = model.Telephone;
                        oldModel.OICQ = model.OICQ;
                        oldModel.OfficePhone = model.OfficePhone;
                        oldModel.OfficeZipCode = model.OfficeZipCode;
                        oldModel.OfficeAddress = model.OfficeAddress;
                        oldModel.OfficeFax = model.OfficeFax;
                        oldModel.EducationId = model.EducationId;
                        oldModel.School = model.School;
                        oldModel.GraduationDate = model.GraduationDate;
                        oldModel.Major = model.Major;
                        oldModel.DegreeId = model.DegreeId;
                        oldModel.TitleId = model.TitleId;
                        oldModel.TitleDate = model.TitleDate;
                        oldModel.TitleLevelId = model.TitleLevelId;
                        oldModel.WorkingDate = model.WorkingDate;
                        oldModel.JoinInDate = model.JoinInDate;
                        oldModel.HomeZipCode = model.HomeZipCode;
                        oldModel.HomeAddress = model.HomeAddress;
                        oldModel.HomePhone = model.HomePhone;
                        oldModel.HomeFax = model.HomeFax;
                        oldModel.Province = model.Province;
                        oldModel.City = model.City;
                        oldModel.Area = model.Area;
                        oldModel.NativePlace = model.NativePlace;
                        oldModel.PartyId = model.PartyId;
                        oldModel.NationId = model.NationId;
                        oldModel.NationalityId = model.NationalityId;
                        oldModel.WorkingPropertyId = model.WorkingPropertyId;
                        oldModel.Competency = model.Competency;
                        oldModel.EmergencyContact = model.EmergencyContact;
                        oldModel.IsDimission = model.IsDimission;
                        oldModel.DimissionDate = model.DimissionDate;
                        oldModel.DimissionCause = model.DimissionCause;
                        oldModel.DimissionWhither = model.DimissionWhither;

                        oldModel.Remark = model.Remark;
                        oldModel.SortIndex = model.SortIndex;
                        oldModel.ModifyDate = DateTime.Now;
                        oldModel.ModifyUserId = model.ModifyUserId;
                        oldModel.ModifyUserName = model.ModifyUserName;
                        #endregion

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
    }
} 
