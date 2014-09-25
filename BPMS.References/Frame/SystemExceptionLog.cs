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
    /// 系统异常日志
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 获取异常日志
        /// <summary>
        /// 获取异常日志
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        public DataTable SystemExceptionGetList(DateTime startDate, DateTime endDate, int pageIndex, int pageSize, out int count)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            count = 0;
            try
            {
                string rlt = _proxy.SystemExceptionGetList(out count, User.Current.Credentials.ToXmlString(), startDate, endDate, pageIndex, pageSize);
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

        #region 清除日志
        /// <summary>
        /// 清除日志
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        public bool SystemExceptionClear()
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.SystemExceptionClear(User.Current.Credentials.ToXmlString());
            });
        }
        #endregion

    }
}
