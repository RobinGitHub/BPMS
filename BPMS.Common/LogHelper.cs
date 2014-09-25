using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace BPMS.Common
{
    public static class LogHelper
    {
        /// <summary>
        /// 返回企业库的日志管理
        /// </summary>
        public static ILog Log
        {
            get
            {
                return new EnterLibLog();
            }
        }
    }


    #region EnterLibLog
    internal sealed class EnterLibLog : ILog
    {
        private readonly static LogWriter logWriter = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();

        private void Write(object message, string category)
        {
            IDictionary<string, object> dic = new Dictionary<string, object>();
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User != null)
                {
                    dic.Add("userid", ConvertHelper.ObjectToString(HttpContext.Current.User.Identity.Name));
                    dic.Add("url", ConvertHelper.ObjectToString(HttpContext.Current.Request.RawUrl));
                }
                else
                {
                    dic.Add("userid", "");
                    dic.Add("url", "");
                }
            }

            logWriter.Write(message, category, dic);
        }

        #region ILog 成员
        /// <summary>
        /// 记录操作
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            this.Write(message, "Info");
        }

        #endregion
    }


    public interface ILog
    {
        void Info(object message);
    }
    #endregion
}
