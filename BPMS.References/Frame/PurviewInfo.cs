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
    /// 权限
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 模块列表
        /// <summary>
        /// 模块列表
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="moduleName"></param>
        /// <param name="moduleCode"></param>
        /// <param name="isEnable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable PurviewGetModuleList(int systemId, string moduleName, string moduleCode, int isEnable, int pageIndex, int pageSize, out int count)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            count = 0;
            try
            {
                string rlt = _proxy.PurviewGetModuleList(out count, User.Current.Credentials.ToXmlString(), systemId, moduleName, moduleCode, isEnable, pageIndex, pageSize);
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

        #region 功能列表
        /// <summary>
        /// 功能列表
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="moduleId"></param>
        /// <param name="functionName"></param>
        /// <param name="functionCode"></param>
        /// <param name="isEnable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable PurviewGetFunctionList(int systemId, int moduleId, string functionName, string functionCode, int isEnable, int pageIndex, int pageSize, out int count)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            count = 0;
            try
            {
                string rlt = _proxy.PurviewGetFunctionList(out count, User.Current.Credentials.ToXmlString(), systemId, moduleId, functionName, functionCode, isEnable, pageIndex, pageSize);
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

        #region 操作列表
        /// <summary>
        /// 操作列表
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="moduleId"></param>
        /// <param name="functionId"></param>
        /// <param name="actionName"></param>
        /// <param name="actionCode"></param>
        /// <param name="isEnable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable PurviewGetActionList(int systemId, int moduleId, int functionId, string actionName, string actionCode, int isEnable, int pageIndex, int pageSize, out int count)
        {
            if (((ServiceClient)_proxy).State == CommunicationState.Faulted)
                CreateOpenClient();
            count = 0;
            try
            {
                string rlt = _proxy.PurviewGetActionList(out count, User.Current.Credentials.ToXmlString(), systemId, moduleId, functionId, actionName, actionCode, isEnable, pageIndex, pageSize);
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

        #region 权限列表根据ParentID
        /// <summary>
        /// 权限列表根据ParentID
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public DataTable PurviewGetListByParentId(int systemId, int parentId)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.PurviewGetListByParentId(User.Current.Credentials.ToXmlString(), systemId, parentId);
            });
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 获取对象
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PurviewInfo PurviewGetModel(int id)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.PurviewGetModel(User.Current.Credentials.ToXmlString(), id);
            });
            return rlt.ToModel<PurviewInfo>();
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
        /// 12 名称重复
        /// 13 编码重复
        /// </returns>
        public int PurviewAdd(PurviewInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.PurviewAdd(User.Current.Credentials.ToXmlString(), xmlModel);
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
        /// 11当前对象已不存在
        /// 12名称重复
        /// 13编码重复
        /// </returns>
        public int PurviewEdit(PurviewInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.PurviewAdd(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 11当前对象已不存在
        /// 14当前数据已经使用，不允许删除
        /// </returns>
        public int PurviewDelete(int id)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.PurviewDelete(User.Current.Credentials.ToXmlString(), id);
            });
        }
        #endregion

        #region 名称是否重复
        /// <summary>
        /// 名称是否重复
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="parentId"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PurviewNameIsRepeat(int systemId, int parentId, string name, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.PurviewNameIsRepeat(User.Current.Credentials.ToXmlString(), systemId, parentId, name, id);
            });
        }
        #endregion

        #region 编码是否重复
        /// <summary>
        /// 编码是否重复
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="parentId"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PurviewCodeIsRepeat(int systemId, int parentId, string code, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.PurviewCodeIsRepeat(User.Current.Credentials.ToXmlString(), systemId, parentId, code, id);
            });
        }
        #endregion
    }
}
