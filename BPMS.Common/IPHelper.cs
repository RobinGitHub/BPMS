using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace BPMS.Common
{
    public static class IPHelper
    {
        #region 对象
        public static class RedirectMode
        {
            public static readonly int Mode_1 = 1;
            public static readonly int Mode_2 = 2;
        }

        public static class IPFormat
        {
            public static readonly int HeaderLength = 8;
            public static readonly int IndexRecLength = 7;
            public static readonly int IndexOffset = 3;
            public static readonly int RecOffsetLength = 3;

            public static readonly string UnknownCountry = "未知的国家";
            public static readonly string UnknownZone = "未知的地区";

            public static uint ToUint(byte[] val)
            {
                if (val.Length > 4) throw new ArgumentException();
                if (val.Length < 4)
                {
                    byte[] copyBytes = new byte[4];
                    Array.Copy(val, 0, copyBytes, 0, val.Length);
                    return BitConverter.ToUInt32(copyBytes, 0);
                }
                else
                {
                    return BitConverter.ToUInt32(val, 0);
                }
            }
        }

        public class IPLocation
        {
            private IPAddress m_ip;
            private string m_country;
            private string m_loc;

            public IPLocation()
            { }

            public IPLocation(IPAddress ip, string country, string loc)
            {
                m_ip = ip;
                m_country = country;
                m_loc = loc;
            }

            public IPAddress GetIPAddress
            {
                get { return m_ip; }
            }

            public string Country
            {
                get { return m_country; }
            }

            public string Zone
            {
                get { return m_loc; }
            }

            public string Address { get; set; }

            public override string ToString()
            {
                if (!string.IsNullOrEmpty(Country))
                    return Country + " " + Zone;
                else
                    return Address;
            }
        }
        #endregion

        #region 属性
        private static string m_libPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["qqwry"]);
        private static uint m_indexStart;
        private static uint m_indexEnd;
        #endregion

        #region 获取IP信息
        /// <summary>
        /// 获取IP信息 根据网络状态来读取
        /// 如果联网 在线读取
        /// 否则 读取本地IP库
        /// </summary>
        /// <returns></returns>
        public static IPLocation GetLocation()
        {
            IPLocation location = new IPLocation();
            string ip = GetClientIP();
            //if (InternetConnectionHelper.Fun_IsNetworkAlive() == InternetConnectionHelper.NetworkStatus.拨号上网
            //    || InternetConnectionHelper.Fun_IsNetworkAlive() == InternetConnectionHelper.NetworkStatus.代理上网)
            //{
            //    location.Address = GetIPAdress(ip);
            //}
            //else
            //{
            IPAddress ipaddr = System.Net.IPAddress.Parse(ip);
            location = GetLocation(ipaddr);
            //}
            return location;
        }
        #endregion

        #region 获取IP归属地 根据本地数据
        /// <summary>
        /// 获取IP归属地 根据本地数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPLocation GetLocation(IPAddress ip)
        {
            if (!File.Exists(m_libPath))
                throw new Exception("IP地址本地库文件不存在！");
            using (FileStream fs = new FileStream(m_libPath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(fs);
                Byte[] header = reader.ReadBytes(IPFormat.HeaderLength);
                m_indexStart = BitConverter.ToUInt32(header, 0);
                m_indexEnd = BitConverter.ToUInt32(header, 4);
                //Because it is network order(BigEndian), so we need to transform it into LittleEndian 
                Byte[] givenIpBytes = BitConverter.GetBytes(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ip.GetAddressBytes(), 0)));
                uint offset = FindStartPos(fs, reader, m_indexStart, m_indexEnd, givenIpBytes);
                return GetIPInfo(fs, reader, offset, ip, givenIpBytes);
            }
        }
        #endregion

        #region 获取客户端IP
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    return ip.ToString();
                }
            }
            return "";
        }
        #endregion

        #region 在线获取IP、域名归属地
        /// <summary>
        /// 获取IP归属地
        /// </summary>
        /// <param name="strIP">IP地址</param>
        /// <returns>IP归属地</returns>
        public static string GetIPAdress(string strIP)
        {
            string s = HttpHelper.GetHtml("http://www.ip138.com/ips1388.asp?ip=" + strIP + "&action=2");
            int a = s.IndexOf("本站主数据：") + "本站主数据：".Length;
            int b = s.IndexOf("参考数据一");
            return s.Substring(a, b - a).Replace("</li><li>", "");
        }

        /// <summary>
        /// 获取网站域名归属地
        /// </summary>
        /// <param name="domainName">网站域名</param>
        /// <returns>网站域名归属地</returns>
        public static string GetDomainIP(string domainName)
        {
            if (string.IsNullOrEmpty(domainName))
                return null;
            string s = HttpHelper.GetHtml("http://www.ip138.com/ips1388.asp?ip=" + domainName + "&action=2");
            int a = s.IndexOf("<h1><font color=\"blue\">") + "<h1><font color=\"blue\">".Length;
            int b = s.IndexOf("</font></h1></td>");
            return s.Substring(a, b - a).Replace(" >> ", "").Replace(" ", "");
        }
        #endregion

        #region 判断指定的IP是否在指定的IP范围内
        /// <summary>
        /// 接口函数 参数分别是你要判断的IP  和 你允许的IP范围
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="ranges">"10.0.0.0-10.255.255.255" 这种格式的数组</param>
        /// <returns></returns>
        public static bool TheIpIsRange(string ip, params string[] ranges)
        {
            bool tmpRes = false;
            foreach (var item in ranges)
            {
                if (TheIpIsRange(ip, item))
                {
                    tmpRes = true; break;
                }
            }
            return tmpRes;
        }

        /// <summary>
        /// 判断指定的IP是否在指定的IP范围内   这里只能指定一个范围
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="ranges">"10.0.0.0-10.255.255.255"</param>
        /// <returns></returns>
        public static bool TheIpIsRange(string ip, string ranges)
        {
            bool result = false;

            int count;
            string start_ip, end_ip;
            //检测指定的IP范围 是否合法
            TryParseRanges(ranges, out count, out start_ip, out end_ip);//检测ip范围格式是否有效

            if (ip == "::1") ip = "127.0.0.1";

            try
            {
                IPAddress.Parse(ip);//判断指定要判断的IP是否合法
            }
            catch (Exception)
            {
                throw new ApplicationException("要检测的IP地址无效");
            }

            if (count == 1 && ip == start_ip) result = true;//如果指定的IP范围就是一个IP，那么直接匹配看是否相等
            else if (count == 2)//如果指定IP范围 是一个起始IP范围区间
            {
                byte[] start_ip_array = Get4Byte(start_ip);//将点分十进制 转换成 4个元素的字节数组
                byte[] end_ip_array = Get4Byte(end_ip);
                byte[] ip_array = Get4Byte(ip);

                bool tmpRes = true;
                for (int i = 0; i < 4; i++)
                {
                    //从左到右 依次比较 对应位置的 值的大小  ，一旦检测到不在对应的范围 那么说明IP不在指定的范围内 并将终止循环
                    if (ip_array[i] > end_ip_array[i] || ip_array[i] < start_ip_array[i])
                    {
                        tmpRes = false; break;
                    }
                }
                result = tmpRes;
            }
            return result;
        }

        //尝试解析IP范围  并获取闭区间的 起始IP   (包含)
        private static void TryParseRanges(string ranges, out int count, out string start_ip, out string end_ip)
        {
            string[] _r = ranges.Split('-');
            if (!(_r.Length == 2 || _r.Length == 1))
                throw new ApplicationException("IP范围指定格式不正确，可以指定一个IP，如果是一个范围请用“-”分隔");

            count = _r.Length;

            start_ip = _r[0];
            end_ip = "";
            try
            {
                IPAddress.Parse(_r[0]);
            }
            catch (Exception)
            {
                throw new ApplicationException("IP地址无效");
            }

            if (_r.Length == 2)
            {
                end_ip = _r[1];
                try
                {
                    IPAddress.Parse(_r[1]);
                }
                catch (Exception)
                {
                    throw new ApplicationException("IP地址无效");
                }
            }
        }


        /// <summary>
        /// 将IP四组值 转换成byte型
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static byte[] Get4Byte(string ip)
        {
            string[] _i = ip.Split('.');

            List<byte> res = new List<byte>();
            foreach (var item in _i)
            {
                res.Add(Convert.ToByte(item));
            }

            return res.ToArray();
        }
        #endregion

        #region 通过SendARP获取网卡Mac
        ///<summary>
        /// 通过SendARP获取网卡Mac
        /// 网络被禁用或未接入网络（如没插网线）时此方法失灵
        ///</summary>
        ///<param name="remoteIP"></param>
        ///<returns></returns>
        public static string GetMacBySendARP(string remoteIP)
        {
            StringBuilder macAddress = new StringBuilder();

            try
            {
                Int32 remote = inet_addr(remoteIP);

                Int64 macInfo = new Int64();
                Int32 length = 6;
                SendARP(remote, 0, ref macInfo, ref length);

                string temp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();

                int x = 12;
                for (int i = 0; i < 6; i++)
                {
                    if (i == 5)
                    {
                        macAddress.Append(temp.Substring(x - 2, 2));
                    }
                    else
                    {
                        macAddress.Append(temp.Substring(x - 2, 2) + "-");
                    }
                    x -= 2;
                }

                return macAddress.ToString();
            }
            catch
            {
                return macAddress.ToString();
            }
        }

        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);
        #endregion

        #region private method
        private static uint FindStartPos(FileStream fs, BinaryReader reader, uint m_indexStart, uint m_indexEnd, byte[] givenIp)
        {
            uint givenVal = BitConverter.ToUInt32(givenIp, 0);
            fs.Position = m_indexStart;

            while (fs.Position <= m_indexEnd)
            {
                Byte[] bytes = reader.ReadBytes(IPFormat.IndexRecLength);
                uint curVal = BitConverter.ToUInt32(bytes, 0);
                if (curVal > givenVal)
                {
                    fs.Position = fs.Position - 2 * IPFormat.IndexRecLength;
                    bytes = reader.ReadBytes(IPFormat.IndexRecLength);
                    byte[] offsetByte = new byte[4];
                    Array.Copy(bytes, 4, offsetByte, 0, 3);
                    return BitConverter.ToUInt32(offsetByte, 0);
                }
            }
            return 0;
        }

        private static IPLocation GetIPInfo(FileStream fs, BinaryReader reader, long offset, IPAddress ipToLoc, Byte[] ipBytes)
        {
            fs.Position = offset;
            //To confirm that the given ip is within the range of record IP range 
            byte[] endIP = reader.ReadBytes(4);
            uint endIpVal = BitConverter.ToUInt32(endIP, 0);
            uint ipVal = BitConverter.ToUInt32(ipBytes, 0);
            if (endIpVal < ipVal) return null;

            string country;
            string zone;
            //Read the Redirection pattern byte 
            Byte pattern = reader.ReadByte();
            if (pattern == RedirectMode.Mode_1)
            {
                Byte[] countryOffsetBytes = reader.ReadBytes(IPFormat.RecOffsetLength);
                uint countryOffset = IPFormat.ToUint(countryOffsetBytes);

                if (countryOffset == 0) return GetUnknownLocation(ipToLoc);

                fs.Position = countryOffset;
                if (fs.ReadByte() == RedirectMode.Mode_2)
                {
                    return ReadMode2Record(fs, reader, ipToLoc);
                }
                else
                {
                    fs.Position--;
                    country = ReadString(reader);
                    zone = ReadZone(fs, reader, Convert.ToUInt32(fs.Position));
                }
            }
            else if (pattern == RedirectMode.Mode_2)
            {
                return ReadMode2Record(fs, reader, ipToLoc);
            }
            else
            {
                fs.Position--;
                country = ReadString(reader);
                zone = ReadZone(fs, reader, Convert.ToUInt32(fs.Position));
            }
            return new IPLocation(ipToLoc, country, zone);

        }

        //When it is in Mode 2 
        private static IPLocation ReadMode2Record(FileStream fs, BinaryReader reader, IPAddress ip)
        {
            uint countryOffset = IPFormat.ToUint(reader.ReadBytes(IPFormat.RecOffsetLength));
            uint curOffset = Convert.ToUInt32(fs.Position);
            if (countryOffset == 0) return GetUnknownLocation(ip);
            fs.Position = countryOffset;
            string country = ReadString(reader);
            string zone = ReadZone(fs, reader, curOffset);
            return new IPLocation(ip, country, zone);
        }

        //return a Unknown Location 
        private static IPLocation GetUnknownLocation(IPAddress ip)
        {
            string country = IPFormat.UnknownCountry;
            string zone = IPFormat.UnknownZone;
            return new IPLocation(ip, country, zone);
        }
        //Retrieve the zone info 
        private static string ReadZone(FileStream fs, BinaryReader reader, uint offset)
        {
            fs.Position = offset;
            byte b = reader.ReadByte();
            if (b == RedirectMode.Mode_1 || b == RedirectMode.Mode_2)
            {
                uint zoneOffset = IPFormat.ToUint(reader.ReadBytes(3));
                if (zoneOffset == 0) return IPFormat.UnknownZone;
                return ReadZone(fs, reader, zoneOffset);
            }
            else
            {
                fs.Position--;
                return ReadString(reader);
            }
        }

        private static string ReadString(BinaryReader reader)
        {
            List<byte> stringLst = new List<byte>();
            byte byteRead = 0;
            while ((byteRead = reader.ReadByte()) != 0)
            {
                stringLst.Add(byteRead);
            }
            return Encoding.GetEncoding("gb2312").GetString(stringLst.ToArray());
        }
        #endregion

    }
}
