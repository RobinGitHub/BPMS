using BPMS.Common;
using BPMS.Model;
using BPMS.References.BPMService;
using System;
using System.Data;
using System.ServiceModel;

namespace BPMS.References
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 获取系统日志列表
        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        public DataTable SysLogGetList(DateTime startDate, DateTime endDate, int operationType, string moduleName, string account, int pageIndex, int pageSize, out int count)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            count = 0;
            try
            {
                string rlt = _proxy.SysLogGetList(out count, User.Current.Credentials.ToXmlString(), startDate, endDate, operationType, moduleName, account, pageIndex, pageSize);
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
        /// <param name="endDate"></param>
        /// <returns>成功 true 失败 false </returns>
        public bool SysLogClear(DateTime endDate)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.SysLogClear(User.Current.Credentials.ToXmlString(), endDate);
            });
        }
        #endregion
    }
}
