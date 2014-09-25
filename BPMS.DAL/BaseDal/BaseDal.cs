using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BPMS.Common;
using BPMS.Model;

namespace BPMS.DAL
{
    public class BaseDAL<T> : IDisposable
        where T : class, new()
    {
        #region Dispose
        public void Dispose()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// 设置事务级别
        /// </summary>
        public void SetNoLock()
        {
            SetNoLock(null);
        }
        /// <summary>
        /// 设置事务级别
        /// </summary>
        public void SetNoLock(BPMSEntities ctx)
        {
            bool newCtx = ctx == null;
            if (newCtx)
                ctx = new BPMSEntities();
            try
            {
                var sql = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";
                ctx.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取最新表主键ID
        /// </summary>
        /// <returns></returns>
        public int GetNewID()
        {
            return this.GetNewID(typeof(T).Name);
        }
        /// <summary>
        /// 获取最新表主键ID
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public int GetNewID(string tableName)
        {
            try
            {
                using (var ctx = new BPMSEntities())
                {
                    return ctx.GetNewID(tableName).FirstOrDefault().Value;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
                return 0;
            }
        }


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Insert(T model)
        {
            return Insert(null, model);
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(BPMSEntities ctx, T model)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            bool blRlt = false;
            try
            {
                ctx.Set<T>().Add(model);
                ctx.SaveChanges();
                blRlt = true;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
            return blRlt;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        public bool Insert(IList<T> models)
        {
            return Insert(null, models);
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        public bool Insert(BPMSEntities ctx, IList<T> models)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            bool blRlt = false;
            try
            {
                foreach (var model in models)
                {
                    ctx.Set<T>().Add(model);
                }
                ctx.SaveChanges();
                blRlt = true;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
            return blRlt;
        }

        public virtual bool Update(T model)
        {
            return Update(null, model);
        }
        public bool Update(BPMSEntities ctx, T model)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            bool blRlt = false;
            DbSet<T> dbSet = ctx.Set<T>();
            try
            {
                DbEntityEntry<T> entry = ctx.Entry(model);
                if (entry.State == EntityState.Detached)
                {
                    dbSet.Attach(model);
                    entry.State = EntityState.Modified;
                }

                ctx.SaveChanges();
                blRlt = true;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
            return blRlt;
        }

        public bool Delete(T model)
        {
            return Delete(null, model);
        }
        public bool Delete(BPMSEntities ctx, T model)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            bool blRlt = false;
            try
            {
                ctx.Entry(model).State = EntityState.Deleted;
                ctx.SaveChanges();
                blRlt = true;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
            return blRlt;
        }

        public bool Delete(Expression<Func<T, bool>> condition)
        {
            return Delete(null, condition);
        }
        public bool Delete(BPMSEntities ctx, Expression<Func<T, bool>> condition)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            bool blRlt = false;
            try
            {
                IList<T> list = ctx.Set<T>().Where(condition).ToList();
                foreach (var data in list)
                {
                    ctx.Entry(data).State = EntityState.Deleted;
                }
                ctx.SaveChanges();
                blRlt = true;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
            return blRlt;
        }

        public T GetModel(Expression<Func<T, bool>> condition)
        {
            return GetModel(null, condition);
        }
        public T GetModel(BPMSEntities ctx, Expression<Func<T, bool>> condition)
        {
            var rlt = GetModel(ctx, t => t, condition);
            return rlt == null ? null : (T)rlt;
        }

        public TResult GetModel<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition)
        {
            return GetModel<TResult>(null, selector, condition);
        }
        public TResult GetModel<TResult>(BPMSEntities ctx, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                var rlt = ctx.Set<T>().Where(condition).Select(selector).FirstOrDefault();
                return rlt;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
                return default(TResult);
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        public int GetCount(Expression<Func<T, bool>> condition)
        {
            return GetCount(null, condition);
        }
        public int GetCount(BPMSEntities ctx, Expression<Func<T, bool>> condition)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                return ctx.Set<T>().Count(condition);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
                return 0;
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        public IList GetTop(int count)
        {
            return GetTop(null, count);
        }
        public IList GetTop(BPMSEntities ctx, int count)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                return ctx.Set<T>().Take(count).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
                return null;
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        public IList GetTop(int count, Expression<Func<T, bool>> condition)
        {
            return GetTop(null, count, condition);
        }
        public IList GetTop(BPMSEntities ctx, int count, Expression<Func<T, bool>> condition)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                return ctx.Set<T>().Where(condition).Take(count).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
                return null;
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        public IList GetTop<TKey>(int count, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, bool IsAsc = true)
        {
            return GetTop(null, count, condition, orderBy, IsAsc);
        }
        public IList GetTop<TKey>(BPMSEntities ctx, int count, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                var rlt = ctx.Set<T>().Where(condition);
                if (isAsc)
                {
                    rlt = rlt.OrderBy(orderBy);
                }
                else
                {
                    rlt = rlt.OrderByDescending(orderBy);
                }
                return rlt.Take(count).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
                return null;
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        public IList GetList(Expression<Func<T, bool>> condition)
        {
            BPMSEntities ctx = null;
            return GetList(ctx, condition);
        }
        public IList GetList(BPMSEntities ctx, Expression<Func<T, bool>> condition)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                return ctx.Set<T>().Where(condition).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
                return null;
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        public IList GetList<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition)
        {
            return GetList(null, selector, condition);
        }
        public IList GetList<TResult>(BPMSEntities ctx, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                return ctx.Set<T>().Where(condition).Select(selector).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.ToString());
                return null;
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        public IList GetList<TResult>(int pageIndex, int pageSize, out int count, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition)
        {
            return GetList<TResult>(null, pageIndex, pageSize, out count, selector, condition);
        }
        public IList GetList<TResult>(BPMSEntities ctx, int pageIndex, int pageSize, out int count, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                var models = ctx.Set<T>();
                count = models.Count(condition);
                return models.Where(condition).Select(selector).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                count = 0;
                LogHelper.Log.Info(ex.ToString());
                return null;
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        public IList GetList<TKey>(int pageIndex, int pageSize, out int count, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> OrderBy, bool IsAsc = true)
        {
            return GetList<TKey>(null, pageIndex, pageSize, out  count, condition, OrderBy, IsAsc);
        }
        public IList GetList<TKey>(BPMSEntities ctx, int pageIndex, int pageSize, out int count, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                var models = ctx.Set<T>();
                count = models.Count(condition);
                var rlt = models.Where(condition);
                if (isAsc)
                {
                    rlt = rlt.OrderBy(orderBy);
                }
                else
                {
                    rlt = rlt.OrderByDescending(orderBy);
                }
                rlt = rlt.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return rlt.ToList();
            }
            catch (Exception ex)
            {
                count = 0;
                LogHelper.Log.Info(ex.ToString());
                return null;
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }

        public IList GetList<TResult, TKey>(int pageIndex, int pageSize, out int count, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            return GetList<TResult, TKey>(null, pageIndex, pageSize, out  count, selector, condition, orderBy, isAsc);
        }
        public IList GetList<TResult, TKey>(BPMSEntities ctx, int pageIndex, int pageSize, out int count, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            bool newCtx = ctx == null;
            if (newCtx)
            {
                ctx = new BPMSEntities();
            }
            try
            {
                var models = ctx.Set<T>();
                count = models.Count(condition);
                if (isAsc)
                {
                    return models.Where(condition).OrderBy(orderBy).Select(selector).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return models.Where(condition).OrderByDescending(orderBy).Select(selector).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                count = 0;
                LogHelper.Log.Info(ex.ToString());
                return null;
            }
            finally
            {
                if (newCtx)
                {
                    ctx.Dispose();
                }
            }
        }
        #endregion


    }

}
