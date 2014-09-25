using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.Model;
using System.ServiceModel;

namespace BPMS.Contracts 
{ 
    /// <summary> 
    /// 权限
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取模块权限列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string PurviewGetModuleList(string xmlCredentials, int systemId, string moduleName, string moduleCode, int isEnable, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 获取功能权限列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string PurviewGetFunctionList(string xmlCredentials, int systemId, int moduleId, string functionName, string functionCode, int isEnable, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 获取操作权限列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string PurviewGetActionList(string xmlCredentials, int systemId, int moduleId, int functionId, string actionName, string actionCode, int isEnable, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 获取权限下启用的模块功能与操作
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string PurviewGetListByParentId(string xmlCredentials, int systemId, int parentId);
        /// <summary>
        /// 获取权限对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string PurviewGetModel(string xmlCredentials, int id);
        /// <summary>
        /// 权限添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int PurviewAdd(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 权限编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int PurviewEdit(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 权限删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        int PurviewDelete(string xmlCredentials, int id);
        /// <summary>
        /// 权限名称是否重复 同一级不允许重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool PurviewNameIsRepeat(string xmlCredentials, int systemId, int parentId, string name, int id);
        /// <summary>
        /// 权限编码是否重复 同一级不允许重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool PurviewCodeIsRepeat(string xmlCredentials, int systemId, int parentId, string code, int id);
    }
} 


