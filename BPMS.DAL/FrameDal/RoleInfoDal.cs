using System; 
using System.Collections.Generic; 
using System.Data; 
using System.Linq; 
using System.Text; 
using BPMS.Model;
using BPMS.Common;

namespace BPMS.DAL 
{ 
    /// <summary> 
    /// 
    /// </summary>  
    public partial class RoleInfoDal : BaseDAL<RoleInfo>
    {
        public DataTable GetList(int systemId)
        { 
            using (var ctx = new BPMSEntities())
            {
                var list = from a in ctx.RoleInfo.Where(t => t.SystemId == systemId)
                           join b in ctx.SystemInfo on a.SystemId equals b.ID
                           select new 
                           {
                               a.ID,
                               a.SystemId,
                               SystemName = b.Name,
                               a.Name,
                               a.Code,
                               a.Category,
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
                return ConvertHelper.ToDataTable(list.Cast<object>().ToArray());
            }
        }

        #region 角色成员
        /// <summary>
        /// 角色成员
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="roleId"></param>
        /// <returns>用户列表</returns>
        public DataTable RoleMembers(int roleId)
        {
            using (var ctx = new BPMSEntities())
            {
                var list = from a in ctx.UserRole.Where(t => t.RoleId == roleId)
                           join b in ctx.UserInfo.Where(t => t.IsEnable) on a.UserId equals b.ID
                           select b;
                return ConvertHelper.ToDataTable(list.ToList().Cast<object>().ToList());
            }
        }
        #endregion
    }
} 
