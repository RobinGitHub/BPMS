using BPMS.Common;
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
        public string SysLoginLogGetList(string xmlCredentials, DateTime startDate, DateTime endDate, string account, int pageIndex, int pageSize, out int count)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.LoginLogMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.SysLoginLogBLL.GetList(objCredentials.UserId, objCredentials.UserName, startDate, endDate, objCredentials.SystemId, account, pageIndex, pageSize, out count);
            return ZipHelper.CompressDataTable(dtRlt);
        }
        /// <summary>
        /// 清除日志
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool SysLoginLogClear(string xmlCredentials, DateTime endDate)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.LoginLogMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.SysLoginLogBLL.Delete(objCredentials.UserId, objCredentials.UserName, objCredentials.SystemId, endDate) > 0;
        }

    }
}
