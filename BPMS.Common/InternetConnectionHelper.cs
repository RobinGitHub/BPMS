using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BPMS.Common
{
    /// <summary>
    /// 网络连接帮助
    /// </summary>
    public class InternetConnectionHelper
    {
        /// <summary>
        /// 网络状态
        /// </summary>
        public enum NetworkStatus
        {
            拨号上网,
            局域网,
            代理上网,
            MODEM被其他非INTERNET连接占用,
            无网络,
        }

        [DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);
        /// <summary>
        /// 测试网络状态
        /// </summary>
        /// <returns></returns>
        public static NetworkStatus Fun_InternetGetConnectedState()
        {
            int INTERNET_CONNECTION_MODEM = 1;
            int INTERNET_CONNECTION_LAN = 2;
            int INTERNET_CONNECTION_PROXY = 4;
            int INTERNET_CONNECTION_MODEM_BUSY = 8;

            NetworkStatus status = NetworkStatus.无网络;
            Int32 flags = new int();//上网方式 
            bool m_bOnline = true;//是否在线 

            m_bOnline = InternetGetConnectedState(ref flags, 0);
            if (m_bOnline)//在线   
            {
                if ((flags & INTERNET_CONNECTION_MODEM) == INTERNET_CONNECTION_MODEM)
                {
                    status = NetworkStatus.拨号上网;
                }
                if ((flags & INTERNET_CONNECTION_LAN) == INTERNET_CONNECTION_LAN)
                {
                    status = NetworkStatus.局域网;
                }
                if ((flags & INTERNET_CONNECTION_PROXY) == INTERNET_CONNECTION_PROXY)
                {
                    status = NetworkStatus.代理上网;
                }
                if ((flags & INTERNET_CONNECTION_MODEM_BUSY) == INTERNET_CONNECTION_MODEM_BUSY)
                {
                    status = NetworkStatus.MODEM被其他非INTERNET连接占用;
                }
            }
            else
            {
                status = NetworkStatus.无网络;
            }

            return status;
        }


        [DllImport("sensapi.dll")]
        private extern static bool IsNetworkAlive(out int connectionDescription);
        /// <summary>
        /// 测试网络状态
        /// </summary>
        /// <returns></returns>
        public static NetworkStatus Fun_IsNetworkAlive()
        {
            int NETWORK_ALIVE_LAN = 0;
            int NETWORK_ALIVE_WAN = 2;
            int NETWORK_ALIVE_AOL = 4;

            NetworkStatus status = NetworkStatus.无网络;
            int flags;//上网方式 
            bool m_bOnline = true;//是否在线 

            m_bOnline = IsNetworkAlive(out flags);
            if (m_bOnline)//在线   
            {
                if ((flags & NETWORK_ALIVE_LAN) == NETWORK_ALIVE_LAN)
                {
                    status = NetworkStatus.拨号上网;
                }
                if ((flags & NETWORK_ALIVE_WAN) == NETWORK_ALIVE_WAN)
                {
                    status = NetworkStatus.局域网;
                }
                if ((flags & NETWORK_ALIVE_AOL) == NETWORK_ALIVE_AOL)
                {
                    //"在线：NETWORK_ALIVE_AOL\n"; 不知道这是什么意思，暂时用这个代替
                    status = NetworkStatus.拨号上网;
                }
            }
            else
            {
                status = NetworkStatus.无网络;
            }

            return status;
        }
    }

}
