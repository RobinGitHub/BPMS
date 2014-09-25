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
    public partial class UserInfoDal : BaseDAL<UserInfo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchType">
        /// 1=编号、2=账号、3=姓名
        /// </param>
        /// <param name="keyWord"></param>
        /// <param name="isEnable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable GetList(int searchType, string keyWord, int isEnable, int pageIndex, int pageSize, out int count)
        {
            Expression<Func<UserInfo, bool>> condition = t => true;
            if (isEnable != EIsEnable.全部.GetHashCode())
            {
                bool bIsEnable = isEnable == EIsEnable.启用.GetHashCode() ? true : false;
                condition.And(t => t.IsEnable == bIsEnable); 
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                if (searchType == 1)
                    condition.And(t => t.Code.Contains(keyWord)); 
                if (searchType == 2)
                    condition.And(t => t.Account.Contains(keyWord));
                if (searchType == 3)
                    condition.And(t => t.Name.Contains(keyWord)); 
            }
            using (var ctx = new BPMSEntities())
            {
                var list = from a in ctx.UserInfo
                           join b in ctx.Organization on a.CompanyId equals b.ID
                           join c in ctx.Organization on a.DepartmentId equals c.ID
                           join d in ctx.Organization on a.WorkgroupId equals d.ID
                           select new 
                           {
                               a.ID,
                               a.Code,
                               a.Account,
                               a.Name,
                               a.Spell,
                               a.Alias,
                               a.RoleId,
                               a.Gender,
                               a.Mobile,
                               a.Telephone,
                               a.Birthday,
                               a.Email,
                               a.OICQ,
                               a.DutyId,
                               a.TitleId,
                               a.CompanyId,
                               CompanyName = b.Name,
                               a.DepartmentId,
                               DepartmentName = c.Name,
                               a.WorkgroupId,
                               WorkgroupName = d.Name,
                               a.ChangePasswordDate,
                               a.IPAddress,
                               a.MACAddress,
                               a.LogOnCount,
                               a.FirstVisit,
                               a.PreviousVisit,
                               a.LastVisit,
                               a.Remark,
                               a.IsEnable,
                               a.SortIndex,
                               a.CreateDate,
                               a.CreateUserId,
                               a.CreateUserName,
                               a.ModifyUserId,
                               a.ModifyDate,
                               a.ModifyUserName
                           };
                count = list.Count();
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return ConvertHelper.ToDataTable(list.ToList().Cast<object>().ToArray());
            }
        }
    }
} 
