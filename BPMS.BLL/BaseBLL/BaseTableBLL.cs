using BPMS.DAL;
using BPMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BPMS.BLL
{
    public class BaseTableBLL<TModel, TDAL> : BaseBLL
        where TModel : class, new()
        where TDAL : BaseDAL<TModel>, new()
    {
        #region 构造函数
        public BaseTableBLL() { }

        public BaseTableBLL(BllProvider provider)
            : base(provider)
        {

        }
        #endregion

        #region 属性
        protected static readonly TDAL dal = new TDAL();
        #endregion

        #region Methods

        public int GetNewID()
        {
            return dal.GetNewID();
        }

        public int GetNewID(string tableName)
        {
            return dal.GetNewID(tableName);
        }

        public virtual bool Insert(TModel model)
        {
            return dal.Insert(model);
        }

        internal bool Insert(BPMSEntities mc, TModel model)
        {
            return dal.Insert(mc, model);
        }

        public bool Insert(IList<TModel> models)
        {
            return dal.Insert(models);
        }

        internal bool Insert(BPMSEntities mc, IList<TModel> models)
        {
            return dal.Insert(mc, models);
        }

        public virtual bool Update(TModel model)
        {
            return dal.Update(model);
        }

        internal bool Update(BPMSEntities mc, TModel model)
        {
            return dal.Update(mc, model);
        }

        public bool Delete(TModel model)
        {
            return dal.Delete(model);
        }

        internal bool Delete(BPMSEntities mc, TModel model)
        {
            return dal.Delete(mc, model);
        }

        public bool Delete(Expression<Func<TModel, bool>> condition)
        {
            return dal.Delete(condition);
        }

        internal bool Delete(BPMSEntities mc, Expression<Func<TModel, bool>> condition)
        {
            return dal.Delete(mc, condition);
        }

        public TModel GetModel(Expression<Func<TModel, bool>> condition)
        {
            return dal.GetModel(condition);
        }

        internal TModel GetModel(BPMSEntities mc, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetModel(mc, condition);
        }

        public TResult GetModel<TResult>(Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetModel(selector, condition);
        }

        internal TResult GetModel<TResult>(BPMSEntities mc, Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetModel(mc, selector, condition);
        }

        public int GetCount(Expression<Func<TModel, bool>> condition)
        {
            return dal.GetCount(condition);
        }

        internal int GetCount(BPMSEntities mc, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetCount(mc, condition);
        }

        public IList GetTop(int count)
        {
            return dal.GetTop(count);
        }

        internal IList GetTop(BPMSEntities mc, int count)
        {
            return dal.GetTop(mc, count);
        }

        public IList GetTop(int count, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetTop(count, condition);
        }

        internal IList GetTop(BPMSEntities mc, int count, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetTop(mc, count, condition);
        }

        public IList GetTop<TKey>(int count, Expression<Func<TModel, bool>> condition, Expression<Func<TModel, TKey>> orderBy, bool isAsc = true)
        {
            return dal.GetTop(count, condition, orderBy, isAsc);
        }

        internal IList GetTop<TKey>(BPMSEntities mc, int count, Expression<Func<TModel, bool>> condition, Expression<Func<TModel, TKey>> orderBy, bool isAsc = true)
        {
            return dal.GetTop(mc, count, condition, orderBy, isAsc);
        }

        public IList GetList(Expression<Func<TModel, bool>> condition)
        {
            return dal.GetList(condition);
        }

        internal IList GetList(BPMSEntities mc, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetList(mc, condition);
        }

        public IList GetList<TResult>(Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetList(selector, condition);
        }

        internal IList GetList<TResult>(BPMSEntities mc, Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetList(mc, selector, condition);
        }

        public IList GetList<TResult>(int pageIndex, int pageSize, out int count, Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetList(pageIndex, pageSize, out count, selector, condition);
        }

        internal IList GetList<TResult>(BPMSEntities mc, int pageIndex, int pageSize, out int count, Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> condition)
        {
            return dal.GetList(mc, pageIndex, pageSize, out count, selector, condition);
        }

        public IList GetList<TKey>(int pageIndex, int pageSize, out int count, Expression<Func<TModel, bool>> condition, Expression<Func<TModel, TKey>> orderBy, bool isAsc = true)
        {
            return dal.GetList(pageIndex, pageSize, out count, condition, orderBy, isAsc);
        }

        internal IList GetList<TKey>(BPMSEntities mc, int pageIndex, int pageSize, out int count, Expression<Func<TModel, bool>> condition, Expression<Func<TModel, TKey>> orderBy, bool isAsc = true)
        {
            return dal.GetList(mc, pageIndex, pageSize, out count, condition, orderBy, isAsc);
        }

        public IList GetList<TResult, TKey>(int pageIndex, int pageSize, out int count, Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> condition, Expression<Func<TModel, TKey>> orderBy, bool isAsc = true)
        {
            return dal.GetList(pageIndex, pageSize, out count, selector, condition, orderBy, isAsc);
        }

        internal IList GetList<TResult, TKey>(BPMSEntities mc, int pageIndex, int pageSize, out int count, Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> condition, Expression<Func<TModel, TKey>> orderBy, bool isAsc = true)
        {
            return dal.GetList(mc, pageIndex, pageSize, out count, selector, condition, orderBy, isAsc);
        }

        #endregion
    }
}
