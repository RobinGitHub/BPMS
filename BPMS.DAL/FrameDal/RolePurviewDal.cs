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
    public partial class RolePurviewDal : BaseDAL<RolePurview>
    {
        #region 角色权限
        public DataTable GetPurviewList(int systemId, int roleId)
        {
            using (var ctx = new BPMSEntities())
            {
                var list = from a in ctx.RolePurview.Where(t => t.RoleId == roleId)
                           join b in ctx.RoleInfo on a.RoleId equals b.ID
                           join c in ctx.PurviewInfo on a.PurviewId equals c.ID
                           where c.SystemId == systemId
                           select new
                           {
                               c.ID,
                               c.Name,
                               c.Code
                           };
                return ConvertHelper.ToDataTable(list.ToList().Cast<object>().ToArray());
            }
        }
        #endregion

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
