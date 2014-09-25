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
    public partial class UserPurviewDal : BaseDAL<UserPurview>
    {
        public void Delete(BPMSEntities ctx, int systemId, int roleId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  UserPurview ");
            strSql.Append(" WHERE ID IN ");
            strSql.Append(" ( ");
            strSql.Append(" SELECT a.ID ");
            strSql.Append(" FROM UserPurview a ");
            strSql.Append(" INNER JOIN PurviewInfo b on a.PurviewId = b.ID ");
            strSql.Append(String.Format(" WHERE a.RoleId = {0} and b.SystemId = {1} and a.UserId = {2}", roleId, systemId, userId));
            strSql.Append(" ) ");
            ctx.Database.ExecuteSqlCommand(strSql.ToString(), null);
        }
    }
} 
