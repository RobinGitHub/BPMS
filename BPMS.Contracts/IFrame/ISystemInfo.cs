using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.Model;
using System.ServiceModel;
using System.Data;

namespace BPMS.Contracts
{
    /// <summary> 
    /// 系统
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string SystemGetList(string xmlCredentials);
        /// <summary>
        /// 获取系统对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string SystemGetModel(string xmlCredentials, int id);
        /// <summary>
        /// 系统添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int SystemAdd(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 系统编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int SystemEdit(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 系统删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        int SystemDelete(string xmlCredentials, int id);
        /// <summary>
        /// 系统名称是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool SystemNameIsRepeat(string xmlCredentials, string name, int id);
        /// <summary>
        /// 系统编码是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool SystemCodeIsRepeat(string xmlCredentials, string code, int id);


    }
}


