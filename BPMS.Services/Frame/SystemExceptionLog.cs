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
        public string SystemExceptionGetList(string xmlCredentials, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, out int count)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.SystemExceptionMng, EActions.Vie) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            DataTable dtRlt = this.BLLProvider.SystemExceptionLogBLL.GetList(objCredentials.UserId, objCredentials.UserName, startDate, endDate, pageIndex, pageSize, out count);
            return ZipHelper.CompressDataTable(dtRlt);
        }

        public bool SystemExceptionClear(string xmlCredentials)
        {
            ClientCredentials objCredentials = xmlCredentials.ToModel<ClientCredentials>();
            if (CheckPurview(objCredentials, EModules.SystemMng, EFunctions.SystemExceptionMng, EActions.Del) != 1)
                throw new Exception(String.Format("Service Method:{0} Access Error", base.GetActionName()));
            return this.BLLProvider.SystemExceptionLogBLL.Delete(objCredentials.UserId, objCredentials.UserName) > 0;
        }

    }
}
