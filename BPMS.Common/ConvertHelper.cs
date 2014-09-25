using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace BPMS.Common
{
    /// <summary>
    /// 常用数据类型转换类及方法，线程安全
    /// </summary>
    public class ConvertHelper
    {
        /// <summary>
        /// 将字符串string转换为整型int
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public int StringToInt(string str)
        {
            return StringToInt(str, 0);
        }

        /// <summary>
        /// 将字符串string转换为整型int
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <param name="defaultValue"></param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public int StringToInt(object vValue, int defaultValue)
        {
            if (IsNumeric(vValue))
            {
                string strTmp = vValue.ToString();
                if (strTmp.Contains("."))
                {
                    strTmp = strTmp.Substring(0, strTmp.IndexOf("."));
                }

                if (strTmp.Length > 15)
                {
                    strTmp = strTmp.Substring(0, 15);
                }
                return int.Parse(strTmp);
            }
            else
                return defaultValue;
        }

        /// <summary>
        /// 判断一个值是否为数字
        /// </summary>
        /// <param name="vValue"></param>
        /// <returns></returns>
        public static Boolean IsNumeric(object vValue)
        {
            if (vValue == null)
                return false;

            Regex digitregex = new Regex(@"^[0-9+-]\d*[.]?\d*$");
            if (digitregex.IsMatch(vValue.ToString()))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 将字符串string转换为无符号整型byte
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public byte StringToByte(string str)
        {
            if (str == null)
                return 0;
            else if (str == "")
                return 0;
            else
            {
                byte i;
                if (byte.TryParse(str, out i))
                    return i;
                else
                    return 0;
            }
        }

        /// <summary>
        /// 将字符串string转换为整型decimal
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public decimal StringToDecimal(string str)
        {
            if (str == null)
                return 0;
            else if (str == "")
                return 0;
            else
            {
                decimal d = 0;
                if (decimal.TryParse(str, out d)) return d;
                return 0;
            }
        }

        public static decimal ObjectToDecimal(object obj)
        {
            return StringToDecimal(ObjectToString(obj));
        }

        /// <summary>
        /// 将字符串string转换为整型double
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public double StringToDouble(string str)
        {
            if (str == null)
                return 0.0;
            else if (str == "")
                return 0.0;
            else
            {
                double d = 0.0;
                if (double.TryParse(str, out d)) return d;
                return 0;
            }
        }

        /// <summary>
        /// 将字符串string转换为布尔bool
        /// </summary>
        /// <param name="str">需要转换的字串，通常情况传True或False</param>
        /// <returns>如果转换成功，返回转换后的值，否则，返回假</returns>
        static public bool StringToBoolean(string str)
        {
            if (str == null)
                return false;
            else if (str == "")
                return false;
            else
            {
                bool b = false;
                if (bool.TryParse(str, out b)) return b;            //如果转换成功，返回转换后的值，否则，返回假
                return false;
            }
        }

        /// <summary>
        /// 把字符串转换成日期形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <param name="dtValue">传唤失败时返回的日期</param>
        /// <returns>返回转换后的日期</returns>
        static public DateTime StringToDateTime(string str, DateTime dtValue)
        {
            DateTime dt = new DateTime();
            if (DateTime.TryParse(str, out dt))
            {
                return dt;
            }
            else
            {
                return dtValue;
            }
        }
        /// <summary>
        /// 把字符串转换成日期形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>返回转换后的日期,如果失败返回日期类型最小值</returns>
        static public DateTime StringToDateTime(string str)
        {
            return StringToDateTime(str, DateTime.MinValue);
        }
        static public DateTime ObjectToDateTime(object obj)
        {
            return StringToDateTime(obj.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public DateTime? ObjToDateTime(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return StringToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为int
        /// </summary>
        /// <param name="obj"></param>
        static public int ObjectToInt(object obj)
        {
            return ObjectToInt(obj, 0);
        }

        /// <summary>
        /// 将对象转换为int
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        static public int ObjectToInt(object obj, int defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            return StringToInt(obj.ToString(), defaultValue);
        }

        /// <summary>
        /// 将对象转换为long
        /// </summary>
        /// <param name="obj"></param>
        static public long ObjectToLong(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            try
            {
                return System.Convert.ToInt64(obj);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将对象转换为short
        /// </summary>
        /// <param name="obj"></param>
        static public short ObjectToShort(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            try
            {
                return System.Convert.ToInt16(obj);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将对象转换为byte
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public byte ObjectToByte(object obj)
        {
            if (obj == null)
            {
                return (byte)0;
            }
            try
            {
                return System.Convert.ToByte(obj);
            }
            catch
            {
                return (byte)0;
            }
        }

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public string ObjectToString(object obj)
        {
            return ObjectToString(obj, "");
        }

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public string ObjectToString(object obj, string defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            return obj.ToString();
        }

        /// <summary>
        /// 将字符串转换为GUID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public Guid? StringToGuid(string obj)
        {
            Guid? rlt = null;
            if (!string.IsNullOrEmpty(obj))
            {
                try
                {
                    rlt = new Guid(obj);
                }
                catch
                { }
            }
            return rlt;
        }

        /// <summary>
        ///将16进制的字符串转换成字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 将字节转换成16进制的字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 批量转换类型
        /// </summary>
        /// <typeparam name="T1">源数据类型</typeparam>
        /// <typeparam name="T2">转换后类型</typeparam>
        /// <param name="source">源数据集合</param>
        /// <param name="defaultValue">转换默认值</param>
        /// <returns></returns>
        static public IEnumerable<T2> ConvertListType<T1, T2>(IEnumerable<T1> source, T2 defaultValue)
        {
            List<T2> rlt = new List<T2>();
            var list = source.ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                try
                {
                    rlt.Add((T2)Convert.ChangeType(list[i], typeof(T2)));
                }
                catch
                {
                    rlt.Add(defaultValue);
                }
            }
            return rlt;
        }
        /// <summary>
        /// 批量转换类型
        /// </summary>
        /// <typeparam name="T1">源数据类型</typeparam>
        /// <typeparam name="T2">转换后类型</typeparam>
        /// <param name="source">源数据集合</param>
        /// <returns></returns>
        static public IEnumerable<T2> ConvertListType<T1, T2>(IEnumerable<T1> source)
        {
            List<T2> rlt = new List<T2>();
            var list = source.ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                rlt.Add((T2)Convert.ChangeType(list[i], typeof(T2)));
            }
            return rlt;
        }

        #region 范型集合和DataSet之间的转换

        /// <summary>
        /// 数据集合转换成DataSet
        /// </summary>
        /// <param name="datas">数据集合转换成的Object数组</param>
        /// <returns></returns>
        public static DataSet ToDataSet(object[] datas)
        {
            DataSet result = new DataSet();
            DataTable _DataTable = ToDataTable(datas);//new DataTable();
            //if (datas.Length > 0)
            //{
            //    PropertyInfo[] propertys = datas[0].GetType().GetProperties();
            //    foreach (PropertyInfo pi in propertys)
            //    {
            //        if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
            //        {
            //            continue;
            //        }
            //        _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
            //    }

            //    for (int i = 0; i < datas.Length; i++)
            //    {
            //        ArrayList tempList = new ArrayList();
            //        foreach (PropertyInfo pi in propertys)
            //        {
            //            if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
            //            {
            //                continue;
            //            }
            //            object obj = pi.GetValue(datas[i], null);
            //            tempList.Add(obj);
            //        }
            //        object[] array = tempList.ToArray();
            //        _DataTable.LoadDataRow(array, true);
            //    }
            //}
            result.Tables.Add(_DataTable);
            return result;
        }

        /// <summary>
        /// 数据集合转化成DataTable
        /// Landry add at: 2010-12-08
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(object[] datas)
        {
            //by yang  2012-07-03
            DataTable _DataTable = new DataTable();
            _DataTable.TableName = "table1";
            if (datas.Length > 0)
            {
                PropertyInfo[] propertys = datas[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
                    {
                        continue;
                    }
                    _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
                }

                for (int i = 0; i < datas.Length; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
                        {
                            continue;
                        }
                        object obj = pi.GetValue(datas[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    _DataTable.LoadDataRow(array, true);
                }
            }
            return _DataTable;
            //if (datas == null) { return null; }
            //DataTable _DataTable = new DataTable();
            //if (datas.Length > 0)
            //{
            //    FieldInfo[] fieldInfos = datas[0].GetType().GetFields();
            //    foreach (FieldInfo field in fieldInfos)
            //    {
            //        _DataTable.Columns.Add(field.Name, GetNotNullableType(field.FieldType));
            //    }
            //    foreach (object obj in datas)
            //    {
            //        DataRow newRow = _DataTable.NewRow();
            //        foreach (FieldInfo field in fieldInfos)
            //        {
            //            newRow[field.Name] = field.GetValue(obj);
            //        }
            //        _DataTable.Rows.Add(newRow);
            //    }
            //}
            //return _DataTable;
        }

        /// <summary>
        /// 通过DataReader获取DataTable
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(IDataReader reader)
        {
            DataTable dtRlt = null;
            if (reader != null)
            {
                dtRlt = new DataTable();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    if (dtRlt.Columns.Contains(name))
                    {
                        name = name + i;
                    }
                    dtRlt.Columns.Add(new DataColumn(name, reader.GetFieldType(i)));
                }
                DataRow dr = null;
                while (reader.Read())
                {
                    dr = dtRlt.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            dr[i] = reader.GetValue(i);
                        }
                    }
                    dtRlt.Rows.Add(dr);
                }
                reader.Close();
            }
            return dtRlt;
        }

        /// <summary>
        /// 范型集合转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IList<T> datas) where T : class, new()
        {
            if (datas == null) { return null; }
            DataTable _DataTable = new DataTable();
            var t = typeof(T);
            bool isLinqOrEF = false;
            bool hasDataMember = false;
            var typeAttrs = t.GetCustomAttributes(false);
            //判断是否Linq2Sql或者EF实体对象
            if (typeAttrs.Any(attr => attr is System.Data.Linq.Mapping.TableAttribute) ||
                typeAttrs.Any(attr => attr is EdmEntityTypeAttribute))
            {
                isLinqOrEF = true;
            }
            var pis = t.GetProperties();
            if (!isLinqOrEF)
            {
                //不是Linq2Sql或者EF实体对象，判断对象是否存在DataMember标签的属性
                if (pis.Any(pi => pi.GetCustomAttributes(false).Any(attr => attr is DataMemberAttribute)))
                {
                    hasDataMember = true;
                }
            }
            if (isLinqOrEF)
            {
                //Linq2Sql或者EF实体对象，只序列化列属性
                pis = pis.Where(pi => pi.GetCustomAttributes(false).Any(attr => attr is System.Data.Linq.Mapping.ColumnAttribute ||
                            attr is EdmScalarPropertyAttribute)).ToArray();
            }
            else if (hasDataMember)
            {
                //存在DataMember标签属性的对象，只序列化标记为DataMember的属性
                pis = pis.Where(pi => pi.GetCustomAttributes(false).Any(attr => attr is DataMemberAttribute)).ToArray();
            }
            foreach (var pi in pis)
            {
                _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
            }
            foreach (var data in datas)
            {
                ArrayList tempList = new ArrayList();
                foreach (var pi in pis)
                {
                    tempList.Add(pi.GetValue(data, null));
                }
                _DataTable.LoadDataRow(tempList.ToArray(), true);
            }
            return _DataTable;
        }

        /// <summary>
        /// 数据集合转化成泛型 List
        /// Landry add at: 2010-12-08
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static List<T> ObjectToIList<T>(object[] datas) where T : class
        {
            if (datas == null) { return null; }
            Type targetType = typeof(T);
            PropertyInfo[] targetPropertyInfos = targetType.GetProperties();
            FieldInfo[] objFieldInfos = datas[0].GetType().GetFields();
            List<T> resultList = new List<T>();
            foreach (object obj in datas)
            {
                T targetObj = (T)Activator.CreateInstance(typeof(T));
                foreach (FieldInfo field in objFieldInfos)
                {
                    PropertyInfo pi = targetPropertyInfos.SingleOrDefault(p => p.Name == field.Name);
                    if (pi != null)
                    {
                        pi.SetValue(targetObj, field.GetValue(obj), null);
                    }
                }
                resultList.Add(targetObj);
            }
            return resultList;
        }

        /// <summary>
        /// 泛型集合转换DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">泛型集合</param>
        /// <returns></returns>
        public static DataSet IListToDataSet<T>(IList<T> list) where T : class
        {
            return IListToDataSet<T>(list, null);
        }


        /// <summary>
        /// 泛型集合转换DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_List">泛型集合</param>
        /// <param name="p_PropertyName">待转换属性名数组</param>
        /// <returns></returns>
        public static DataSet IListToDataSet<T>(IList<T> p_List, params string[] p_PropertyName) where T : class
        {
            List<string> propertyNameList = new List<string>();
            if (p_PropertyName != null)
                propertyNameList.AddRange(p_PropertyName);

            DataSet result = new DataSet();
            DataTable _DataTable = new DataTable();
            if (p_List.Count > 0)
            {
                PropertyInfo[] propertys = p_List[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
                    {
                        continue;
                    }
                    if (propertyNameList.Count == 0)
                    {
                        // 没有指定属性的情况下全部属性都要转换
                        _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
                    }
                }

                for (int i = 0; i < p_List.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
                        {
                            continue;
                        }
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(p_List[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(p_List[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    _DataTable.LoadDataRow(array, true);
                }
            }
            result.Tables.Add(_DataTable);
            return result;
        }

        /// <summary>
        /// DataSet装换为泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_DataSet">DataSet</param>
        /// <param name="p_TableIndex">待转换数据表索引</param>
        /// <returns></returns>
        public static IList<T> DataSetToIList<T>(DataSet p_DataSet, int p_TableIndex) where T : class
        {
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (p_TableIndex > p_DataSet.Tables.Count - 1)
                return null;
            if (p_TableIndex < 0)
                p_TableIndex = 0;

            DataTable p_Data = p_DataSet.Tables[p_TableIndex];
            // 返回值初始化
            IList<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }

        /// <summary>
        /// DataSet装换为泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_DataSet">DataSet</param>
        /// <param name="p_TableName">待转换数据表名称</param>
        /// <returns></returns>
        public static IList<T> DataSetToIList<T>(DataSet p_DataSet, string p_TableName) where T : class
        {
            int _TableIndex = 0;
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (string.IsNullOrEmpty(p_TableName))
                return null;
            for (int i = 0; i < p_DataSet.Tables.Count; i++)
            {
                // 获取Table名称在Tables集合中的索引值
                if (p_DataSet.Tables[i].TableName.Equals(p_TableName))
                {
                    _TableIndex = i;
                    break;
                }
            }
            return DataSetToIList<T>(p_DataSet, _TableIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_type"></param>
        /// <returns></returns>
        public static Type GetNotNullableType(Type p_type)
        {
            if (p_type == typeof(Int16?))
            {
                return typeof(Int16);
            }
            if (p_type == typeof(Int32?))
            {
                return typeof(Int32);
            }
            if (p_type == typeof(Int64?))
            {
                return typeof(Int64);
            }
            if (p_type == typeof(decimal?))
            {
                return typeof(decimal);
            }
            if (p_type == typeof(double?))
            {
                return typeof(double);
            }
            if (p_type == typeof(DateTime?))
            {
                return typeof(DateTime);
            }
            if (p_type == typeof(Boolean?))
            {
                return typeof(Boolean);
            }
            if (p_type == typeof(Guid?))
            {
                return typeof(Guid);
            }
            if (p_type == typeof(byte?))
            {
                return typeof(byte);
            }
            if (p_type == typeof(float?))
            {
                return typeof(float);
            }
            return p_type;
        }
        #endregion

        /// <summary>
        /// 获取匿名类的值
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static object GetObjectValue(object Source, string ColumnName)
        {
            object objRlt = null;
            try
            {
                objRlt = Source.GetType().GetProperty(ColumnName).GetValue(Source, null);
            }
            catch
            { }
            return objRlt;
        }

        /// <summary>
        /// 设置匿名类的值
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="ColumnName"></param>
        /// <param name="Value"></param>
        public static void SetObjectValue(object Source, string ColumnName, object Value)
        {
            System.Reflection.PropertyInfo pi = Source.GetType().GetProperty(ColumnName);
            if (pi != null)
            {
                pi.SetValue(Source, Value, null);
            }
        }


        //#region Model与XML相互转换
        //public static List<T> XmlToModels<T>(string lstXML) where T : class, new()
        //{
        //    if (String.IsNullOrEmpty(lstXML))
        //        return new List<T>();

        //    List<T> lstModel = new List<T>();
        //    foreach (XElement objElem in XElement.Parse(lstXML).Elements())
        //    {
        //        T objModel = XmlToModel<T>(objElem.ToString());
        //        lstModel.Add(objModel);
        //    }
        //    return lstModel;
        //}
        //public static T XmlToModel<T>(string xml) where T : class, new()
        //{
        //    if (String.IsNullOrEmpty(xml))
        //        return null;
        //    T objModel = new T();
        //    CommonHelper.Replace<T>(ref objModel, XElement.Parse(xml));
        //    return objModel;

        //    //T objModel = new T();

        //    //XElement elemRoot = XElement.Parse(xml);

        //    //var arrProperty = typeof(T).GetProperties();
        //    //foreach (PropertyInfo objPro in arrProperty)
        //    //{
        //    //    if (objPro.GetCustomAttributes(false).Any(t =>
        //    //        t is System.Data.Objects.DataClasses.EdmRelationshipNavigationPropertyAttribute ||
        //    //        t is System.ComponentModel.BrowsableAttribute))
        //    //    {
        //    //        continue;
        //    //    }
        //    //    try
        //    //    {
        //    //        object value = null;
        //    //        string elemValue = elemRoot.Element(objPro.Name).Value;
        //    //        if (elemValue != "")
        //    //        {
        //    //            switch (objPro.PropertyType.Name)
        //    //            {
        //    //                case "Binary":
        //    //                    value = new Binary(Convert.FromBase64String(elemValue));
        //    //                    break;
        //    //                default:
        //    //                    value = Convert.ChangeType(elemValue, GetNotNullableType(objPro.PropertyType));
        //    //                    break;
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            if (objPro.PropertyType.IsValueType)
        //    //                value = Activator.CreateInstance(objPro.PropertyType);
        //    //            else if (objPro.PropertyType == typeof(string))
        //    //                value = "";
        //    //            else
        //    //                value = null;
        //    //        }
        //    //        objPro.SetValue(objModel, value, null);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        if (ignore)
        //    //            continue;
        //    //        else
        //    //            throw ex;
        //    //    }
        //    //}
        //    //return objModel;
        //}
        //public static string ModelsToXML<T>(List<T> lstModel) where T : class, new()
        //{
        //    string rootName = typeof(T).Name + "List";
        //    XElement objRootElem = new XElement(rootName);
        //    foreach (T objModel in lstModel)
        //    {
        //        if (objModel != null)
        //        {
        //            string xml = ModelToXML(objModel);
        //            objRootElem.Add(XElement.Parse(xml));
        //        }
        //    }
        //    return objRootElem.ToString();
        //}
        //public static string ModelToXML<T>(T objModel) where T : class, new()
        //{
        //    if (objModel == null)
        //        return "";
        //    return CommonHelper.GetXml<T>(objModel, typeof(T).Name, null).ToString();

        //    //XElement elemRoot = new XElement(typeof(T).Name);
        //    //var arrProperty = typeof(T).GetProperties();
        //    //foreach (PropertyInfo objPro in arrProperty)
        //    //{
        //    //    if (objPro.GetCustomAttributes(false).Any(t =>
        //    //        t is System.Data.Objects.DataClasses.EdmRelationshipNavigationPropertyAttribute ||
        //    //        t is System.ComponentModel.BrowsableAttribute))
        //    //    {
        //    //        continue;
        //    //    }
        //    //    object value = objPro.GetValue(objModel, null);
        //    //    if (value != null)
        //    //    {
        //    //        switch (objPro.PropertyType.Name)
        //    //        {
        //    //            case "Binary":
        //    //                var arrByte = ((Binary)value).ToArray();
        //    //                value = Convert.ToBase64String(arrByte);
        //    //                break;
        //    //            default:

        //    //                break;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        value = "";
        //    //    }
        //    //    elemRoot.Add(new XElement(objPro.Name, value));
        //    //}
        //    //return elemRoot.ToString();
        //}
        //#endregion

        #region 将二进制转化为图片
        /// <summary>
        /// 将二进制转化为图片
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
        }
        #endregion

        #region 序列化 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Object UnSerialize(string str)
        {
            try
            {
                //反序列化
                IFormatter formatter = new BinaryFormatter();


                byte[] b = Convert.FromBase64String(str);

                MemoryStream ms = new MemoryStream(b);

                Object obj = formatter.Deserialize(ms);

                return obj;
            }
            catch
            {
                throw new Exception("对象反序列化失败");
            }
        }

        /// <summary>
        /// 将对象序例化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(Object obj)
        {
            string ret = "";

            try
            {
                //序列化
                IFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                formatter.Serialize(ms, obj);

                byte[] byte_obj = ms.GetBuffer();


                ret = Convert.ToBase64String(byte_obj);
            }
            catch
            {
                throw new Exception("对象序列化失败");
            }

            return ret;
        }


        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns></returns>
        public static string XMLSerialize<T>(T t)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(t.GetType());
                xz.Serialize(sw, t);
                return sw.ToString();
            }
        }

        /// <summary>
        /// 反序列化为对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="s">对象序列化后的Xml字符串</param>
        /// <returns></returns>
        public static object XMLDeserialize(Type type, string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                XmlSerializer xz = new XmlSerializer(type);
                return xz.Deserialize(sr);
            }
        }
        #endregion

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
    }
}
