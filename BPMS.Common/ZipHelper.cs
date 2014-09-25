using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BPMS.Common
{
    /// <summary>
    /// 压缩 解压
    /// </summary>
    public class ZipHelper
    {
        #region 压缩解缩
        /// <summary>
        /// 压缩指定字符串
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string Compress(string strSource)
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            byte[] buffer = encoding.GetBytes(strSource);
            return Convert.ToBase64String(Compress(buffer)); //将压缩后的byte[]转换为Base64String
        }
        /// <summary>
        /// 解压缩指定字符串
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string Decompress(string strSource)
        {
            byte[] buffer = Convert.FromBase64String(strSource);
            return System.Text.Encoding.Unicode.GetString(Decompress(buffer));//转换为普通的字符串
        }
        /// <summary>
        /// 压缩DataTable
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CompressDataTable(DataTable source)
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            return Convert.ToBase64String(GetDataTableZipBytes(source));
        }
        /// <summary>
        /// 解压DataTable
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable DecompressDataTable(string source)
        {
            byte[] buffer = Convert.FromBase64String(source);
            return GetZipBytesDataTable(buffer);
        }
        /// <summary>
        /// 压缩DataSet
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CompressDataSet(DataSet source)
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            return Convert.ToBase64String(GetDataSetZipBytes(source));
        }
        /// <summary>
        /// 解压DataSet
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataSet DecompressDataSet(string source)
        {
            byte[] buffer = Convert.FromBase64String(source);
            return GetZipBytesDataSet(buffer);
        }
        #endregion

        #region 私有方法

        #region 压缩基础流
        /// <summary>
        /// 压缩基础流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                Stream zipStream = null;
                zipStream = new GZipStream(ms, CompressionMode.Compress, true);
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                ms.Position = 0;
                byte[] compressed_data = new byte[ms.Length];
                ms.Read(compressed_data, 0, int.Parse(ms.Length.ToString()));
                return compressed_data;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 解压缩基础流
        /// <summary>
        /// 解压缩基础流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                Stream zipStream = null;
                zipStream = new GZipStream(ms, CompressionMode.Decompress);
                byte[] dc_data = null;
                dc_data = EtractBytesFormStream(zipStream, data.Length);
                return dc_data;
            }
            catch
            {
                return null;
            }
        }
        private static byte[] EtractBytesFormStream(Stream zipStream, int dataBlock)
        {
            try
            {
                byte[] data = null;
                int totalBytesRead = 0;
                while (true)
                {
                    Array.Resize(ref data, totalBytesRead + dataBlock + 1);
                    int bytesRead = zipStream.Read(data, totalBytesRead, dataBlock);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    totalBytesRead += bytesRead;
                }
                Array.Resize(ref data, totalBytesRead);
                return data;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 将DataSet转换为压缩的字节数组
        /// </summary>
        /// <returns></returns>
        private static byte[] GetDataSetZipBytes(DataSet ds)
        {
            BinaryFormatter ser = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ser.Serialize(ms, ds);
            byte[] buffer = ms.ToArray();
            byte[] zipBuffer = Compress(buffer);
            return zipBuffer;
        }
        private static byte[] GetDataTableZipBytes(DataTable dt)
        {
            BinaryFormatter ser = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ser.Serialize(ms, dt);
            byte[] buffer = ms.ToArray();
            byte[] zipBuffer = Compress(buffer);
            return zipBuffer;
        }

        /// <summary>
        /// 将压缩的字节数组转换为DataSet
        /// </summary>
        /// <returns></returns>
        private static DataSet GetZipBytesDataSet(byte[] zipBuffer)
        {
            byte[] buffer = Decompress(zipBuffer);
            BinaryFormatter ser = new BinaryFormatter();
            DataSet ds = ser.Deserialize(new MemoryStream(buffer)) as DataSet;
            return ds;
        }

        /// <summary>
        /// 将压缩的字节数组转换为DataTable
        /// </summary>
        /// <returns></returns>
        private static DataTable GetZipBytesDataTable(byte[] zipBuffer)
        {
            byte[] buffer = Decompress(zipBuffer);
            BinaryFormatter ser = new BinaryFormatter();
            DataTable dt = ser.Deserialize(new MemoryStream(buffer)) as DataTable;
            return dt;
        }
        #endregion
    }
}
