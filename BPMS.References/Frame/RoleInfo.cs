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
    /// 角色
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable RoleGetList(int systemId)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.RoleGetList(User.Current.Credentials.ToXmlString(), systemId);
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
        public RoleInfo RoleGetModel(int id)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.DataDictGetModel(User.Current.Credentials.ToXmlString(), id);
            });
            return rlt.ToModel<RoleInfo>();
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
        public int RoleAdd(RoleInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.DataDictAdd(User.Current.Credentials.ToXmlString(), xmlModel);
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
        public int RoleEdit(RoleInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.DataDictAdd(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 11当前对象已不存在
        /// 14当前数据已经使用，不允许删除
        /// </returns>
        public int RoleDelete(int id)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.DataDictDelete(User.Current.Credentials.ToXmlString(), id);
            });
        }
        #endregion

        #region 名称是否重复
        /// <summary>
        /// 名称是否重复
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool RoleNameIsRepeat(int systemId, string name, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.RoleNameIsRepeat(User.Current.Credentials.ToXmlString(), systemId, name, id);
            });
        }
        #endregion

        #region 编码是否重复
        /// <summary>
        /// 编码是否重复
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool RoleCodeIsRepeat(int systemId, string code, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.RoleCodeIsRepeat(User.Current.Credentials.ToXmlString(), systemId, code, id);
            });
        }
        #endregion
    }
}
