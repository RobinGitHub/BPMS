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
    public partial class MenuInfoDal : BaseDAL<MenuInfo>
    { 
        public DataTable GetList(int systemId, int parentId)
        {
            using (var ctx = new BPMSEntities())
            {
                Expression<Func<MenuInfo, bool>> condition = t => t.SystemId == systemId && t.ParentId == parentId;
                var list = from a in ctx.MenuInfo.Where(condition)
                           join b in ctx.SystemInfo on a.SystemId equals b.ID
                           select new 
                           {
                               a.ID,
                               a.SystemId,
                               SystemName = b.Name,
                               a.ParentId,
                               a.Name,
                               a.Code,
                               a.Category,
                               a.PurviewId,
                               a.Icon,
                               a.IconUrl,
                               a.NavigateUrl,
                               a.FormName,
                               a.IsSplit,
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
                return ConvertHelper.ToDataTable(list.ToList().Cast<object>().ToArray());
            }

        }
    }
} 
