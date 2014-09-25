using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BPMS.Common
{
    public class HttpWebResponseHelper
    {
        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36";
        private static readonly string DefaultAccept = "text/html,application/json,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        //private static readonly string DefaultContentType = "text/html; charset=GBK";
        private static readonly int DefaultTimeOut = 10000;
        /// <summary>
        /// cookie容器
        /// </summary>
        public static CookieContainer cookieContainer = new CookieContainer();

        #region 创建GET方式的HTTP请求
        public static HttpWebRequest CreateGetHttpResponse(string url)
        {
            return CreateGetHttpResponse(url, null, null, null, null);
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebRequest CreateGetHttpResponse(string url, CookieContainer cookieContainer)
        {
            return CreateGetHttpResponse(url, null, null, cookieContainer, null);
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookieContainer">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <param name="referer"></param>
        /// <returns></returns> 
        public static HttpWebRequest CreateGetHttpResponse(string url, CookieContainer cookieContainer, string referer)
        {
            return CreateGetHttpResponse(url, null, null, cookieContainer, referer);
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebRequest CreateGetHttpResponse(string url, int? timeout, CookieContainer cookieContainer)
        {
            return CreateGetHttpResponse(url, timeout, null, cookieContainer, null);
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebRequest CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieContainer cookieContainer, string referer)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.KeepAlive = true;
            request.Accept = DefaultAccept;
            request.UserAgent = DefaultUserAgent;
            request.Headers.Add("Accept-Language: zh-CN");
            request.Headers.Add("Accept-Encoding: gzip, deflate");

            request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            else
            {
                request.Timeout = DefaultTimeOut;
            }
            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            else
            {
                request.CookieContainer = HttpWebResponseHelper.cookieContainer;
            }
            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }
            return request;
        }
        #endregion

        #region 创建POST方式的HTTP请求
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, List<KeyValuePair<string, string>> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieContainer container, string referer)
        {
            HttpWebResponse response = null;

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                requestEncoding = Encoding.ASCII;
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            request.Method = "POST";
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Headers.Add("Accept-Language: zh-CN,zh");
            request.Headers.Add("Accept-Encoding: gzip,deflate,sdch");
            request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            else
            {
                request.Timeout = DefaultTimeOut;
            }
            request.CookieContainer = container;
            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }

            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;

                foreach (var keyvalue in parameters)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", keyvalue.Key, keyvalue.Value);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", keyvalue.Key, keyvalue.Value);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                request.ContentLength = data.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            response = request.GetResponse() as HttpWebResponse;
            return response;
        }
        #endregion

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }


        #region Get请求
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetString(string url, CookieContainer cookieContainer, string referer)
        {
            string result = string.Empty;
            HttpWebRequest request = CreateGetHttpResponse(url, cookieContainer, referer);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                result = GetResponseString(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region 解析响应
        /// <summary>
        /// 解析响应
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string GetResponseString(HttpWebResponse response)
        {
            string result = string.Empty;
            if (response != null)
            {
                Stream responseStream = response.GetResponseStream();
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                }
                using (StreamReader streamReader = new StreamReader(responseStream)) //, Encoding.GetEncoding(response.ContentEncoding
                {
                    result = streamReader.ReadToEnd();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
            }
            return result;
        }
        #endregion

        #region Post请求
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="container"></param>
        /// <param name="referer"></param>
        /// <returns></returns>
        public static string PostString(string url, List<KeyValuePair<string, string>> parameters, CookieContainer container, string referer)
        {
            string result = string.Empty;
            try
            {
                HttpWebResponse response = CreatePostHttpResponse(url, parameters, null, null, null, container, referer);
                result = GetResponseString(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion
    }
}
