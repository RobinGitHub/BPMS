using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPMS.BLL;
using BPMS.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BPMS.Model;
namespace BPMS.BLL.Tests
{
    [TestClass()]
    public class UserInfoBllTests
    {
        [TestMethod()]
        public void LoginTest()
        {
            int systemId = 0;
            int userId = 0; 
            string userName = "";
            List<RoleInfo> roleList = null;
            UserInfoBll userInfo = new UserInfoBll();
            int rlt = userInfo.Login(Consts.ConstValue.SystemCode, "admin", "admin", out systemId, out userId, out userName, out roleList);

        }
    }
}
