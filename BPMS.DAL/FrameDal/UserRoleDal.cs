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
    public partial class UserRoleDal : BaseDAL<UserRole>
    {
        public DataTable GetList(int systemId, int userId)
        {
            using (var ctx = new BPMSEntities())
            {
                var list = from a in ctx.UserRole
                           join b in ctx.RoleInfo on a.RoleId equals b.ID
                           where a.UserId == userId && b.SystemId == systemId
                           select new 
                           {
                               b.ID,
                               b.Name
                           };
                return ConvertHelper.ToDataTable(list.ToList().Cast<object>().ToArray());
            }
        }

        public void Delete(BPMSEntities ctx, int systemId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM UserRole ");
            strSql.Append(" WHERE UserId = " + userId + " and RoleId in  ");
            strSql.Append(" ( ");
            strSql.Append(" SELECT ID ");
            strSql.Append(" FROM RoleInfo ");
            strSql.Append(" WHERE SystemId = " + systemId + " ");
            strSql.Append(" ) ");
            ctx.Database.ExecuteSqlCommand(strSql.ToString(), null);
        }
    }
} 
