using BPMS.Common;
using BPMS.Model;
using BPMS.References.BPMService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace BPMS.References
{
    /// <summary>
    /// 公共访问
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 获取启用的系统列表
        /// <summary>
        /// 获取启用的系统列表
        /// </summary>
        /// <returns></returns>
        public DataTable PubGetSystemList()
        {
            string rlt = _proxy.PubGetSystemList();
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 系统下所有角色
        /// <summary>
        /// 系统下所有角色
        /// </summary>
        public DataTable PubGetRoleList(int systemId)
        {
            string rlt = _proxy.PubGetRoleList(systemId);
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 系统下所有权限
        /// <summary>
        /// 系统下所有权限
        /// </summary>
        public DataTable PubGetAllPurview(int systemId)
        {
            string rlt = _proxy.PubGetAllPurview(systemId);
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 用户权限列表
        /// <summary>
        /// 用户权限列表
        /// </summary>
        public DataTable PubGetUserPurviewList(int systemId, int roleId, int userId)
        {
            string rlt = _proxy.PubGetUserPurviewList(systemId, roleId, userId);
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 查询部门下的员工
        /// <summary>
        /// 查询部门下的员工
        /// </summary>
        public DataTable PubGetUserByDeptId(int deptId)
        {
            string rlt = _proxy.PubGetUserByDeptId(deptId);
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion
    }
}
