using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPMS.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BPMS.Model;
using System.Data;
using BPMS.Common;
namespace BPMS.BLL.Tests
{
    [TestClass()]
    public class SysLogBllTests
    {
        [TestMethod()]
        public void AddTest()
        {
            //SysLogBll bll = new SysLogBll();

            //User.Current.ID = 1;
            //User.Current.Name = "管理员";
            //User.Current.SystemId = 1;
            //User.Current.RoleId = 1;

            //SysLog model = new SysLog();
            //model.OperationType = EOperationType.新增.GetHashCode();
            //model.TableName = "Test";
            //model.BusinessName = "测试";
            //model.ObjectID = 1;

            //List<SysLogDetails> detailList = new List<SysLogDetails>();
            //detailList.Add(new SysLogDetails() { FieldName = "Test1", FieldText = "Test1", NewValue = "Test1", OldValue = "Test2" });

            //var rlt = bll.Add(null, model, detailList);

            //DateTime startDate = DateTime.Now.AddMonths(-1);
            //DateTime endDate = DateTime.Now;
            //int pageIndex = 1;
            //int pageSize = 1;
            //int count = 1;
            //DataTable dt = bll.GetList(startDate, endDate, EOperationType.全部, "", "", pageIndex, pageSize, out count);

            //int id = ConvertHelper.ObjectToInt(dt.Rows[0]["ID"]);

            //DataTable detailDt = bll.GetDetailList(id);
        }
    }
}
