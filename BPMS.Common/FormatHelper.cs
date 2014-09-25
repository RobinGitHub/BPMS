using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.Common
{
    /// <summary>
    /// 格式化帮助类
    /// </summary>
    public class FormatHelper
    {
        #region 格式化日期字符串
        /// <summary>
        /// 格式化日期字符串 0 yyyyMMdd HH:mm 1 yyyyMMdd br HH:mm 2 yyyy-MM-dd HH:mm 3 HH:mm 4 yyyy-MM-dd
        /// </summary>
        /// <param name="objDateTime"></param>
        /// <param name="showType"></param>
        /// <returns></returns>
        public static string FormatDateTime(object objDateTime, int showType)
        {
            if (objDateTime == null)
            {
                return "";
            }
            try
            {
                DateTime dt = Convert.ToDateTime(objDateTime);
                switch (showType)
                {
                    case 0:
                        return dt.ToString("yyyyMMdd HH:mm");
                    case 1:
                        return dt.ToString("yyyyMMdd<br>HH:mm");
                    case 2:
                        return dt.ToString("yyyy-MM-dd HH:mm");
                    case 3:
                        return dt.ToString("HH:mm");
                    case 4:
                        return dt.ToString("yyyy-MM-dd");
                    default:
                        break;
                }
            }
            catch
            { }
            return "";
        }
        #endregion
    }
}
