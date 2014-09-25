using BPMS.Model;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace BPMS.DAL
{
    public class TranContext : BPMSEntities, IDisposable
    {
        public TranContext()
        {
            IsDisposed = false;
        }

        public bool IsDisposed
        {
            get;
            set;
        }
        private IDbTransaction Tran
        {
            get;
            set;
        }
        void IDisposable.Dispose()
        {
            if (!IsDisposed)
            {
                try
                {
                    if (this.Tran != null)
                    {
                        this.Tran.Rollback();
                    }
                    if (this.Database.Connection != null && this.Database.Connection.State != System.Data.ConnectionState.Closed)
                        this.Database.Connection.Close();
                    base.Dispose(true);
                }
                catch { }
            }
        }

        #region 静态方法
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public static TranContext BeginTran()
        {
            TranContext ctx = new TranContext();
            ctx.IsDisposed = false;
            ctx.Database.Connection.Open();
            DbConnection con = ((IObjectContextAdapter)ctx).ObjectContext.Connection;
            ctx.Tran = con.BeginTransaction();
            return ctx;
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="ctx"></param>
        public static void Commit(TranContext ctx)
        {
            ctx.Tran.Commit();
            ctx.Database.Connection.Close();
            ctx.Dispose(true);
            ctx.IsDisposed = true;
        }
        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="ctx"></param>
        public static void Rollback(TranContext ctx)
        {
            ctx.Tran.Rollback();
            ctx.Database.Connection.Close();
            ctx.Dispose(true);
            ctx.IsDisposed = true;
        }
        #endregion
    }
}
