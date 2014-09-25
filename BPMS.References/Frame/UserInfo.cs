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
    /// 用户信息
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="searchType">
        /// 1=编号、2=账号、3=姓名
        /// </param>
        /// <returns></returns>
        public DataTable UserGetList(int searchType, string keyWord, int isEnable, int pageIndex, int pageSize, out int count)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            count = 0;
            try
            {
                string rlt = _proxy.UserGetList(out count, User.Current.Credentials.ToXmlString(), searchType, keyWord, isEnable, pageIndex, pageSize);
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

        #region 获取用户对象
        /// <summary>
        /// 获取用户对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserInfo UserGetModel(int id)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.UserGetModel(User.Current.Credentials.ToXmlString(), id);
            });
            return rlt.ToModel<UserInfo>();
        }
        #endregion

        #region 用户添加
        /// <summary>
        /// 用户添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0 失败
        /// 1 成功
        /// 17 账号重复
        /// 13 编码重复
        /// </returns>
        public int UserAdd(UserInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.UserAdd(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }
        #endregion

        #region 用户编辑
        /// <summary>
        /// 用户编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 11当前对象已不存在
        /// 17账号重复
        /// 13编码重复
        /// </returns>
        public int UserEdit(UserInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.UserEdit(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }
        #endregion

        #region 用户名称是否重复
        /// <summary>
        /// 用户名称是否重复
        /// </summary>
        /// <param name="account"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool UserAccountIsRepeat(string account, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.UserAccountIsRepeat(User.Current.Credentials.ToXmlString(), account, id);
            });
        }
        #endregion

        #region 用户编码是否重复
        /// <summary>
        /// 用户编码是否重复
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool UserCodeIsRepeat(string code, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.UserCodeIsRepeat(User.Current.Credentials.ToXmlString(), code, id);
            });
        }
        #endregion

        #region 重置用户密码
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="xmlCredentials"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UserPasswordReset(int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.UserPasswordReset(User.Current.Credentials.ToXmlString(), id);
            });
        }
        #endregion
    }
}
