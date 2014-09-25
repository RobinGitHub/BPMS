using BPMS.Common;
using BPMS.Contracts;
using BPMS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BPMS.Services
{
    public partial class Service
    {
        /// <summary>
        /// 缓存列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="keyWord"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public string GetCacheKeyList(string xmlCredentials, string keyWord, int pageSize, int pageIndex, out int count)
        {
            if (CheckPurview(xmlCredentials, EModules.PurviewMng, EFunctions.CacheMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            List<string> lstKey = CacheHelper.GetCacheKeyList(pageIndex, pageSize, out count, keyWord).OrderBy(t => t).ToList();
            DataTable tbRlt = new DataTable();
            tbRlt.Columns.Add("Key");
            foreach (string key in lstKey)
            {
                DataRow row = tbRlt.NewRow();
                row["Key"] = key;
                tbRlt.Rows.Add(row);
            }
            return ZipHelper.CompressDataTable(tbRlt);
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="lstKeyWord"></param>
        /// <returns>0 失败 1成功</returns>
        public int CacheClear(string xmlCredentials, List<string> lstKeyWord)
        {
            if (CheckPurview(xmlCredentials, EModules.PurviewMng, EFunctions.CacheMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            foreach (string key in lstKeyWord)
                CacheHelper.Remove(key);
            return 1;
        }
    }
}
