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
    /// IP黑名单
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public DataTable IPBlackGetList()
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.IPBlackGetList(User.Current.Credentials.ToXmlString());
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
        public IPBlacklist IPBlackGetModel(int id)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.EmployeeGetModel(User.Current.Credentials.ToXmlString(), id);
            });
            return rlt.ToModel<IPBlacklist>();
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 1 成功
        /// 0 失败
        /// </returns>
        public int IPBlackAdd(IPBlacklist model)
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
        /// 11当前对象已不存在
        /// </returns>
        public int IPBlackEdit(IPBlacklist model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.IPBlackEdit(User.Current.Credentials.ToXmlString(), xmlModel);
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
        /// </returns>
        public int IPBlackDelete(int id)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.IPBlackDelete(User.Current.Credentials.ToXmlString(), id);
            });
        }
        #endregion
    }
}
