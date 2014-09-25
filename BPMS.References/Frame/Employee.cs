using BPMS.Common;
using BPMS.Model;
using BPMS.References.BPMService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace BPMS.References
{
    /// <summary>
    /// 员工
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 员工列表
        /// <summary>
        /// 员工列表
        /// </summary>
        /// <param name="category"></param>
        /// <param name="orgaId"></param>
        /// <param name="searchType">1=工号、2=姓名</param>
        /// <param name="keyWord"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable EmployeeGetList(int category, int orgaId, int searchType, string keyWord, int pageIndex, int pageSize, out int count)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            count = 0;
            try
            {
                string rlt = _proxy.EmployeeGetList(out count, User.Current.Credentials.ToXmlString(), category, orgaId, searchType, keyWord, pageIndex, pageSize);
                return ZipHelper.DecompressDataTable(rlt);
            }
            catch (EndpointNotFoundException endPointEx)
            {
                throw endPointEx.InnerException;
            }
            catch (Exception ex)
            {
                if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                    CreateOpenClient();
                throw ex;
            }
        }
        #endregion

        #region 获取员工对象
        /// <summary>
        /// 获取员工对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee EmployeeGetModel(int id)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.EmployeeGetModel(User.Current.Credentials.ToXmlString(), id);
            });
            return rlt.ToModel<Employee>();
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0 失败
        /// 1 成功
        /// 13 编码重复
        /// </returns>
        public int EmployeeAdd(Employee model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.EmployeeAdd(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 13编码重复
        /// </returns>
        public int EmployeeEdit(Employee model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.EmployeeEdit(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }
        #endregion

        #region 工号是否重复
        /// <summary>
        /// 工号是否重复
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool EmployeeCodeIsRepeat(string code, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.EmployeeCodeIsRepeat(User.Current.Credentials.ToXmlString(), code, id);
            });
        }
        #endregion
    }
}
