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
        /// 获取员工列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="searchType">1=工号、2=姓名</param>
        /// <returns></returns>
        [OperationContract]
        string EmployeeGetList(string xmlCredentials, int category, int orgaId, int searchType, string keyWord, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 获取员工对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string EmployeeGetModel(string xmlCredentials, int id);
        /// <summary>
        /// 员工添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int EmployeeAdd(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 员工编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int EmployeeEdit(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 员工编码是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool EmployeeCodeIsRepeat(string xmlCredentials, string code, int id);
    }
} 


