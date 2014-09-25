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
    /// 用户角色
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 用户权限设置
        /// <summary>
        /// 用户权限设置
        /// </summary>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// </returns>
        public int UserRoleSet(int systemId, int userId, List<int> lstRoleId)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.UserRoleSet(User.Current.Credentials.ToXmlString(), systemId, userId, lstRoleId.ToArray());
            });
        }
        #endregion

        #region 用户权限设置
        /// <summary>
        /// 用户权限设置
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable UserRoleGetList(int systemId, int userId)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.UserRoleGetList(User.Current.Credentials.ToXmlString(), systemId, userId);
            });
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion
    }
}
