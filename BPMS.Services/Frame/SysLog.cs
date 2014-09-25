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
        public string SysLogGetList(string xmlCredentials, DateTime startDate, DateTime endDate, int operationType, string moduleName, string account, int pageIndex, int pageSize, out int count)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.SysLogMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.SysLogBLL.GetList(objCredentials.UserId, objCredentials.UserName, startDate, endDate, (EOperationType)operationType, moduleName, account, pageIndex, pageSize, out count);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public bool SysLogClear(string xmlCredentials, DateTime endDate)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.SysLogMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.SysLogBLL.Delete(objCredentials.UserId, objCredentials.UserName, endDate) > 0;
        }


    }
}
