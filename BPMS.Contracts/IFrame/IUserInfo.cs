using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.Model;
using System.ServiceModel;

namespace BPMS.Contracts 
{ 
    /// <summary> 
    /// 用户
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="searchType">
        /// 1=编号、2=账号、3=姓名
        /// </param>
        /// <returns></returns>
        [OperationContract]
        string UserGetList(string xmlCredentials, int searchType, string keyWord, int isEnable, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 获取用户对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string UserGetModel(string xmlCredentials, int id);
        /// <summary>
        /// 用户添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int UserAdd(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 用户编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int UserEdit(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 用户名称是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="account"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool UserAccountIsRepeat(string xmlCredentials, string account, int id);
        /// <summary>
        /// 用户编码是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool UserCodeIsRepeat(string xmlCredentials, string code, int id);
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        bool UserPasswordReset(string xmlCredentials, int id);
    }
} 


