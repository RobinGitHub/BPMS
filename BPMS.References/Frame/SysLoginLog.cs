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
    /// 系统登录日志
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 获取系统登录日志列表
        /// <summary>
        /// 获取系统登录日志列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        public DataTable SysLoginLogGetList(DateTime startDate, DateTime endDate, string account, int pageIndex, int pageSize, out int count)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            count = 0;
            try
            {
                string rlt = _proxy.SysLoginLogGetList(out count, User.Current.Credentials.ToXmlString(), startDate, endDate, account, pageIndex, pageSize);
                return ZipHelper.DecompressDataTable(rlt);
            }
            catch (EndpointNotFoundException endPointEx)
            {
                throw endPointEx.InnerException;
            }
            catch (Exception ex)
            {
                if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                    CreateOpenClient();
                throw ex;
            }
        }
        #endregion

        #region 清除登录日志
        /// <summary>
        /// 清除登录日志
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool SysLoginLogClear(DateTime endDate)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.SysLoginLogClear(User.Current.Credentials.ToXmlString(), endDate);
            });
        }
        #endregion
    }
}
