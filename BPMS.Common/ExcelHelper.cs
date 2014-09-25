using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace BPMS.Common
{
    /// <summary>
    /// Excel文件操作类。
    /// </summary>
    public class ExcelHelper
    {
        //private static readonly int m_maxSheelSize = 65000;
        #region 公用静态方法

        #region 从Excel读数据
        /// <summary>
        /// 从Excel读数据
        /// </summary>
        /// <param name="filePath">excel文档路径</param>
        /// <param name="excelVersion">文档版本</param>
        /// <param name="pHDR">第一行是否标题</param>
        /// <param name="bMerge">
        /// 如果有多页，是否合并数据，合并时必须保证多页的表结构一致
        /// </param>
        /// <returns>DataTable集</returns>
        public static DataTable[] GetExcelData(string filePath, ExcelVersion excelVersion, HeadRowType pHDR, bool bMerge)
        {
            List<DataTable> dtResult = new List<DataTable>();
            string connectionString = string.Format(GetConnectionString(excelVersion, ImportOrExportType.Import),
              filePath, pHDR);
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                string[] sheels = GetExcelWorkSheets(filePath, excelVersion);
                foreach (string sheelName in sheels)
                {
                    try
                    {
                        DataTable dtExcel = new DataTable();
                        OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from [" + sheelName + "$]", con);

                        adapter.FillSchema(dtExcel, SchemaType.Mapped);
                        adapter.Fill(dtExcel);

                        dtExcel.TableName = sheelName;
                        dtResult.Add(dtExcel);
                    }
                    catch
                    {
                        //容错处理：取不到时，不报错，结果集为空即可。
                    }
                }

                //如果需要合并数据，则合并到第一张表
                if (bMerge)
                {
                    for (int i = 1; i < dtResult.Count; i++)
                    {
                        //如果不为空才合并
                        if (dtResult[0].Columns.Count == dtResult[i].Columns.Count &&
                            dtResult[i].Rows.Count > 0)
                        {
                            dtResult[0].Load(dtResult[i].CreateDataReader());
                        }
                    }
                }
            }
            return dtResult.ToArray();
        }
        #endregion

        #region 将Datatable转换为Excel
        /// <summary>
        /// 将Datatable转换为Excel
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="FileName"></param>
        public static void DataTableToExcel(System.Data.DataTable dtData, String FileName)
        {
            if (dtData == null || dtData.Rows.Count == 0)
            {
                return;
            }
            System.Web.UI.WebControls.GridView dgExport = null;
            //当前对话 
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            //IO用于导出并返回excel文件 
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                //设置编码和附件格式 
                //System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8)作用是方式中文文件名乱码
                curContext.Response.AddHeader("content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".xls");
                curContext.Response.ContentType = "application nd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;//.GetEncoding("GB2312");
                curContext.Response.Charset = "UTF-8";// "GB2312";

                //导出Excel文件 
                strWriter = new System.IO.StringWriter();
                htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

                //为了解决dgData中可能进行了分页的情况,需要重新定义一个无分页的GridView 
                dgExport = new System.Web.UI.WebControls.GridView();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();

                //下载到客户端 
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }
        #endregion
        #endregion

        #region 私有静态方法
        #region 返回指定文件所包含的工作簿列表;如果有WorkSheet，就返回以工作簿名字命名的ArrayList，否则返回空
        /// <summary>
        /// 返回指定文件所包含的工作簿列表;如果有WorkSheet，就返回以工作簿名字命名的ArrayList，否则返回空
        /// </summary>
        /// <param name="filePath">要获取的Excel</param>
        /// <param name="excelVersion">文档版本</param>
        /// <returns>如果有WorkSheet，就返回以工作簿名字命名的string[]，否则返回空</returns>
        private static string[] GetExcelWorkSheets(string filePath, ExcelVersion excelVersion)
        {
            List<string> alTables = new List<string>();
            string connectionString = string.Format(GetConnectionString(excelVersion, ImportOrExportType.Import),
              filePath, "Yes");
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                DataTable dt = new DataTable();

                dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    throw new Exception("无法获取指定Excel的架构。");
                }

                foreach (DataRow dr in dt.Rows)
                {
                    string tempName = dr["Table_Name"].ToString();

                    int iDolarIndex = tempName.IndexOf('$');

                    if (iDolarIndex > 0)
                    {
                        tempName = tempName.Substring(0, iDolarIndex);
                    }

                    //修正Excel2003中某些工作薄名称为汉字的表无法正确识别的BUG。
                    if (tempName[0] == '\'')
                    {
                        if (tempName[tempName.Length - 1] == '\'')
                        {
                            tempName = tempName.Substring(1, tempName.Length - 2);
                        }
                        else
                        {
                            tempName = tempName.Substring(1, tempName.Length - 1);
                        }
                    }
                    if (!alTables.Contains(tempName))
                    {
                        alTables.Add(tempName);
                    }

                }
            }

            if (alTables.Count == 0)
            {
                return null;
            }
            return alTables.ToArray();
        }
        #endregion



        /// <summary>
        /// 构建Excel列脚本。
        /// 格式如：Name VarChar，CreateDate Date
        /// </summary>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        private static string CreateExcelColums(DataTable dtSource)
        {
            //检查列数
            if (dtSource.Columns.Count == 0)
            {
                throw new Exception("数据源列数为0");
            }
            //构建列
            StringBuilder sbColums = new StringBuilder();
            foreach (DataColumn dc in dtSource.Columns)
            {
                sbColums.AppendFormat(",{0} {1}", dc.ColumnName, GetOleDbTypeByDataColumn(dc).ToString());
            }
            //去掉多余的逗号
            sbColums.Remove(0, 1);
            return sbColums.ToString();
        }
        ///// <summary>
        ///// 构建Excel列脚本。
        ///// 格式如：Name VarChar，CreateDate Date
        ///// </summary>
        ///// <param name="dtSource"></param>
        ///// <returns></returns>
        //private static string CreateExcelColums(DataGridView dtSource)
        //{
        //    //检查列数
        //    if (dtSource.Columns.Count == 0)
        //    {
        //        throw new Exception("数据源列数为0");
        //    }
        //    //构建列
        //    StringBuilder sbColums = new StringBuilder();
        //    foreach (DataGridViewColumn dc in dtSource.Columns)
        //    {
        //        sbColums.AppendFormat(",{0} {1}", dc.HeaderText, OleDbType.VarChar);
        //    }
        //    //去掉多余的逗号
        //    sbColums.Remove(0, 1);

        //    return sbColums.ToString();
        //}

        /// <summary>
        /// 获取DataColumn对应的Excel列类型
        /// </summary>
        /// <param name="dc">源数据的列</param>
        /// <returns>Excel列类型名称</returns>
        private static OleDbType GetOleDbTypeByDataColumn(DataColumn dc)
        {
            switch (dc.DataType.Name)
            {
                case "String"://字符串
                    return OleDbType.VarChar;
                case "Double"://数字
                    return OleDbType.Double;
                case "Decimal"://数字
                    return OleDbType.Decimal;
                case "DateTime"://时间
                    return OleDbType.Date;
                default:
                    return OleDbType.VarChar;
            }
        }
        /// <summary>
        /// 获得连接
        /// </summary>
        /// <param name="excelVersion"></param>
        /// <returns></returns>
        private static string GetConnectionString(ExcelVersion excelVersion, ImportOrExportType etype)
        {
            if (etype == ImportOrExportType.Import)
            {
                if (excelVersion == ExcelVersion.Excel12)
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1'";
                }
                else if (excelVersion == ExcelVersion.Excel3)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 3.0;HDR={1};IMEX=1'";
                }
                else if (excelVersion == ExcelVersion.Excel4)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 4.0;HDR={1};IMEX=1'";
                }
                else if (excelVersion == ExcelVersion.Excel5)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 5.0;HDR={1};IMEX=1'";
                }
                else
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 8.0;HDR={1};IMEX=1'";
                }
            }
            else
            {
                if (excelVersion == ExcelVersion.Excel12)
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1}'";
                }
                else if (excelVersion == ExcelVersion.Excel3)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 3.0;HDR={1}'";
                }
                else if (excelVersion == ExcelVersion.Excel4)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 4.0;HDR={1};'";
                }
                else if (excelVersion == ExcelVersion.Excel5)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 5.0;HDR={1};'";
                }
                else
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 8.0;HDR={1};'";
                }
            }
        }
        #endregion

        


        #region   运用流导出Excel
        //public static void DataToExcel(DataGridView dgv, string FileName)
        //{
        //    FileStream myStream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
        //    //StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

        //    string columnTitle = "";
        //    //写入列标题   
        //    for (int i = 0; i < dgv.ColumnCount; i++)
        //    {
        //        if (i > 0)
        //        {
        //            columnTitle += "\t";
        //        }
        //        columnTitle += dgv.Columns[i].HeaderText;
        //    }
        //    byte[] arr = Encoding.Default.GetBytes(columnTitle + "\n");
        //    myStream.Write(arr, 0, arr.Length);
        //    //sw.WriteLine(columnTitle);

        //    //写入列内容   
        //    for (int j = 0; j < dgv.Rows.Count; j++)
        //    {
        //        string columnValue = "";
        //        for (int k = 0; k < dgv.Columns.Count; k++)
        //        {
        //            if (k > 0)
        //            {
        //                columnValue += "\t";
        //            }
        //            if (dgv.Rows[j].Cells[k].Value == null)
        //                columnValue += "";
        //            else
        //                columnValue += "'" + dgv.Rows[j].Cells[k].Value.ToString();
        //        }
        //        byte[] arrValue = Encoding.Default.GetBytes(columnValue + "\n");
        //        myStream.Write(arrValue, 0, arr.Length);
        //        //sw.WriteLine(columnValue);
        //    }
        //    //sw.Close();
        //    myStream.Close();
        //}
        #endregion
    }

    /// <summary>
    /// Excel版本
    /// </summary>
    public enum ExcelVersion
    {
        /// <summary>
        /// Excel3.0版文档格式
        /// </summary>
        Excel3,
        /// <summary>
        /// Excel4.0版文档格式
        /// </summary>
        Excel4,
        /// <summary>
        /// Excel5.0版文档格式，适用于 Microsoft Excel 5.0 和 7.0 (95) 工作簿
        /// </summary>
        Excel5,
        /// <summary>
        /// Excel8.0版文档格式，适用于Microsoft Excel 8.0 (98-2003) 工作簿
        /// </summary>
        Excel8,
        /// <summary>
        /// Excel12.0版文档格式，适用于Microsoft Excel 12.0 (2007) 工作簿
        /// </summary>
        Excel12
    }
    public enum HeadRowType
    {
        /// <summary>
        /// HDR=Yes，这代表第一行是标题，不做为数据使用
        /// </summary>
        YES,
        /// <summary>
        /// HDR=NO，则表示第一行不是标题，做为数据来使用
        /// </summary>
        NO
    }

    /// <summary>
    /// 判断Excel是导入还是导出
    /// </summary>
    public enum ImportOrExportType
    {
        /// <summary>
        /// 将Excel数据导入程序
        /// </summary>
        Import,
        /// <summary>
        /// 将数据导出到Excel
        /// </summary>
        Export
    }

}
