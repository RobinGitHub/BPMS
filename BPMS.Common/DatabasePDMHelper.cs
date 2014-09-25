using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;

namespace BPMS.Common
{
    /// <summary>
    /// 读取数据库PDM设计文件
    /// </summary>
    public class DatabasePDMHelper
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetDataTableName(string code)
        {
            string fileName = ConfigurationManager.AppSettings["PDMFileName"];
            string filePaht = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(filePaht))
                throw new Exception("数据库设计文件不存在！");
            XElement xe = XElement.Load(filePaht);
            var tables = xe.Elements().FirstOrDefault().Elements().FirstOrDefault().Elements().FirstOrDefault().Elements().Where(a => a.Name.LocalName == "Tables").FirstOrDefault();

            string tableName = string.Empty;
            foreach (XElement item in tables.Elements())
            {
                foreach (XElement child in item.Elements())
                {
                    if (child.Name.LocalName == "Code" && child.Value == code)
                    {
                        tableName = item.Elements().Where(t => t.Name.LocalName == "Name").FirstOrDefault().Value.ToString();
                        break;
                    }
                }
            }
            return tableName;
        }
        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="tableCode"></param>
        /// <param name="columnCode"></param>
        /// <returns></returns>
        public static string GetColumnName(string tableCode, string columnCode)
        {
            string fileName = ConfigurationManager.AppSettings["PDMFileName"];
            string filePaht = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(filePaht))
                throw new Exception("数据库设计文件不存在！");
            XElement xe = XElement.Load(filePaht);
            var tables = xe.Elements().FirstOrDefault().Elements().FirstOrDefault().Elements().FirstOrDefault().Elements().Where(a => a.Name.LocalName == "Tables").FirstOrDefault();

            string columnName = string.Empty;
            foreach (XElement item in tables.Elements())
            {
                foreach (XElement child in item.Elements())
                {
                    if (child.Name.LocalName == "Code" && child.Value == tableCode)
                    {
                        foreach (XElement column in item.Elements().Where(t => t.Name.LocalName == "Columns").Elements())
                        {
                            foreach (XElement col in column.Elements())
                            {
                                if (col.Name.LocalName == "Code" && col.Value == columnCode)
                                {
                                    columnName = column.Elements().Where(t => t.Name.LocalName == "Name").FirstOrDefault().Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return columnName;
        }

    }
}
