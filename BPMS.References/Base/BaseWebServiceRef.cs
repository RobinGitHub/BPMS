using BPMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.References
{
    public abstract class BaseWebServiceRef
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        protected abstract string Url { get; }
        /// <summary>
        /// WebService服务对象 base.GetInstance("服务ClassName")
        /// </summary>
        protected abstract object ServiceInstance { get; }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        protected object GetInstance(string className)
        {
            var obj = WebServiceHelper.GetClassInstance(this.Url, className);
            ((System.Web.Services.Protocols.SoapHttpClientProtocol)obj).Url = this.Url;
            return obj;
        }

        protected object InvokeMethod(string methodName, object[] args)
        {
            return WebServiceHelper.InvokeWebService(this.ServiceInstance, methodName, args);
        }
    }
}
