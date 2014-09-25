using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Data;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Linq.Expressions;
using System.Data.Objects.DataClasses;
using System.Collections;
using System.Data.Linq;
using System.Web;
using System.ComponentModel;

namespace BPMS.Common
{
    /// <summary>
    /// 公用方法库
    /// </summary>
    public sealed class CommonHelper
    {
        #region Clone..

        /// <summary>
        /// 返回clone的实体给调用方
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="obj">T类型</param>
        /// <returns>返回T类型的新实体</returns>        
        public static T Clone<T>(T obj) where T : class
        {
            try
            {
                Type t = obj.GetType();
                T newObj = (T)Activator.CreateInstance(t);

                foreach (PropertyInfo pi in t.GetProperties())
                {
                    try
                    {
                        pi.SetValue(newObj, pi.GetValue(obj, null), null);
                    }
                    catch { }
                }
                foreach (FieldInfo fi in t.GetFields())
                {
                    try
                    {
                        fi.SetValue(newObj, fi.GetValue(obj));
                    }
                    catch { }
                }

                return newObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Copy/Clone方法，将Source的数据拷贝到Target对象中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Clone<T>(T source, T target) where T : class
        {
            try
            {
                Type t = source.GetType();

                if (target == null)
                { return; }

                foreach (PropertyInfo pi in t.GetProperties())
                {
                    try
                    {
                        pi.SetValue(target, pi.GetValue(source, null), null);
                    }
                    catch { }
                }
                foreach (FieldInfo fi in t.GetFields())
                {
                    try
                    {
                        fi.SetValue(target, fi.GetValue(source));
                    }
                    catch { }
                }

                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Copy指定列的数据新对象
        /// </summary>
        /// <typeparam name="T">对象类型，仅限于类</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="target">新对象，传入对象必须已实例化</param>
        /// <param name="propertyNames">Copy列数组</param>
        public static void Clone<T>(T source, T target, string[] propertyNames) where T : class
        {
            try
            {
                Type t = source.GetType();

                if (target == null)
                { return; }

                foreach (PropertyInfo pi in t.GetProperties())
                {
                    try
                    {
                        if (propertyNames.Where(o => o.Trim().ToLower() == pi.Name.Trim().ToLower()).Count() > 0)
                        {
                            pi.SetValue(target, pi.GetValue(source, null), null);
                        }

                    }
                    catch { }
                }
                foreach (FieldInfo fi in t.GetFields())
                {
                    try
                    {
                        if (propertyNames.Where(o => o.Trim().ToLower() == fi.Name.Trim().ToLower()).Count() > 0)
                        {
                            fi.SetValue(target, fi.GetValue(source));
                        }
                    }
                    catch { }
                }

                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 两个不同类型的对象数据复制
        /// </summary>
        /// <typeparam name="TSource">数据对象类</typeparam>
        /// <typeparam name="TTarget">要赋值的对象类</typeparam>
        /// <param name="source">数据对象</param>
        /// <returns></returns>
        public static TTarget Clone<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class
        {
            if (source == null)
            {
                return null;
            }

            try
            {
                Type tSource = source.GetType();
                Type tTarget = typeof(TTarget);

                // 催化剂实例对象
                TTarget target = (TTarget)Activator.CreateInstance(tTarget);

                PropertyInfo sProperty;
                foreach (PropertyInfo pi in tTarget.GetProperties())
                {
                    try
                    {
                        sProperty = tSource.GetProperty(pi.Name);

                        pi.SetValue(target, sProperty.GetValue(source, null), null);
                    }
                    catch { }
                }
                FieldInfo sfield;
                foreach (FieldInfo fi in tTarget.GetFields())
                {
                    try
                    {
                        sfield = tSource.GetField(fi.Name);

                        fi.SetValue(target, sfield.GetValue(source));
                    }
                    catch { }
                }
                return target;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 两个不同类型同属性的对象数组复制
        /// </summary>
        /// <param name="arrSourceObj">数据源数组对象</param>
        /// <returns>返回的指定类型的数组对象</returns>
        public static TTarget[] CloneArrayObject<TSource, TTarget>(TSource[] arrSourceObj)
            where TSource : class
            where TTarget : class
        {
            if (arrSourceObj == null)
            {
                return null;
            }
            //复制返回的数组TTarget Clone<TSource, TTarget>(TSource source) where TSource : class where TTarget :class
            List<TTarget> iList = new List<TTarget>();

            for (int i = 0; i < arrSourceObj.Length; i++)
            {
                TTarget newObj;
                newObj = CommonHelper.Clone<TSource, TTarget>(arrSourceObj[i]);
                iList.Add(newObj);
            }

            return iList.ToArray<TTarget>();
        }

        #endregion

        #region 反映处理 ..

        /// <summary>
        /// 通过属性名，获得实例中对应属性名的值
        /// </summary>
        /// <typeparam name="T">实例类型，必须为类</typeparam>
        /// <param name="t">实例</param>
        /// <param name="fieldName">属性名</param>
        /// <returns></returns>
        public static object GetValue<T>(T t, string fieldName) where T : class
        {
            try
            {
                if (fieldName.Trim().Length < 1) return null;

                Type type = t.GetType();

                object obj = null;

                PropertyInfo pi;
                pi = type.GetProperty(fieldName.Trim());
                if (pi == null) pi = type.GetProperty(fieldName.Trim().ToLower());

                if (pi != null)
                {
                    obj = pi.GetValue(t, null);
                    if (obj != null)
                    {
                        switch (pi.PropertyType.Name)
                        {
                            case "Binary":
                                var arrByte = ((Binary)obj).ToArray();
                                obj = Convert.ToBase64String(arrByte);
                                break;
                            case "Byte[]":
                                obj = Convert.ToBase64String((byte[])obj);
                                break;
                            default:

                                break;
                        }
                    }
                }
                else
                {
                    // 获取字段的值                    
                    FieldInfo fi;
                    fi = type.GetField(fieldName.Trim());
                    if (fi == null) fi = type.GetField(fieldName.Trim().ToLower());
                    if (fi != null)
                    {
                        obj = fi.GetValue(t);
                    }
                }

                // 没有对应属性
                return obj;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 通过属性名，获得ORM实例中对应属性名的值
        /// </summary>
        /// <typeparam name="T">实例类型，必须为类</typeparam>
        /// <param name="t">实例</param>
        /// <param name="fieldName">属性名</param>
        /// <returns></returns>
        public static object GetTableValue<T>(T t, string fieldName) where T : class
        {
            try
            {
                if (fieldName.Trim().Length < 1) return string.Empty;

                Type type = t.GetType();

                fieldName = fieldName.Trim().ToLower();
                foreach (PropertyInfo pi in type.GetProperties())
                {
                    if (pi.Name.Trim().ToLower() != fieldName)
                    { continue; }

                    object[] attrs = pi.GetCustomAttributes(false);
                    foreach (object attr in attrs)
                    {
                        if (attr is System.Data.Linq.Mapping.ColumnAttribute)
                        {
                            System.Data.Linq.Mapping.ColumnAttribute column = (System.Data.Linq.Mapping.ColumnAttribute)attr;

                            if (column.Storage.Trim().ToLower() == "_" + fieldName)
                            { return pi.GetValue(t, null); }
                        }
                    }
                }

                // 没有任何条件
                return null;
            }
            catch
            { return null; }
        }

        /// <summary>
        /// 获取实体对应表的字段列，只取实体对应数据库表中的列串
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetTableFieldList(Type t)
        {
            try
            {
                if (t == null) return string.Empty;

                StringBuilder sb = new StringBuilder();

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
                foreach (PropertyInfo pi in pis)
                {
                    if (sb.Length > 0)
                    { sb.Append("|"); }

                    // 追加属性名
                    sb.Append(pi.Name);
                }

                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region List..

        /// <summary>
        /// 获取List中符合条件的条一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstInfos"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T First<T>(List<T> lstInfos, string fieldName, object value) where T : class
        {
            if (lstInfos == null || lstInfos.Count == 0) return null;

            PropertyInfo pi = null;
            foreach (T info in lstInfos)
            {
                try
                {
                    // 获取属性
                    if (pi == null)
                    { pi = info.GetType().GetProperty(fieldName); }

                    // 获取属性值是否与结点中值一致，如一致返回实例
                    if (pi.GetValue(info, null).ToString() == value.ToString())
                    { return info; }
                }
                catch { }
            }

            return null;
        }

        #endregion

        #region Xml方法..

        #region 获取xml结点名字串..

        /// <summary>
        /// 获取xmlNode二级结点的字符串
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string GetNodeNameList(XElement xmlNode)
        {
            return CommonHelper.GetNodeNameList(xmlNode, "|");
        }

        /// <summary>
        /// 只能是二级结点的xml结点
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="sSplit"></param>
        /// <returns></returns>
        public static string GetNodeNameList(XElement xmlNode, string sSplit)
        {
            try
            {
                if (xmlNode == null) return "";

                StringBuilder sb = new StringBuilder();

                foreach (XElement item in xmlNode.Elements())
                {
                    if (sb.Length > 0) sb.Append(sSplit);
                    sb.Append(item.Name);
                }

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region 获取xml结点..

        /// <summary>
        /// 通过结点名和值获取结点信息
        /// </summary>
        /// <param name="XName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XElement GetXElement(string XName, object value)
        {
            try
            {
                return new XElement(XName.Trim(), value);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将xml字符串
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XElement GetXElementByString(string xml)
        {
            try
            {
                //// 把xml字符串转换成结点
                //Stream stream = new MemoryStream(Encoding.Default.GetBytes(xml));
                //XmlReader reader = XmlReader.Create(stream);
                //XElement xmlroot = XElement.Load(reader);

                //return xmlroot;

                return GetXElementByString(xml, Encoding.Default);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将xml字符串
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static XElement GetXElementByString(string xml, Encoding encoding)
        {
            try
            {
                if (!xml.ToLower().StartsWith("<?xml version"))
                { xml = GetXmlString(xml, encoding); }

                // 把xml字符串转换成结点
                Stream stream = new MemoryStream(encoding.GetBytes(xml));
                XmlReader reader = XmlReader.Create(stream);
                XElement xmlroot = XElement.Load(reader);

                return xmlroot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打开xml文件，生成XElement对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XElement GetXElementByFile(string path)
        {
            try
            {
                if (!File.Exists(path)) return null;

                // 把xml字符串转换成结点
                Stream stream = new FileStream(path, FileMode.Open);
                XmlReader reader = XmlReader.Create(stream);
                XElement xml = XElement.Load(reader);

                return xml;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 通过DataTable生成xml结点
        /// </summary>
        /// <param name="dt">datatable生成xml结点</param>
        /// <param name="rowName">行的名称</param>
        /// <returns></returns>
        public static XElement GetXElementByDataTable(DataTable dt, string rowName)
        {
            try
            {
                return GetXElementByDataTable(dt, rowName, "Root");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 通过DataTable生成xml结点
        /// </summary>
        /// <param name="dt">datatable生成xml结点</param>
        /// <param name="rowName">行的名称</param>
        /// <param name="rootName">根结点名称</param>
        /// <returns></returns>
        public static XElement GetXElementByDataTable(DataTable dt, string rowName, string rootName)
        {
            try
            {
                if (dt == null) return null;
                if (dt.Rows.Count == 0) return null;

                if (rowName.Trim().Length == 0) rowName = "DataRow";
                if (rootName.Trim().Length == 0) rootName = "Root";

                XElement xmlRoot = new XElement(rootName);
                XElement xmlRow;
                foreach (DataRow row in dt.Rows)
                {
                    xmlRow = new XElement(rowName);

                    foreach (DataColumn col in dt.Columns)
                    {
                        xmlRow.Add(new XElement(col.ColumnName, row[col.ColumnName].ToString()));
                    }

                    xmlRoot.Add(xmlRow);
                }

                return xmlRoot;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 获取XML结点..

        /// <summary>
        /// 返回实体对应列的xml结点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="NodeName"></param>
        /// <param name="sFields"></param>
        /// <returns></returns>
        public static XElement GetXml<T>(T entity, string NodeName, string[] sFields) where T : class
        {
            try
            {
                return GetXml<T>(entity, NodeName, sFields, false);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取实体对应列的xml，实体列与xml结点列可不同
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体实例</param>
        /// <param name="NodeName">结点名</param>
        /// <param name="sFields">实体列</param>
        /// <param name="isMapping">实体列与xml结点是否有映射，默认false；
        /// isMapping为false时，xml结点和实体列一致；
        /// isMapping为true时，strFidlds保存列表和结点名，用“,”分隔开，如"FieldName,NodeName"</param>
        /// <returns></returns>
        public static XElement GetXml<T>(T entity, string NodeName, string[] sFields, bool isMapping) where T : class
        {
            try
            {
                if (entity == null) return null;

                // 只有一个并为空时，获取数据库列
                if (sFields == null || sFields.Length == 0 || (sFields.Length == 1 && sFields[0].Trim().Length == 0))
                { sFields = CommonHelper.GetTableFieldList(entity.GetType()).Split('|'); }

                XElement xmlInfo = new XElement(NodeName);

                string tempNodeName = string.Empty;
                object tempValue = null;

                #region 未传入列集合时，直接返回实体所有属性的XML值..

                // 未传入列时，返回实体的Xml
                Type type = entity.GetType();
                if (sFields == null || sFields.Length == 0)
                {
                    foreach (PropertyInfo pi in type.GetProperties())
                    {
                        tempNodeName = pi.Name;
                        tempValue = pi.GetValue(entity, null);

                        xmlInfo.Add(new XElement(tempNodeName, tempValue == null ? "" : tempValue.ToString()));
                    }

                    // 直接返回
                    return xmlInfo;
                }

                #endregion

                // 遍历列集合，生成Xml
                for (int i = 0; i < sFields.Length; i++)
                {
                    // 设置结点名
                    if (!isMapping)
                    {
                        tempNodeName = sFields[i].Trim();
                    }
                    else
                    {
                        // 只有一个值或者第二项值为空
                        if (sFields[i].Trim().Split(',').Length == 1
                            || (sFields[i].Trim().Split(',').Length == 1
                                && sFields[i].Trim().Split(',')[0].Trim().Length == 0))
                        { tempNodeName = sFields[i].Trim(); }
                        else
                        { tempNodeName = sFields[i].Trim().Split(',')[1].Trim(); }
                    }

                    tempValue = CommonHelper.GetValue(entity, tempNodeName);

                    // 生成结点信息
                    xmlInfo.Add(new XElement(tempNodeName, tempValue == null ? "" : tempValue.ToString()));
                }

                return xmlInfo;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取差异xml
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="compareEntity">当前对比对象</param>
        /// <param name="baseObj">基础对象</param>
        /// <param name="nodeName">结点名</param>
        /// <param name="sFields">字段名</param>
        /// <returns></returns>
        public static XElement GetXml<T>(T compareEntity, T baseEntity, string nodeName, string[] sFields) where T : class
        {
            try
            {
                if (compareEntity == null) return null;
                if (baseEntity == null) return null;
                if (sFields.Length == 0) return null;

                if (nodeName.Trim().Length == 0) nodeName = "Root";

                XElement xNode = null;

                Type type = compareEntity.GetType();
                foreach (PropertyInfo pi in type.GetProperties())
                {
                    if (sFields.FirstOrDefault(f => f.Trim().ToLower() == pi.Name.Trim().ToLower()) == null)
                    { continue; }

                    if (pi.GetValue(compareEntity, null) != pi.GetValue(baseEntity, null))
                    {
                        if (xNode == null) xNode = new XElement(nodeName);
                        xNode.Add(GetXElement(pi.Name, pi.GetValue(compareEntity, null)));
                    }
                }

                return xNode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region DataTable转换成Xml..

        /// <summary>
        /// 通过DataTable获取xml结点
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowNodeName"></param>
        /// <returns></returns>
        public static XElement GetXml(DataTable dt, string rowNodeName)
        {
            return GetXml(dt, rowNodeName, null, false);
        }

        /// <summary>
        /// 通过DataTable获取xml结点
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootNodeName"></param>
        /// <param name="rowNodeName"></param>
        /// <returns></returns>
        public static XElement GetXml(DataTable dt, string rootNodeName, string rowNodeName)
        {
            return GetXml(dt, rootNodeName, rowNodeName, null, false);
        }

        /// <summary>
        /// 通过DataTable获取xml结点
        /// </summary>
        /// <param name="dt">DataTable数据</param>
        /// <param name="rowNodeName">每行对应生成结点的nodeName</param>
        /// <param name="sFields">字段列</param>
        /// <param name="isMapping">实体列与xml结点是否有映射，默认false；
        /// isMapping为false时，xml结点和实体列一致；
        /// isMapping为true时，strFidlds保存列表和结点名，用“,”分隔开，如"FieldName,NodeName"</param>
        /// <returns></returns>
        public static XElement GetXml(DataTable dt, string rowNodeName, string[] sFields, bool isMapping)
        {
            return GetXml(dt, "root", rowNodeName, sFields, isMapping);
        }

        /// <summary>
        /// 通过DataTable获取xml结点
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootNodeName">根结点名</param>
        /// <param name="rowNodeName">DataRow对应结点名</param>
        /// <param name="sFields"></param>
        /// <param name="isMapping">实体列与xml结点是否有映射，默认false；
        /// isMapping为false时，xml结点和实体列一致；
        /// isMapping为true时，strFidlds保存列表和结点名，用“,”分隔开，如"FieldName,NodeName"</param>
        /// <returns></returns>
        public static XElement GetXml(DataTable dt, string rootNodeName, string rowNodeName, string[] sFields, bool isMapping)
        {
            try
            {
                if (dt == null) return null;

                // 只有一个并为空时
                if (sFields == null || sFields.Length == 0 || (sFields.Length == 1 && sFields[0].Trim().Length == 0))
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (sb.Length > 0) sb.Append("|");
                        sb.Append(column.ColumnName);

                        // 增加对应实体列
                        if (isMapping) sb.Append("," + column.ColumnName);
                    }

                    sFields = sb.ToString().Split('|');
                }

                XElement xmlRoot = new XElement(rootNodeName);

                XElement xmlNode;
                foreach (DataRow dr in dt.Rows)
                {
                    xmlNode = GetXml(dr, rowNodeName, sFields, isMapping);

                    if (xmlNode != null) xmlRoot.Add(xmlNode);
                }

                return xmlRoot;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 通过DataRow获取XElement结点
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="NodeName"></param>
        /// <param name="sFields"></param>
        /// <param name="isMapping">实体列与xml结点是否有映射，默认false；
        /// isMapping为false时，xml结点和实体列一致；
        /// isMapping为true时，strFidlds保存列表和结点名，用“,”分隔开，如"FieldName,NodeName"</param>
        /// <returns></returns>
        public static XElement GetXml(DataRow dr, string NodeName, string[] sFields, bool isMapping)
        {
            try
            {
                if (dr == null) return null;

                // 只有一个并为空时
                if (sFields.Length == 0 || (sFields.Length == 1 && sFields[0].Trim().Length == 0))
                { return null; }

                XElement xmlNode = new XElement(NodeName);

                if (!isMapping)
                {
                    for (int i = 0; i < sFields.Length; i++)
                    {
                        if (sFields[i].Trim().Length == 0) continue;

                        object obj = dr[sFields[i]];

                        xmlNode.Add(new XElement(sFields[i], obj == null ? "" : obj.ToString()));
                    }
                }
                else
                {
                    for (int i = 0; i < sFields.Length; i++)
                    {
                        if (sFields[i].Trim().Length == 0) continue;

                        object obj = dr[sFields[i]];
                        if (sFields[i].Trim().Split(',').Length == 1)
                        { xmlNode.Add(new XElement(sFields[i], obj == null ? "" : obj.ToString())); }
                        else
                        { xmlNode.Add(new XElement(sFields[i].Trim().Split(',')[1], obj == null ? "" : obj.ToString())); }
                    }
                }

                return xmlNode;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// 保存XElement对象到文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string SaveXElementToFile(string path, XElement xmlNode)
        {
            try
            {
                if (!File.Exists(path)) return null;

                // 把xml字符串转换成结点
                StreamWriter sw = File.CreateText(path);
                sw.Write(xmlNode.ToString());

                sw.Close();

                return "";
            }
            catch (Exception ex)
            {
                return "99|" + ex.Message;
            }
        }

        /// <summary>
        /// 批量增加XElement结点
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="xmlSubNodes"></param>
        public static void AddRange(XElement xmlNode, List<XElement> xmlSubNodes)
        {
            if (xmlNode == null) return;
            if (xmlSubNodes == null || xmlSubNodes.Count == 0) return;

            foreach (XElement item in xmlSubNodes)
            { xmlNode.Add(item); }

            return;
        }

        #region 获取xml字符串..

        /// <summary>
        /// 把结点转换成string类型，并将编码设置为“GB2312”
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string GetXmlString(XElement xmlNode)
        {
            try
            {
                if (xmlNode == null) return "";

                return GetXmlString(xmlNode.ToString());
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 把结点转换成string类型，并将编码设置为“GB2312”
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string GetXmlString(string xml)
        {
            try
            {
                if (xml.Length == 0) return "";

                return "<?xml version=\"1.0\" encoding=\"GB2312\" ?> " + xml;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取xml结点的xml字符串
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetXmlString(XElement xmlNode, Encoding encoding)
        {
            try
            {
                if (xmlNode == null) return "";

                return GetXmlString(xmlNode.ToString(), encoding);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 按一定编码生成xml字符串
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetXmlString(string xml, Encoding encoding)
        {
            try
            {
                if (xml.Length == 0) return "";

                return "<?xml version=\"1.0\" encoding=\"" + encoding.HeaderName + "\" ?> " + xml;
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region xml/实体操作（替换）..

        /// <summary>
        /// 用xml结点的内容替换实例，返回当前实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="xmlNode"></param>
        public static void Replace<T>(ref T entity, XElement xmlNode) where T : class
        {
            if (entity == null) return;

            CommonHelper.Replace(ref entity, xmlNode, null);

            return;
        }

        /// <summary>
        /// 用xml结点指定列的内容替换实例，返回当前实例，如果列为空，替换所有结点内容；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="xmlNode"></param>
        /// <param name="fields"></param>
        public static void Replace<T>(ref T entity, XElement xmlNode, string[] fields) where T : class
        {
            try
            {
                if (entity == null) return;
                if (xmlNode == null) return;

                // 如果列数组为空，返回整个替换整个xml结点；
                if (fields == null || fields.Length == 0)
                {
                    // 获取XML结点中的结点名并生成替换列
                    fields = GetNodeNameList(xmlNode).Split('|');
                }

                XElement item;
                PropertyInfo pi;
                FieldInfo fi;
                Type type = entity.GetType();

                for (int i = 0; i < fields.Length; i++)
                {
                    try
                    {
                        item = xmlNode.Element(fields[i]);
                        // 默认兼容小写
                        if (item == null) item = xmlNode.Element(fields[i].ToLower());
                        if (item != null)
                        {
                            pi = type.GetProperty(fields[i]);
                            if (pi != null)
                            {
                                object itemValue = "";
                                switch (pi.PropertyType.Name)
                                {
                                    case "Binary":
                                        itemValue = new Binary(Convert.FromBase64String(item.Value));
                                        break;
                                    case "Byte[]":
                                        itemValue = Convert.FromBase64String(item.Value);
                                        break;
                                    default:
                                        itemValue = (object)ConvertByType(pi.PropertyType, item.Value);
                                        break;
                                }
                                pi.SetValue(entity, itemValue, null);
                            }
                            else
                            {
                                fi = type.GetField(fields[i]);
                                if (fi == null) continue;

                                object itemValue = "";
                                switch (fi.FieldType.Name)
                                {
                                    case "Binary":
                                        itemValue = new Binary(Convert.FromBase64String(item.Value));
                                        break;
                                    case "Byte[]":
                                        itemValue = Convert.FromBase64String(item.Value);
                                        break;
                                    default:
                                        itemValue = (object)ConvertByType(fi.FieldType, item.Value);
                                        break;
                                }
                                fi.SetValue(entity, itemValue);
                                //fi.SetValue(entity, (object)ConvertByType(fi.FieldType, item.Value));
                            }
                        }

                    }
                    catch { }
                }

                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用xml结点的内容替换实例，返回实体与传入实体共址
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static T Replace<T>(T entity, XElement xmlNode) where T : class
        {
            // 调用基本方法
            return CommonHelper.Replace(entity, xmlNode, null);
        }

        /// <summary>
        /// 替换方法，返回实体与传入实体共址
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="xmlNode"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static T Replace<T>(T entity, XElement xmlNode, string[] fields) where T : class
        {
            CommonHelper.Replace(ref entity, xmlNode, fields);

            return entity;
        }

        #endregion


        /// <summary>
        /// 转换二级结点实体列表，二级结点的NodeName需与实体Property一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlNode"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(XElement xmlNode, string nodeName) where T : class
        {
            try
            {
                if (xmlNode == null) return null;

                T obj;
                Type t = typeof(T);
                List<T> lst = new List<T>();

                // 取配送结点
                IEnumerable<XElement> itemNodes = from itemNode in xmlNode.Elements(nodeName)
                                                  select itemNode;
                foreach (XElement itemNode in itemNodes)
                {
                    obj = (T)Activator.CreateInstance(t);

                    lst.Add(CommonHelper.Replace<T>(obj, itemNode));
                }

                return lst;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Convert By Type..

        /// <summary>
        /// 将字符串转换成对应类型的数据
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConvertByType(Type t, string value)
        {
            try
            {
                if (value == null) return null;

                if (t.FullName == typeof(String).FullName)
                { return value; }
                if (t.FullName == typeof(Boolean).FullName || t == typeof(Nullable<Boolean>))
                { return Boolean.Parse(value); }
                if (t.FullName == typeof(Byte).FullName || t == typeof(Nullable<Byte>))
                { return Byte.Parse(value); }
                if (t.FullName == typeof(SByte).FullName || t == typeof(Nullable<SByte>))
                { return SByte.Parse(value); }
                if (t.FullName == typeof(Decimal).FullName || t == typeof(Nullable<Decimal>))
                { return Decimal.Parse(value); }
                if (t.FullName == typeof(Single).FullName || t == typeof(Nullable<Single>))
                { return Single.Parse(value); }
                if (t.FullName == typeof(Double).FullName || t == typeof(Nullable<Double>))
                { return Double.Parse(value); }
                if (t.FullName == typeof(Int16).FullName || t == typeof(Nullable<Int16>))
                { return Int16.Parse(value); }
                if (t.FullName == typeof(UInt16).FullName || t == typeof(Nullable<UInt16>))
                { return UInt16.Parse(value); }
                if (t.FullName == typeof(int).FullName || t == typeof(Nullable<int>))
                { return int.Parse(value); }
                if (t.FullName == typeof(Int32).FullName || t == typeof(Nullable<Int32>))
                { return Int32.Parse(value); }
                if (t.FullName == typeof(UInt32).FullName || t == typeof(Nullable<UInt32>))
                { return UInt32.Parse(value); }
                if (t.FullName == typeof(Int64).FullName || t == typeof(Nullable<Int64>))
                { return Int64.Parse(value); }
                if (t.FullName == typeof(UInt64).FullName || t == typeof(Nullable<UInt64>))
                { return UInt64.Parse(value); }
                if (t.FullName == typeof(DateTime).FullName || t == typeof(Nullable<DateTime>))
                { return DateTime.Parse(value); }



                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion


        #region 获取枚举的 描述 名称 值
        /// <summary>
        ///  获取枚举的 描述 名称 值
        /// string = 描述, string = 名称, int = 值
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static List<Tuple<string, string, int>> GetEnumItems(Type enumType)
        {
            List<Tuple<string, string, int>> list = new List<Tuple<string, string, int>>();
            if (enumType.IsEnum != true)
            {
                throw new InvalidOperationException();
            }

            FieldInfo[] fields = enumType.GetFields();
            foreach (var field in fields)
            {
                //过滤掉一个不是枚举值的，记录的是枚举的源类型 
                if (field.FieldType.IsEnum == false)
                    continue;

                // 通过字段的名字得到枚举的值 
                int value = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                string text = field.Name;

                string desc = string.Empty;
                object[] descObj = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descObj != null && descObj.Length > 0)
                {
                    desc = ((DescriptionAttribute)descObj[0]).Description;
                }
                //过滤掉删除项
                if (value != -1)
                    list.Add(Tuple.Create<string, string, int>(desc, text, value));
            }
            return list.OrderBy(t => t.Item3).ToList();
        }
        #endregion

    }

    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extensions
    {
        #region System.Data..

        #region DataTable..

        /// <summary>
        /// 合并DataTable中各行的数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keyField">合并列时的参照列</param>
        /// <returns>返回合并后的DataTable</returns>
        public static DataTable MergeRow(this DataTable source, string keyField)
        {
            if (source == null || source.Rows.Count == 0)
            { return null; }

            StringBuilder sb = new StringBuilder();
            foreach (DataColumn dc in source.Columns)
            {
                if (sb.Length > 0) { sb.Append("|"); }

                sb.Append(dc.ColumnName);
            }

            return source.MergeRow(keyField.Split('|'), sb.ToString().Split('|'));
        }

        /// <summary>
        /// 合并DataTable中各行的数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keyField">合并列时的参照列</param>
        /// <param name="mergeField">合并列，为空时不合并</param>
        /// <returns>返回合并后的DataTable</returns>
        public static DataTable MergeRow(this DataTable source, string keyField, string[] mergeField)
        {
            return source.MergeRow(keyField.Split('|'), mergeField);
        }

        /// <summary>
        /// 合并行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keyFields"></param>
        /// <param name="mergeField"></param>
        /// <returns></returns>
        public static DataTable MergeRow(this DataTable source, string[] keyFields, string[] mergeField)
        {
            return source.MergeRow(keyFields, mergeField, "<br>");
        }

        /// <summary>
        /// 合并行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keyFields"></param>
        /// <param name="mergeField"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static DataTable MergeRow(this DataTable source, string[] keyFields, string[] mergeField, string splitStr)
        {
            try
            {
                if (source == null || source.Rows.Count == 0)
                { return null; }

                // 不合并
                if (keyFields.Length == 0) return source;
                if (mergeField.Length < 1) return source;

                splitStr = splitStr.Trim();
                if (splitStr.Length == 0) splitStr = "<br>";

                DataTable result = source.Clone();

                // 遍历整个记录集
                foreach (DataRow drSource in source.Rows)
                {
                    bool blnHasRow = false;
                    foreach (DataRow drTarget in result.Rows)
                    {
                        if (IsEquals(drSource, drTarget, keyFields))
                        {
                            blnHasRow = true;
                            // 把需要合并的列合并
                            for (int i = 0; i < mergeField.Length; i++)
                            {
                                try
                                {
                                    if (drTarget[mergeField[i]].ToString().IndexOf(drSource[mergeField[i]].ToString()) < 0)
                                    { drTarget[mergeField[i]] += splitStr + drSource[mergeField[i]].ToString(); }
                                }
                                catch
                                { }
                            }
                        }
                    }
                    if (!blnHasRow)
                    { DataRow newData = result.Rows.Add(drSource.ItemArray); }
                }

                return result;
            }
            catch
            {
                return source;
            }
        }

        /// <summary>
        /// 判断两个行相应列的值是否相等，全部相等返回true，否则返回false
        /// </summary>
        /// <param name="drSource"></param>
        /// <param name="drTarget"></param>
        /// <param name="keyFields"></param>
        /// <returns></returns>
        private static bool IsEquals(DataRow drSource, DataRow drTarget, string[] keyFields)
        {
            try
            {
                if (keyFields.Length == 0) return false;

                for (int i = 0; i < keyFields.Length; i++)
                {
                    if (keyFields[i].Trim().Length == 0) continue;
                    if (drSource[keyFields[i]].ToString() != drTarget[keyFields[i]].ToString()) return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Xml..


        /// <summary>
        /// 转换二级结点实体列表，二级结点的NodeName需与实体Property一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlNode"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this XElement xmlNode, string nodeName) where T : class
        {
            return CommonHelper.ToList<T>(xmlNode, nodeName);
        }

        /// <summary>
        /// 用xml替换实体内容，返回替换后的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static T Replace<T>(this T entity, XElement xmlNode) where T : class
        {
            CommonHelper.Replace(ref entity, xmlNode);

            return entity;
        }

        /// <summary>
        /// 用xml替换实体内容，返回替换后的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="xmlNode"></param>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static T Replace<T>(this T entity, XElement xmlNode, string[] fileds) where T : class
        {
            CommonHelper.Replace<T>(ref entity, xmlNode, fileds);

            return entity;
        }

        /// <summary>
        /// 在XElement结点中批量增加子结点
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="xmlSubNodes">子结点集合</param>
        public static void AddRange(this XElement xmlNode, List<XElement> xmlSubNodes)
        {
            CommonHelper.AddRange(xmlNode, xmlSubNodes);

            return;
        }

        /// <summary>
        /// 获取结点名，结点名默认按"|"分割
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string GetNodeNameList(this XElement xmlNode)
        {
            return CommonHelper.GetNodeNameList(xmlNode);
        }

        /// <summary>
        /// 获取结点名
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="sSplit"></param>
        /// <returns></returns>
        public static string GetNodeNameList(this XElement xmlNode, string sSplit)
        {
            return CommonHelper.GetNodeNameList(xmlNode, sSplit);
        }

        #endregion

        #region List..
        public static List<T> ToList<T>(this string xmlString) where T : class
        {
            if (String.IsNullOrEmpty(xmlString))
                return new List<T>();

            return ToList<T>(XElement.Parse(xmlString), typeof(T).Name);
        }
        public static T ToModel<T>(this string xmlString) where T : class, new()
        {
            if (String.IsNullOrEmpty(xmlString))
                return null;

            var objModel = (T)Activator.CreateInstance(typeof(T));
            CommonHelper.Replace<T>(ref objModel, XElement.Parse(xmlString));
            return objModel;
        }

        /// <summary>
        /// 通过属性名和值过滤实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstInfos"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T First<T>(this List<T> lstInfos, string fieldName, object value) where T : class
        {
            return CommonHelper.First<T>(lstInfos, fieldName, value);
        }

        /// <summary>
        /// 将IList转换为XElement对象
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <param name="rootName">根节点名</param>
        /// <returns></returns>
        public static XElement GetXml(this IList list, string rootName)
        {
            XElement rlt = new XElement(rootName);

            foreach (var item in list)
            {
                rlt.Add(item.GetXml());
            }

            return rlt;
        }

        /// <summary>
        /// 将IList转换为XElement对象
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <param name="rootName">根节点名</param>
        /// <returns></returns>
        public static XElement GetXml(this IList list, string rootName, string nodeName)
        {
            XElement rlt = new XElement(rootName);

            foreach (var item in list)
            {
                rlt.Add(item.GetXml(nodeName));
            }

            return rlt;
        }

        /// <summary>
        /// 将IList转换为XElement对象，并添加到制定XElement对象下
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <param name="parentElement">制定父对象</param>
        public static void GetXml(this IList list, XElement parentElement)
        {
            foreach (var item in list)
            {
                parentElement.Add(item.GetXml());
            }
        }

        /// <summary>
        /// 将IList转换为XElement对象，并添加到制定XElement对象下
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <param name="parentElement">制定父对象</param>
        public static void GetXml(this IList list, XElement parentElement, string nodeName)
        {
            foreach (var item in list)
            {
                parentElement.Add(item.GetXml(nodeName));
            }
        }

        #endregion

        #region DataTable..

        /// <summary>
        /// 获取DataTable的Xml
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static XElement GetXml(this DataTable dt)
        {
            return CommonHelper.GetXml(dt, "Root", "Row");
        }

        /// <summary>
        /// 获取DataTable的Xml
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootNodeName"></param>
        /// <param name="rowNodeName"></param>
        /// <returns></returns>
        public static XElement GetXml(this DataTable dt, string rootNodeName, string rowNodeName)
        {
            return CommonHelper.GetXml(dt, rootNodeName, rowNodeName);
        }

        #endregion

        #region Entity..

        #region by yang 2012-07-14
        /// <summary>
        /// 将IList转换为XElement对象
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <param name="rootName">根节点名</param>
        /// <returns></returns>
        public static string ToXmlString(this IList list, string rootName = "List")
        {
            if (list.Count == 0)
                return "";

            XElement rlt = new XElement(rootName);
            foreach (var item in list)
            {
                rlt.Add(item.GetXml());
            }
            return rlt.ToString();
        }
        public static string ToXmlString<T>(this T entity, string rootName = "List") where T : class
        {
            if (entity == null)
                return "";
            if (entity is IList)
            {
                return ToXmlString((IList)entity, rootName);
            }
            else
            {
                var t = entity.GetType();
                var fields = CommonHelper.GetTableFieldList(t);
                return CommonHelper.GetXml(entity, t.Name, fields.Split('|')).ToString();
            }
        }
        #endregion

        public static XElement GetXml<T>(this T entity) where T : class
        {
            var t = entity.GetType();
            var fields = CommonHelper.GetTableFieldList(t);
            return CommonHelper.GetXml(entity, t.Name, fields.Split('|'));
        }

        public static XElement GetXml<T>(this T entity, string NodeName) where T : class
        {
            var t = entity.GetType();
            var fields = CommonHelper.GetTableFieldList(t);
            return CommonHelper.GetXml(entity, NodeName, fields.Split('|'));
        }

        public static XElement GetXml<T>(this T entity, string NodeName, string[] fields) where T : class
        {
            return CommonHelper.GetXml(entity, NodeName, fields);
        }

        #endregion

        #region Expression..

        public class ParameterRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> map;
            public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;
                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            // apply composition of lambda expression bodies to parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        #endregion
    }
}
