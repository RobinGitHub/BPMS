using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.DAL;
using BPMS.Model;
using BPMS.Common;
using System.Data;

namespace BPMS.Services
{
    /// <summary>
    /// 公共访问类
    /// </summary>
    public partial class Service
    {
        /// <summary>
        /// 获取启用的系统列表
        /// </summary>
        /// <returns></returns>
        public string PubGetSystemList()
        {
            var list = this.BLLProvider.SystemInfoBLL.GetList(t => new { t.ID, t.Name }, t => t.IsEnable);
            DataTable rltDt = ConvertHelper.ToDataTable(list.Cast<object>().ToArray());
            return ZipHelper.CompressDataTable(rltDt);
        }
        /// <summary>
        /// 系统下所有角色
        /// </summary>
        public string PubGetRoleList(int systemId)
        {
            var list = this.BLLProvider.RoleInfoBLL.GetList(t => new { t.ID, t.Name }, t => t.IsEnable && t.SystemId == systemId);
            DataTable rltDt = ConvertHelper.ToDataTable(list.Cast<object>().ToArray());
            return ZipHelper.CompressDataTable(rltDt);
        }
        /// <summary>
        /// 系统下所有权限
        /// </summary>
        public string PubGetAllPurview(int systemId)
        {
            var list = this.BLLProvider.PurviewInfoBLL.GetList(t => new { t.ID, t.Name, t.ParentId }, t => t.IsEnable && t.SystemId == systemId);
            DataTable rltDt = ConvertHelper.ToDataTable(list.Cast<object>().ToArray());
            return ZipHelper.CompressDataTable(rltDt);
        }
        /// <summary>
        /// 用户权限列表
        /// </summary>
        public string PubGetUserPurviewList(int systemId, int roleId, int userId)
        {
            DataTable rltDt = this.BLLProvider.UserPurviewBLL.GetUserPurview(systemId, roleId, userId);
            return ZipHelper.CompressDataTable(rltDt);
        }

        /// <summary>
        /// 查询部门下的员工
        /// </summary>
        public string PubGetUserByDeptId(int deptId)
        {
            var list = this.BLLProvider.EmployeeBLL.GetList(t => new { t.ID, t.Name }, t => t.DepartmentId == deptId);
            DataTable rltDt = ConvertHelper.ToDataTable(list.Cast<object>().ToArray());
            return ZipHelper.CompressDataTable(rltDt);
        }
    }
}
