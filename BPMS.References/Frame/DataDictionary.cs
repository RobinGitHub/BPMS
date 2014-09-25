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
    /// 数据字典
    /// </summary>
    public partial class BPMSServiceRef
    {
        #region 获取数据字典列表
        /// <summary>
        /// 获取数据字典列表
        /// </summary>
        /// <returns></returns>
        public DataTable DataDictGetList(int systemId, int dictType)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.DataDictGetList(User.Current.Credentials.ToXmlString(), systemId, dictType);
            });
            return ZipHelper.DecompressDataTable(rlt);
        }
        #endregion

        #region 获取数据字典对象
        /// <summary>
        /// 获取数据字典对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataDictionary DataDictGetModel(int id)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.DataDictGetModel(User.Current.Credentials.ToXmlString(), id);
            });
            return rlt.ToModel<DataDictionary>();
        }
        #endregion

        #region 数据字典添加
        /// <summary>
        /// 数据字典添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0 失败
        /// 1 成功
        /// 12 名称重复
        /// 13 编码重复
        /// </returns>
        public int DataDictAdd(DataDictionary model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.DataDictAdd(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }

        #endregion

        #region 数据字典编辑
        /// <summary>
        /// 数据字典编辑
        /// </summary>
        /// <param name="xmlModel"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 11当前对象已不存在
        /// 12名称重复
        /// 13编码重复
        /// </returns>
        public int DataDictEdit(DataDictionary model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.DataDictEdit(User.Current.Credentials.ToXmlString(), xmlModel);
            });
        }
        #endregion

        #region 数据字典删除
        /// <summary>
        /// 数据字典删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// 11当前对象已不存在
        /// 14当前数据已经使用，不允许删除
        /// </returns>
        public int DataDictDelete(int id)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.DataDictDelete(User.Current.Credentials.ToXmlString(), id);
            });
        }
        #endregion

        #region 数据字典名称是否重复
        /// <summary>
        /// 数据字典名称是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool DataDictNameIsRepeat(int dictType, string name, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.DataDictNameIsRepeat(User.Current.Credentials.ToXmlString(), dictType, name, id);
            });
        }
        #endregion

        #region 数据字典编码是否重复
        /// <summary>
        /// 数据字典编码是否重复
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool DataDictCodeIsRepeat(int dictType, string code, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.DataDictCodeIsRepeat(User.Current.Credentials.ToXmlString(), dictType, code, id);
            });
        }
        #endregion
    }
}
