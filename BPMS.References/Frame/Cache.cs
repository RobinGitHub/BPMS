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
    /// 缓存
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 缓存列表
        /// <summary>
        /// 缓存列表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public DataTable GetCacheKeyList(string keyWord, int pageSize, int pageIndex, out int count)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            count = 0;
            try
            {
                string rlt = _proxy.GetCacheKeyList(out count, User.Current.Credentials.ToXmlString(), keyWord, pageIndex, pageSize);
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

        #region 清空缓存
        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <param name="lstKeyWord"></param>
        /// <returns>0 失败 1成功</returns>
        public int CacheClear(List<string> lstKeyWord)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.CacheClear(User.Current.Credentials.ToXmlString(), lstKeyWord.ToArray());
            });
        }
        #endregion
    }
}
