using System; 
using System.Collections.Generic; 
using System.Data; 
using System.Linq; 
using System.Text; 
using BPMS.Model;
using BPMS.Common;
using System.Linq.Expressions;

namespace BPMS.DAL 
{ 
    /// <summary> 
    /// 
    /// </summary>  
    public partial class PurviewInfoDal : BaseDAL<PurviewInfo>
    {
        #region 列表
        /// <summary>
        /// 模块列表
        /// </summary>
        public DataTable GetModuleList(int systemId, string moduleName, string moduleCode, int isEnable, int pageIndex, int pageSize, out int count)
        {
            using(var ctx = new BPMSEntities())
            {
                Expression<Func<PurviewInfo, bool>> condition = t => t.SystemId == systemId && t.PurviewType == EPurviewType.模块.GetHashCode();
                if (isEnable != EIsEnable.全部.GetHashCode())
                {
                    bool bIsEnable = isEnable == EIsEnable.启用.GetHashCode() ? true : false;
                    condition.And(t => t.IsEnable == bIsEnable); 
                }
                if (!string.IsNullOrEmpty(moduleName))
                    condition.And(t => t.Name.Contains(moduleName));
                if (!string.IsNullOrEmpty(moduleCode))
                    condition.And(t => t.Code.Contains(moduleCode));

                var list = from a in ctx.PurviewInfo.Where(condition)
                           join b in ctx.SystemInfo on a.SystemId equals b.ID
                           select new 
                           {
                               a.ID,
                               a.SystemId,
                               SystemName = b.Name,
                               a.Name,
                               a.Code,
                               a.ParentId,
                               ParentName = "",
                               a.PurviewType,
                               a.Remark,
                               a.IsEnable,
                               a.SortIndex,
                               a.CreateDate,
                               a.CreateUserId,
                               a.CreateUserName,
                               a.ModifyDate,
                               a.ModifyUserId,
                               a.ModifyUserName
                           };
                count = list.Count();
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return ConvertHelper.ToDataTable(list.ToList().Cast<object>().ToArray());
            } 
        }

        /// <summary>
        /// 功能列表
        /// </summary>
        public DataTable GetFunctionList(int systemId, int moduleId, string functionName, string functionCode, int isEnable, int pageIndex, int pageSize, out int count)
        {
            using (var ctx = new BPMSEntities())
            {
                Expression<Func<PurviewInfo, bool>> condition = t => t.SystemId == systemId && t.PurviewType == EPurviewType.功能.GetHashCode();
                if (isEnable != EIsEnable.全部.GetHashCode())
                {
                    bool bIsEnable = isEnable == EIsEnable.启用.GetHashCode() ? true : false;
                    condition.And(t => t.IsEnable == bIsEnable);
                }
                if (moduleId > 0)
                    condition.And(t => t.ParentId == moduleId);
                if (!string.IsNullOrEmpty(functionName))
                    condition.And(t => t.Name.Contains(functionName));
                if (!string.IsNullOrEmpty(functionCode))
                    condition.And(t => t.Code.Contains(functionCode));

                var list = from a in ctx.PurviewInfo.Where(condition)
                           join b in ctx.SystemInfo on a.SystemId equals b.ID
                           select new
                           {
                               a.ID,
                               a.SystemId,
                               SystemName = b.Name,
                               a.Name,
                               a.Code,
                               a.ParentId,
                               ParentName = "",
                               a.PurviewType,
                               a.Remark,
                               a.IsEnable,
                               a.SortIndex,
                               a.CreateDate,
                               a.CreateUserId,
                               a.CreateUserName,
                               a.ModifyDate,
                               a.ModifyUserId,
                               a.ModifyUserName
                           };
                count = list.Count();
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return ConvertHelper.ToDataTable(list.ToList().Cast<object>().ToArray());
            }
        }
        /// <summary>
        /// 操作列表
        /// </summary>
        public DataTable GetActionList(int systemId, int moduleId, int functionId, string actionName, string actionCode, int isEnable, int pageIndex, int pageSize, out int count)
        {
            using (var ctx = new BPMSEntities())
            {
                Expression<Func<PurviewInfo, bool>> condition = t => t.SystemId == systemId && t.PurviewType == EPurviewType.操作.GetHashCode();
                if (isEnable != EIsEnable.全部.GetHashCode())
                {
                    bool bIsEnable = isEnable == EIsEnable.启用.GetHashCode() ? true : false;
                    condition.And(t => t.IsEnable == bIsEnable);
                }
                if (functionId > 0)
                    condition.And(t => t.ParentId == functionId);
                if (!string.IsNullOrEmpty(actionName))
                    condition.And(t => t.Name.Contains(actionName));
                if (!string.IsNullOrEmpty(actionCode))
                    condition.And(t => t.Code.Contains(actionCode));

                var list = from a in ctx.PurviewInfo.Where(condition)
                           join b in ctx.SystemInfo on a.SystemId equals b.ID
                           join c in ctx.PurviewInfo on a.ParentId equals c.ID
                           select new
                           {
                               a.ID,
                               a.SystemId,
                               SystemName = b.Name,
                               a.Name,
                               a.Code,
                               a.ParentId,
                               MoudleId = c.ParentId,
                               ParentName = "",
                               a.PurviewType,
                               a.Remark,
                               a.IsEnable,
                               a.SortIndex,
                               a.CreateDate,
                               a.CreateUserId,
                               a.CreateUserName,
                               a.ModifyDate,
                               a.ModifyUserId,
                               a.ModifyUserName
                           };
                if (moduleId > 0)
                    list = list.Where(t => t.MoudleId == moduleId);
                count = list.Count();
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return ConvertHelper.ToDataTable(list.ToList().Cast<object>().ToArray());
            }
        }
        #endregion
    }
} 
