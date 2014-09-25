using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.Model;
using System.ServiceModel;

namespace BPMS.Contracts 
{ 
    /// <summary> 
    /// 字典
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取数据字典列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string DataDictGetList(string xmlCredentials, int systemId, int dictType);
        /// <summary>
        /// 获取数据字典对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string DataDictGetModel(string xmlCredentials, int id);
        /// <summary>
        /// 数据字典添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int DataDictAdd(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 数据字典编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int DataDictEdit(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 数据字典删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        int DataDictDelete(string xmlCredentials, int id);
        /// <summary>
        /// 数据字典名称是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool DataDictNameIsRepeat(string xmlCredentials, int dictType, string name, int id);
        /// <summary>
        /// 数据字典编码是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool DataDictCodeIsRepeat(string xmlCredentials, int dictType, string code, int id);
        
    }
} 


