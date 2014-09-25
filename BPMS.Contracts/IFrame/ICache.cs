using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BPMS.Contracts
{
    public partial interface IService
    {
        /// <summary>
        /// 获取缓存Key列表
        /// </summary>
        /// <returns>Key</returns>
        [OperationContract]
        string GetCacheKeyList(string xmlCredentials, string keyWord, int pageIndex, int pageSize, out int count);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [OperationContract]
        int CacheClear(string xmlCredentials, List<string> lstKeyWord);
    }

}
