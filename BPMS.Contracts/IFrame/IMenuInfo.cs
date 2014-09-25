using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.Model;
using System.ServiceModel;

namespace BPMS.Contracts 
{ 
    /// <summary> 
    /// 
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string MenuGetList(string xmlCredentials, int systemId, int parentId);
        /// <summary>
        /// 获取菜单对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string MenuGetModel(string xmlCredentials, int id);
        /// <summary>
        /// 菜单添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int MenuAdd(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 菜单编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int MenuEdit(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 菜单删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        int MenuDelete(string xmlCredentials, int id);
        /// <summary>
        /// 菜单名称是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool MenuNameIsRepeat(string xmlCredentials, int systemId, string name, int id);
        /// <summary>
        /// 菜单编码是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool MenuCodeIsRepeat(string xmlCredentials, int systemId, string code, int id);
    }
} 


