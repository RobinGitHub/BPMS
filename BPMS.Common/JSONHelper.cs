using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Collections;
using System.Reflection;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace BPMS.Common
{
    public class JSONHelper
    {
        /// <summary>
        /// 其中时间格式为new Date(1234656000000),js直接解析
        /// </summary>
        /// <param name="ins"></param>
        /// <returns></returns>
        public static string Serialize(object ins)
        {
            return JsonConvert.SerializeObject(ins, Formatting.None);
        }

        public static string Serialize<T>(IEnumerable<T> list) where T : class
        {
            var typeName = typeof(T).Name;
            XElement xlist = new XElement(typeName + "s");
            foreach (var data in list)
            {
                xlist.Add(CommonHelper.GetXml<T>(data, typeName, null));
            }
            return JsonConvert.SerializeXNode(xlist);
        }

        public static string Serialize<TKey, TValue>(IDictionary<TKey, TValue> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Dics>");
            foreach (var key in dic.Keys)
            {
                sb.AppendFormat("<Dic><key>{0}</key><value>{1}</value></Dic>",
                    ConvertHelper.ObjectToString(key), ConvertHelper.ObjectToString(dic[key]));
            }
            sb.Append("</Dics>");
            return JsonConvert.SerializeXNode(XElement.Parse(sb.ToString()));
        }

        /// <summary>
        /// 序列化给js前段用
        /// </summary>
        /// <param name="ins"></param>
        /// <returns></returns>
        public static string SerializeJS(object ins)
        {
            return JsonConvert.SerializeObject(ins, Formatting.None, new JavaScriptDateTimeConverter());
        }

        public static string SerializeStandard(object ins)
        {
            return JsonConvert.SerializeObject(ins, Formatting.Indented, new JavaScriptDateTimeConverter());
        }

        /// <summary>
        /// 如果对象的属性未赋值或为null,则不序列化，减少大小
        /// </summary>
        /// <param name="ins"></param>
        /// <returns></returns>
        public static string SerializeNonIncludeNull(object ins)
        {
            return JsonConvert.SerializeObject(ins, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string SerializeByCondition(object ins, string[] fliterList)
        {
            return JsonConvert.SerializeObject(ins, Formatting.None, new JsonSerializerSettings() { ContractResolver = new DynamicContractResolver(fliterList), Converters = new List<JsonConverter>() { new JavaScriptDateTimeConverter() } });
        }

        public static object Deserialize(string str)
        {
            return JsonConvert.DeserializeObject(str);
        }

        public static T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// DataTable序列化成JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJSON(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer();
                jw.WriteStartObject();
                jw.WritePropertyName(dt.TableName);
                jw.WriteStartArray();
                foreach (DataRow dr in dt.Rows)
                {
                    jw.WriteStartObject();

                    foreach (DataColumn dc in dt.Columns)
                    {
                        jw.WritePropertyName(dc.ColumnName);
                        ser.Serialize(jw, dr[dc].ToString());
                    }

                    jw.WriteEndObject();
                }
                jw.WriteEndArray();
                jw.WriteEndObject();

                sw.Close();
                jw.Close();

            }

            return sb.ToString();
        }

        #region json converter for flexigrid

        public static string JsonForFlexiGrid(object datas, int PageIndex, int Total)
        {
            StringBuilder sb = new StringBuilder();

            StringWriter sw = new StringWriter(sb);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("page");
                jsonWriter.WriteValue(PageIndex.ToString());//当前页
                jsonWriter.WritePropertyName("total");
                jsonWriter.WriteValue(Total.ToString());//总共的条数
                jsonWriter.WritePropertyName("rows");
                jsonWriter.WriteStartArray();
                if (datas is DataTable)
                {
                    DataColumnCollection columns = null;
                    foreach (DataRow row in (datas as DataTable).Rows)
                    {
                        jsonWriter.WriteStartObject();
                        jsonWriter.WritePropertyName("cell");
                        jsonWriter.WriteStartArray();

                        if (columns == null)
                        {
                            columns = (datas as DataTable).Columns;
                        }
                        foreach (DataColumn column in columns)
                        {
                            jsonWriter.WriteValue(ConvertHelper.ObjectToString(row[column]));
                        }

                        jsonWriter.WriteEndObject();
                    }
                }
                else if (datas is IEnumerable)
                {
                    PropertyInfo[] pis = null;
                    foreach (var data in (datas as IEnumerable))
                    {
                        jsonWriter.WriteStartObject();
                        jsonWriter.WritePropertyName("cell");
                        jsonWriter.WriteStartArray();

                        if (pis == null)
                        {
                            pis = data.GetType().GetProperties().Where(t => t.GetCustomAttributes(false).Any(o => (o is EdmRelationshipNavigationPropertyAttribute) || (o is BrowsableAttribute)) == false).ToArray();
                        }
                        foreach (var pi in pis)
                        {
                            jsonWriter.WriteValue(ConvertHelper.ObjectToString(pi.GetValue(data, null)));
                        }

                        jsonWriter.WriteEndObject();
                    }
                }
                jsonWriter.WriteEnd();
                jsonWriter.WriteEndObject();
            }

            return sb.ToString();
        }
        #endregion
    }

    public class DynamicContractResolver : DefaultContractResolver
    {
        private readonly string[] filterLst;
        public DynamicContractResolver(string[] lst)
        {
            filterLst = lst;
        }

        protected override IList<JsonProperty> CreateProperties(JsonObjectContract contract)
        {
            IList<JsonProperty> properties = base.CreateProperties(contract);

            properties =
              properties.Where(p => filterLst.Contains(p.PropertyName)).ToList();

            return properties;
        }
    }
}
