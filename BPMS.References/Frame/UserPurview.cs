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
    /// 用户权限
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 用户权限列表
        /// <summary>
        /// 用户权限列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable UserGetPurviewList(int systemId, int roleId, int userId)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.UserGetPurviewList(User.Current.Credentials.ToXmlString(), systemId, roleId, userId);
            });
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 设置用户权限
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userPurviews">用户权限对象</param>
        /// <returns>
        /// 0 失败，请联系管理员
        /// 1 成功
        /// </returns>
        public int UserPurviewSet(List<UserPurview> userPurviews)
        {
            List<string> lstModel = new List<string>();
            foreach (UserPurview item in userPurviews)
            {
                lstModel.Add(item.ToXmlString());
            }
            return TryCatchCore<int>(() =>
            {
                return _proxy.UserPurviewSet(User.Current.Credentials.ToXmlString(), lstModel.ToArray());
            });
        }
        #endregion

        #region 重置权限
        /// <summary>
        /// 重置权限
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// </returns>
        public int UserPurviewReset(int systemId, int roleId, int userId)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.UserPurviewReset(User.Current.Credentials.ToXmlString(), systemId, roleId, userId);
            });
        }
        #endregion
    }
}
