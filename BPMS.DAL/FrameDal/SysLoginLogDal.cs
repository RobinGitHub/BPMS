using System; 
using System.Collections.Generic; 
using System.Data; 
using System.Linq; 
using System.Text; 
using BPMS.Model;
using BPMS.Common;
using System.Linq.Expressions;
using System.Collections;

namespace BPMS.DAL 
{ 
    /// <summary> 
    /// 
    /// </summary>  
    public partial class SysLoginLogDal : BaseDAL<SysLoginLog>
    {
        #region 日志列表
        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="account"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable GetList(DateTime startDate, DateTime endDate, int systemId, string account, int pageIndex, int pageSize, out int count)
        {
            count = 0;
            using (var ctx = new BPMSEntities())
            {
                DateTime tmpStartDate = ConvertHelper.ObjectToDateTime(startDate.ToShortDateString() + " 00:00:00");
                DateTime tmpEndDate = ConvertHelper.ObjectToDateTime(endDate.AddDays(1).ToShortDateString() + " 00:00:00");
                Expression<Func<SysLoginLog, bool>> condition = t => t.CreateDate >= tmpStartDate && t.CreateDate <= endDate && t.SystemId == systemId;
                if (!string.IsNullOrEmpty(account))
                    condition.And(t => t.Account.Contains(account));

                ///先 匿名查询 
                var list = from a in ctx.SysLoginLog.Where(condition)
                           join b in ctx.SystemInfo on a.SystemId equals b.ID
                           join c in ctx.UserInfo on a.Account equals c.Account into logInfo
                           from log in logInfo.DefaultIfEmpty()
                           select new
                           {
                               Account = a.Account,
                               AccountName = log == null ? string.Empty : log.Name,
                               a.CreateDate,
                               a.IPAddress,
                               a.IPAddressName,
                               a.Status,
                               a.SystemId,
                               SystemName = b.Name
                           };
                count = list.Count();
                var data = list.OrderByDescending(t => t.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                DataTable tbRlt = ConvertHelper.ToDataTable(data.ToList().Cast<object>().ToArray());
                DataColumn col = new DataColumn("StatusName");
                tbRlt.Columns.Add(col);
                foreach (DataRow item in tbRlt.Rows)
                {
                    item["StatusName"] = Enum.GetName(typeof(ELoginStatus), item["Status"]);
                }
                return tbRlt;
            }
        }
        #endregion
    }
} 
