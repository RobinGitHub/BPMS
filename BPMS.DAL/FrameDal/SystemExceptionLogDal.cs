using System; 
using System.Collections.Generic; 
using System.Data; 
using System.Linq; 
using System.Text; 
using BPMS.Model;
using BPMS.Common;
using System.Linq.Expressions;

namespace BPMS.DAL 
{ 
    /// <summary> 
    /// 
    /// </summary>  
    public partial class SystemExceptionLogDal : BaseDAL<SystemExceptionLog>
    {
        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(string source, string exception, string desction)
        {
            int rlt = 1;
            SystemExceptionLog model = new SystemExceptionLog();
            model.ID = GetNewID();
            model.Source = source;
            model.Exception = exception;
            model.Description = desction;
            model.CreateDate = DateTime.Now;
            var location = IPHelper.GetLocation();
            model.IPAddress = location.GetIPAddress.ToString();
            model.IPAddressName = location.ToString();
            if (!Insert(model))
                rlt = 0;
            return rlt;
        }

        #endregion
    }
} 
