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
    /// 角色
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string RoleGetList(string xmlCredentials, int systemId);
        /// <summary>
        /// 获取角色对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string RoleGetModel(string xmlCredentials, int id);
        /// <summary>
        /// 角色添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int RoleAdd(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 角色编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int RoleEdit(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 角色删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        int RoleDelete(string xmlCredentials, int id);
        /// <summary>
        /// 角色名称是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool RoleNameIsRepeat(string xmlCredentials, int systemId, string name, int id);
        /// <summary>
        /// 角色编码是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool RoleCodeIsRepeat(string xmlCredentials, int systemId, string code, int id);

    }
} 


