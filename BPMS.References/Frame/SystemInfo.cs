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
    /// 系统信息
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 获取系统列表
        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <returns></returns>
        public DataTable SystemGetList()
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.SystemGetList(User.Current.Credentials.ToXmlString());
            });
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 获取系统对象
        /// <summary>
        /// 获取系统对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SystemInfo SystemGetModel(int id)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.SystemGetModel(User.Current.Credentials.ToXmlString(), id);
            });
            return rlt.ToModel<SystemInfo>();
        }
        #endregion

        #region 系统添加
        /// <summary>
        /// 系统添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0 失败
        /// 1 成功
        /// 12 名称重复
        /// 13 编码重复
        /// </returns>
        public int SystemAdd(SystemInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.SystemAdd(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }
        #endregion

        #region 系统编辑
        /// <summary>
        /// 系统编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 11当前对象已不存在
        /// 12名称重复
        /// 13编码重复
        /// </returns>
        public int SystemEdit(SystemInfo model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.SystemEdit(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }
        #endregion

        #region 系统删除
        /// <summary>
        /// 系统删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 11当前对象已不存在
        /// 14当前数据已经使用，不允许删除
        /// </returns>
        public int SystemDelete(int id)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.DataDictDelete(User.Current.Credentials.ToXmlString(), id);
            });
        }
        #endregion

        #region 系统名称是否重复
        /// <summary>
        /// 系统名称是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool SystemNameIsRepeat(string name, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.SystemNameIsRepeat(User.Current.Credentials.ToXmlString(), name, id);
            });
        }
        #endregion

        #region 系统编码是否重复
        /// <summary>
        /// 系统编码是否重复
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool SystemCodeIsRepeat(string code, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.SystemCodeIsRepeat(User.Current.Credentials.ToXmlString(), code, id);
            });
        }
        #endregion
    }
}
