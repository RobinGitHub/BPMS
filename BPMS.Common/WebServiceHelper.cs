using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Net;
using System.IO;
using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace BPMS.Common
{
    /// <summary>
    /// 动态调用web服务(不通过Web引用)
    /// </summary>
    public class WebServiceHelper
    {
        #region InvokeWebService
        //动态调用web服务(不通过Web引用)
        /// <summary>
        /// 调用web服务
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="methodname">方法名</param>
        /// <param name="args">参数</param>
        /// <returns>返回结果</returns>
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return WebServiceHelper.InvokeWebService(url, null, methodname, args);
        }

        public static object InvokeWebService1(string url, string methodname, params object[] args)
        {
            return WebServiceHelper.InvokeWebService(url, null, methodname, args);
        }

        public static object InvokeWebService(object obj, string methodname, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            try
            {
                Type t = obj.GetType();
                var assembly = t.Assembly;
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);

                //重新构造参数
                System.Collections.Generic.List<object> listParams = new System.Collections.Generic.List<object>();
                // Landry modify at: 2010-12-08
                // Description: 添加 args 与 arg 的为空判断，以支持反射调用没有参数及可为空值参数的方法
                if (args != null)
                {
                    foreach (var arg in args)
                    {
                        if (arg != null)
                        {
                            var argType = arg.GetType();
                            if (argType.Namespace == @namespace)
                            {
                                var tmpClass = assembly.GetType(@namespace + "." + argType.Name, true, true);
                                var tmpObj = Activator.CreateInstance(tmpClass);
                                foreach (var field in argType.GetFields())
                                {
                                    tmpClass.GetField(field.Name).SetValue(tmpObj, field.GetValue(arg));
                                }
                                listParams.Add(tmpObj);
                            }
                            else
                            {
                                listParams.Add(arg);
                            }
                        }
                        else
                        {
                            listParams.Add(arg);
                        }
                    }
                }

                return mi.Invoke(obj, listParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        /// <summary>
        /// 调用web服务
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="classname">类名</param>
        /// <param name="methodname">方法名</param>
        /// <param name="args">参数</param>
        /// <returns>返回结果</returns>
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            if ((classname == null) || (classname == ""))
            {
                classname = WebServiceHelper.GetWsClassName(url);
            }

            try
            {
                object obj = GetClassInstance(url, classname);

                return InvokeWebService(obj, methodname, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }
        #endregion

        #region GetClassInstance
        /// <summary>
        /// 获取实例对象
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="classname">类名</param>
        /// <returns>返回结果</returns>
        public static object GetClassInstance(string url, string classname)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = WebServiceHelper.GetWsClassName(url);
            }

            try
            {
                //获取WSDL
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider csc = new CSharpCodeProvider();
                //ICodeCompiler icc = csc.CreateCompiler();

                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = csc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);

                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }
        #endregion

        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }

    }
}
