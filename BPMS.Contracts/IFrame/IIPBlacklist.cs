using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.Model;
using System.ServiceModel;

namespace BPMS.Contracts 
{ 
    /// <summary> 
    /// IP黑名单
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取IP黑名单列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string IPBlackGetList(string xmlCredentials);
        /// <summary>
        /// 获取IP黑名单对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string IPBlackGetModel(string xmlCredentials, int id);
        /// <summary>
        /// IP黑名单添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int IPBlackAdd(string xmlCredentials, string xmlModel);
        /// <summary>
        /// IP黑名单编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int IPBlackEdit(string xmlCredentials, string xmlModel);
        /// <summary>
        /// IP黑名单删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        int IPBlackDelete(string xmlCredentials, int id);        
    }
} 


