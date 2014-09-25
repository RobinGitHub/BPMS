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
using System.Xml.Linq;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

namespace BPMS.Common
{
    /// <summary>
    /// comet 【计】：基于 HTTP长连接的“服务器推”技术，是一种新的 Web 应用架构。基于这种架构开发的应用中，服务器端会主动以异步的方式向客户端程序推送数据，而不需要客户端显式的发出请求。Comet 架构非常适合事件驱动的 Web 应用，以及对交互性和实时性要求很强的应用，如股票交易行情分析、聊天室和 Web 版在线游戏等。
    /// </summary>
    public class CometHelper
    {
        /// <summary>
        /// 请求超时时间，秒
        /// </summary>
        public static int Timeout = 5;

        /// <summary>
        /// 创建Comet线程
        /// </summary>
        /// <param name="count">线程数量</param>
        /// <param name="timeout">请求超时时间</param>
        public static void CreateCometThreads(int timeout, int count)
        {
            Timeout = timeout;
            CometThreadPool.CreateThreads(count);
        }

        /// <summary>
        /// 推数据，并做对应的检查
        /// </summary>
        /// <param name="request">Comet请求</param>
        /// <param name="timeout">是否超时</param>
        /// <returns></returns>
        public delegate object CheckForServerPushDele(CometWaitRequest request, bool timeout);
        public static CheckForServerPushDele CheckForServerPushEvent;
    }

    #region Comet框架

    #region Comet框架：异步请求处理类

    /// <summary>
    /// Comet框架：异步请求处理类
    /// </summary>
    public class CometAsyncHandler : IHttpAsyncHandler
    {
        #region IHttpAsyncHandler Members

        /// <summary>
        /// 重写方法：启动对 HTTP 处理程序的异步调用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cb">异步方法调用完成时要调用的 AsyncCallback。如果 cb 为 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing），则不调用委托</param>
        /// <param name="extraData">处理该请求所需的所有额外数据</param>
        /// <returns></returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            //  get the result here
            CometAsyncResult result = new CometAsyncResult(context, cb, extraData);

            result.BeginWaitRequest();

            //  ok, return it
            return result;
        }

        /// <summary>
        /// 重写方法：进程结束时提供异步处理 End 方法
        /// </summary>
        /// <param name="result"></param>
        public void EndProcessRequest(IAsyncResult result)
        {
            CometAsyncResult cometAsyncResult = result as CometAsyncResult;

            if (cometAsyncResult != null && cometAsyncResult.ResponseObject != null)
            {
                cometAsyncResult.HttpContext.Response.Write(cometAsyncResult.ResponseObject);
            }

            cometAsyncResult.HttpContext.Response.End();
        }

