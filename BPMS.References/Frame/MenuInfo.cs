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
    /// 菜单
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public DataTable MenuGetList(int systemId, int parentId)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.MenuGetList(User.Current.Credentials.ToXmlString(), systemId, parentId);
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
        public MenuInfo MenuGetModel(int id)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.MenuGetModel(User.Current.Credentials.ToXmlString(), id);
            });
            return rlt.ToModel<MenuInfo>();
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
        public int MenuAdd(MenuInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.MenuAdd(User.Current.Credentials.ToXmlString(), xmlModel);
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
        public int MenuEdit(MenuInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.MenuEdit(User.Current.Credentials.ToXmlString(), xmlModel);
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
        /// 16当前数据有子集，不允许删除
        /// </returns>
        public int MenuDelete(int id)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.MenuDelete(User.Current.Credentials.ToXmlString(), id);
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
        public bool MenuNameIsRepeat(int systemId, string name, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.MenuNameIsRepeat(User.Current.Credentials.ToXmlString(), systemId, name, id);
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
        public bool MenuCodeIsRepeat(int systemId, string code, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.MenuCodeIsRepeat(User.Current.Credentials.ToXmlString(), systemId, code, id);
            });
        }
        #endregion
    }
}
