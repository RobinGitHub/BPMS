using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using BPMS.Common;

namespace BPMS.References
{
    public abstract class BaseActiveMQRef
    {
        /// <summary>
        /// 消息队列类型
        /// </summary>
        protected enum MQType
        { 
            /// <summary>
            /// 广播模式
            /// </summary>
            Topic = 0,
            /// <summary>
            /// 点对点模式
            /// </summary>
            Queue = 1,
        }
        /// <summary>
        /// 消息队列类型
        /// </summary>
        protected abstract MQType CurrentMQType { get; }

        /// <summary>
        /// 消息队列Uri
        /// </summary>
        protected abstract Uri MQUri { get; }

        /// <summary>
        /// 消息队列主题名称
        /// </summary>
        protected abstract string MQTopicOrQueueName { get; }

        /// <summary>
        /// 消息队列ClientID
        /// </summary>
        protected abstract string MQClientID { get; }

        /// <summary>
        /// 连接
        /// </summary>
        protected IConnection TopicConn
        {
            get;
            set;
        }

        /// <summary>
        /// 连接工厂对象
        /// </summary>
        protected IConnectionFactory ConnFactory
        {
            get
            {
                return NMSConnectionFactory.CreateConnectionFactory(this.MQUri);
            }
        }

        public void RecieveMessage()
        {
            try
            {
                using (TopicConn = ConnFactory.CreateConnection())
                {
                    TopicConn.ClientId = this.MQClientID;
                    TopicConn.Start();

                    using (ISession session = TopicConn.CreateSession())
                    {
                        IMessageConsumer consumer = null;
                        switch (this.CurrentMQType)
                        {
                            case MQType.Topic:
                                {
                                    consumer = session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(this.MQTopicOrQueueName), this.MQClientID, null, true);
                                    consumer.Listener += new MessageListener(Listener);
                                }
                                break;
                            case MQType.Queue:
                                {
                                    consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(this.MQTopicOrQueueName));
                                    consumer.Listener += new MessageListener(Listener);
                                }
                                break;
                            default:
                                break;
                        }
                        while (true)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info("BaseActiveMQRef.RecieveMessage出错：" + ex.Message);
            }
        }

        protected abstract void Listener(IMessage message);
    }
}