        #endregion

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new InvalidOperationException("ASP.NET Should never use this property");
        }

        #endregion
    }

    #endregion

    #region Comet框架：异步处理结果

    /// <summary>
    /// Comet框架：异步处理结果
    /// </summary>
    public class CometAsyncResult : IAsyncResult
    {
        private HttpContext context;
        private AsyncCallback callback;
        private object asyncState;
        private bool isCompleted = false;
        private object responseObject;

        public CometAsyncResult(HttpContext context, AsyncCallback callback, object asyncState)
        {
            this.callback = callback;
            this.context = context;
            this.asyncState = asyncState;
        }

        #region IAsyncResult Members

        public object AsyncState
        {
            get { return this.asyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new InvalidOperationException("ASP.NET Should never use this property"); }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted
        {
            get { return this.isCompleted; }
        }

        #endregion

        public HttpContext HttpContext
        {
            get { return this.context; }
        }

        public object ResponseObject
        {
            get { return this.responseObject; }
            set { this.responseObject = value; }
        }


        public void BeginWaitRequest()
        {
            CometThreadPool.QueueCometWaitRequest(new CometWaitRequest(this));
        }

        internal void SetCompleted()
        {
            this.isCompleted = true;

            if (callback != null)
                callback(this);
        }
    }

    #endregion

    #region Comet框架：线程池

    /// <summary>
    /// Comet框架：线程池
    /// </summary>
    public static class CometThreadPool
    {
        private static object state = new object();
        private static List<CometWaitThread> waitThreads = new List<CometWaitThread>();
        private static int nextWaitThread = 0;
        private static int maxWaitThreads = 0;

        /// <summary>
        /// 添加请求到队列
        /// </summary>
        /// <param name="request"></param>
        internal static void QueueCometWaitRequest(CometWaitRequest request)
        {
            CometWaitThread waitThread;

            lock (state)
            {
                //  else, get the next wait thread
                waitThread = waitThreads[nextWaitThread];
                //  cycle the thread that we want
                nextWaitThread++;
                if (nextWaitThread == maxWaitThreads)
                    nextWaitThread = 0;

                CometWaitRequest.RequestCount++;
            }

            //  queue the wait request
            waitThread.QueueCometWaitRequest(request);
        }

        /// <summary>
        /// 创建线程
        /// </summary>
        /// <param name="count"></param>
        public static void CreateThreads(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CometWaitThread waitThread = new CometWaitThread();
                waitThreads.Add(waitThread);
            }

            maxWaitThreads = count;
        }
    }

    #endregion

    #region Comet框架：等待请求

    /// <summary>
    /// Comet框架：等待请求
    /// </summary>
    public class CometWaitRequest
    {
        private CometAsyncResult result;
        private DateTime dateTimeAdded = DateTime.Now;
        public static int RequestCount = 0;

        public CometWaitRequest(CometAsyncResult result)
        {
            this.result = result;
        }

        public CometAsyncResult Result
        {
            get { return this.result; }
        }

        public DateTime DateTimeAdded
        {
            get { return this.dateTimeAdded; }
        }
    }

    #endregion

    #region Comet框架：等待线程

    /// <summary>
    /// Comet框架：等待线程
    /// </summary>
    public class CometWaitThread
    {
        private object state = new object();
        private List<CometWaitRequest> waitRequests = new List<CometWaitRequest>();
        private bool started = false;

        public List<CometWaitRequest> WaitRequests
        {
            get { return this.waitRequests; }
        }

        public CometWaitThread()
        {
            if (!started)
            {
                started = true;

                Thread t = new Thread(new ThreadStart(QueueCometWaitRequest_WaitCallback));
                t.IsBackground = false;
                t.Start();
            }
        }

        /// <summary>
        /// 增加请求
        /// </summary>
        /// <param name="request"></param>
        internal void QueueCometWaitRequest(CometWaitRequest request)
        {
            lock (this.state)
            {
                waitRequests.Add(request);
            }
        }
        /// <summary>
        /// 删除请求
        /// </summary>
        /// <param name="request"></param>
        internal void DequeueCometWaitRequest(CometWaitRequest request)
        {
            lock (state)
            {
                this.waitRequests.Remove(request);
                CometWaitRequest.RequestCount--;
            }
        }

        /// <summary>
        /// 请求处理完成，返回结果，并置状态
        /// </summary>
        /// <param name="target"></param>
        private void QueueCometWaitRequest_Finished(object target)
        {
            CometWaitRequest request = target as CometWaitRequest;
            request.Result.SetCompleted();
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        private void QueueCometWaitRequest_WaitCallback()
        {
            //  here we are...
            //  in a loop

            while (true)
            {
                //Debug.WriteLine(string.Format("QueueCometWaitRequest_WaitCallback Tick: {0} {1} ", Thread.CurrentThread.IsThreadPoolThread, Thread.CurrentThread.ManagedThreadId));

                CometWaitRequest[] processRequest;

                lock (this.state)
                {
                    processRequest = waitRequests.ToArray();
                }

                //  we have no more wait requests left, so we want exis
                //if (processRequest.Length == 0)
                //    break;

                Thread.Sleep(100);

                for (int i = 0; i < processRequest.Length; i++)
                {
                    //  timed out so remove from the queue
                    if (DateTime.Now.Subtract(processRequest[i].DateTimeAdded).TotalSeconds >= CometHelper.Timeout)
                    {
                        //Debug.WriteLine(string.Format("QueueCometWaitRequest_WaitCallback Timeout: {0} {1} ", Thread.CurrentThread.IsThreadPoolThread, Thread.CurrentThread.ManagedThreadId));
                        //  dequeue the request 
                        DequeueCometWaitRequest(processRequest[i]);
                        //
                        //  queue anotehr wait callback, so
                        //  we tell close handler down
                        //  the endRequest will exist on a different thread to this
                        //  one and not tear down this thread
                        processRequest[i].Result.ResponseObject = this.CheckForServerPushEvent(processRequest[i], true);
                        this.QueueCometWaitRequest_Finished(processRequest[i]);
                    }
                    else
                    {
                        object serverPushEvent = this.CheckForServerPushEvent(processRequest[i], false);
                        if (serverPushEvent != null)
                        {
                            //  we have our event, which is good
                            //  it means we can serialize it back to the client
                            processRequest[i].Result.ResponseObject = serverPushEvent;
                            //  queue the response on another ASP.NET Worker thread
                            this.QueueCometWaitRequest_Finished(processRequest[i]);
                            //  dequeue the request
                            DequeueCometWaitRequest(processRequest[i]);
                        }
                    }

                    // Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// 推数据，并做对应的检查
        /// </summary>
        /// <param name="request">Comet等待请求</param>
        /// <param name="timeout">是否超时</param>
        /// <returns></returns>
        private object CheckForServerPushEvent(CometWaitRequest request, bool timeout)
        {
            if (CometHelper.CheckForServerPushEvent != null)
            {
                return CometHelper.CheckForServerPushEvent(request, timeout);
            }
            return null;
        }

    }

    #endregion

    #endregion
}
