using BPMS.Common;
using BPMS.References.BPMService;
using System;
using System.ServiceModel;

namespace BPMS.References
{
    public partial class BPMSServiceRef : BaseRef, IDisposable
    {
        #region 构造函数
        public BPMSServiceRef()
        {
            CreateOpenClient();
        }
        #endregion

        #region 属性
        /// <summary>
        /// WCF服务的绑定类型
        /// </summary>
        public static EBinding Binding
        {
            get;
            set;
        }
        /// <summary>
        /// WCF服务的IP
        /// </summary>
        public static string IP
        {
            get;
            set;
        }
        /// <summary>
        /// WCF服务的端品
        /// </summary>
        public static int Port
        {
            get;
            set;
        }
        private static IService _proxy = null;

        #endregion

        #region 打开服务
        /// <summary>
        /// 打开服务
        /// </summary>
        private static void CreateOpenClient()
        {
            if (Binding == EBinding.WsHttpBinding)
            {
                EndpointAddress address = new EndpointAddress(string.Format("http://{0}:{1}/Service.svc", IP, Port));
                _proxy = new ServiceClient("WSHttpBinding_IService", address);
            }
            else if (Binding == EBinding.NetTcpBinding)
            {
                EndpointAddress address = new EndpointAddress(
                  String.Format("net.tcp://{0}:{1}/Service/", IP, Port));
                _proxy = new ServiceClient("NetTcpBinding_IService", address);
            }
            ((ServiceClient)_proxy).Open();
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (((ServiceClient)_proxy).State != CommunicationState.Closed)
                    ((ServiceClient)_proxy).Close();
            }
            catch
            {
            }
        }
        #endregion

        #region 捕获方法
        /// <summary>
        /// 捕获方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        private static T TryCatchCore<T>(Func<T> fun)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            T rlt = default(T);
            try
            {
                rlt = fun();
            }
            catch (EndpointNotFoundException endPointEx)
            {
                throw endPointEx.InnerException;
            }
            catch (Exception ex)
            {
                if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                    CreateOpenClient();
                throw ex;
            }
            return rlt;
        }
        #endregion
    }
}
