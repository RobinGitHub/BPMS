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
    public partial class BPMSServiceRef
    {
        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public DataTable OrganGetList()
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.OrganGetList(User.Current.Credentials.ToXmlString());
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
        public Organization OrganGetModel(int id)
        {
            string rlt = TryCatchCore<string>(() =>
            {
                return _proxy.OrganGetList(User.Current.Credentials.ToXmlString());
            });
            return rlt.ToModel<Organization>();
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
        public int OrganAdd(Organization model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.OrganAdd(User.Current.Credentials.ToXmlString(), xmlModel);
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
        public int OrganEdit(Organization model)
        {
            string xmlModel = model.ToXmlString();
            return TryCatchCore<int>(() =>
            {
                return _proxy.OrganAdd(User.Current.Credentials.ToXmlString(), xmlModel);
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
        public int OrganDelete(int id)
        {
            return TryCatchCore<int>(() =>
            {
                return _proxy.OrganDelete(User.Current.Credentials.ToXmlString(), id);
            });
        }
        #endregion

        #region 名称是否重复
        /// <summary>
        /// 名称是否重复
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool OrganNameIsRepeat(int parentId, string name, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.OrganNameIsRepeat(User.Current.Credentials.ToXmlString(), parentId, name, id);
            });
        }
        #endregion

        #region 编码是否重复
        /// <summary>
        /// 编码是否重复
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns>true 重复 false 不重复</returns>
        public bool OrganCodeIsRepeat(int parentId, string code, int id)
        {
            return TryCatchCore<bool>(() =>
            {
                return _proxy.OrganCodeIsRepeat(User.Current.Credentials.ToXmlString(), parentId, code, id);
            });
        }
        #endregion
    }
}
