using System.ServiceModel;

namespace BPMS.Contracts 
{ 
    /// <summary> 
    /// 组织架构
    /// </summary>  
    public partial interface IService
    {
        /// <summary>
        /// 获取组织架构列表
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <returns></returns>
        [OperationContract]
        string OrganGetList(string xmlCredentials);
        /// <summary>
        /// 获取组织架构对象
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string OrganGetModel(string xmlCredentials, int id);
        /// <summary>
        /// 组织架构添加
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int OrganAdd(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 组织架构编辑
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        [OperationContract]
        int OrganEdit(string xmlCredentials, string xmlModel);
        /// <summary>
        /// 组织架构删除
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        int OrganDelete(string xmlCredentials, int id);
        /// <summary>
        /// 组织架构名称是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool OrganNameIsRepeat(string xmlCredentials, int parentId, string name, int id);
        /// <summary>
        /// 组织架构编码是否重复
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        [OperationContract]
        bool OrganCodeIsRepeat(string xmlCredentials, int parentId, string code, int id);
    }
} 


