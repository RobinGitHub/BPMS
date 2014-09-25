using BPMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.References
{
    public abstract class BaseWcfServiceRef
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        protected abstract string Url { get; }
        /// <summary>
        /// 获取服务对象工厂
        /// </summary>
        protected DynamicProxyFactory DynProFac
        {
            get { return new DynamicProxyFactory(Url); }
        }
        /// <summary>
        /// 获取服务对象 base.DynProFac.CreateProxy("服务ClassName")
        /// </summary>
        protected abstract DynamicProxy DynPro
        {
            get;
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected DynamicObject GetDynamicObject(string typeName)
        {
            DynamicObject obj = new DynamicObject(DynPro.ObjectType.Assembly.GetType(typeName, true, true));
            obj.CallConstructor();
            return obj;
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected DynamicObject GetDynamicObject(object obj)
        {
            return new DynamicObject(obj);
        }
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected object InvokeMethod(string methodName, params object[] args)
        {
            return this.DynPro.CallMethod(methodName, args);
        }
    }
}
