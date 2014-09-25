using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPMS.Common
{
    /// <summary>
    /// Cache 的辅助类
    /// </summary>
    public class CacheHelper
    {
        private readonly static ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();

        #region Add
        /// <summary>
        /// 无过期的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Add(string key, object value)
        {
            cache.Add(key, value);
            AddKey(key);
        }

        /// <summary>
        /// 绝对时间的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteTime"></param>
        public static void Add(string key, object value, DateTime absoluteTime)
        {
            cache.Add(key, value, CacheItemPriority.Normal, null, new AbsoluteTime(absoluteTime));
            AddKey(key);
        }

        /// <summary>
        /// 相对时间的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingTime"></param>
        public static void Add(string key, object value, TimeSpan slidingTime)
        {
            cache.Add(key, value, CacheItemPriority.Normal, null, new SlidingTime(slidingTime));
            AddKey(key);
        }

        /// <summary>
        /// 依赖文件的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependFilePath"></param>
        public static void Add(string key, object value, string dependFilePath)
        {
            Add(key, value, null, dependFilePath);
            AddKey(key);
        }

        /// <summary>
        /// 依赖文件缓存(包含移除刷新事件)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <param name="dependFilePath"></param>
        public static void Add(string key, object value, ICacheItemRefreshAction action, string dependFilePath)
        {
            FileDependency depen = new FileDependency(dependFilePath);
            cache.Add(key, value, CacheItemPriority.Normal, action, depen);
            AddKey(key);
        }
        #endregion

        #region Get Key List
        private const string CACHEKEYS = "Cache_Key_CacheKeys";
        private static List<string> CacheKeyList
        {
            get
            {
                var tmp = Get(CACHEKEYS);
                if (Get<List<string>>(CACHEKEYS) == null)
                {
                    tmp = new List<string>();
                    Add(CACHEKEYS, tmp);
                }
                return tmp as List<string>;
            }
            set
            {
                Add(CACHEKEYS, value);
            }
        }
        private static void AddKey(string key)
        {
            if (key != CACHEKEYS && CacheKeyList.Contains(key) == false)
            {
                CacheKeyList.Add(key);
            }
        }
        public static List<string> GetCacheKeyList(int pageIndex,int pageSize,  out int count, string keyWord)
        {
            count = 0;
            List<string> keys = new List<string>();
            var list = CacheKeyList;
            if (string.IsNullOrEmpty(keyWord) == false)
            {
                list = list.Where(t => t.ToLower().Contains(keyWord.ToLower())).ToList();
            }
            List<string> keystoremove = new List<string>();
            foreach (var key in list)
            {
                if (Get(key) != null)
                {
                    keys.Add(key);
                }
                else
                {
                    keystoremove.Add(key);
                }
            }
            foreach (var key in keystoremove)
            {
                CacheKeyList.Remove(key);
            }
            count = keys.Count;
            keys = keys.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            return keys;
        }
        #endregion

        #region Get
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return cache.GetData(key);
        }

        public static T Get<T>(string key)
        {
            return (T)cache.GetData(key);
        }

        private static object lockKey = new object();
        /// <summary>
        /// 根据传入的委托函数来设置缓存(此处的委托无参数,如需参数请使用匿名委托来闭包访问相应上下文)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getDataFunc"></param>
        /// <returns></returns>
        public static object Get(string key, Func<object> getDataFunc)
        {
            if (cache.Contains(key))
            {
                return Get(key);
            }

            object value = null;
            lock (lockKey)
            {
                value = Get(key);
                if (value == null)
                {
                    value = getDataFunc();
                    Add(key, value);
                }
            }

            return value;
        }

        //public static object Get(string key, string sql, CacheDataEnum cacheEnum)
        //{
        //    if (cache.Contains(key))
        //    {
        //        return Get(key);
        //    }

        //    object value = null;

        //    if (cacheEnum == CacheDataEnum.DataSet)
        //    {
        //        value = DataAccessManger.DefaultConnection.ExecuteDataSet(System.Data.CommandType.Text, sql);
        //    }
        //    else
        //    {
        //        value = DataAccessManger.DefaultConnection.ExecuteScalar(System.Data.CommandType.Text, sql);
        //    }

        //    Add(key, value, DateTime.Now.AddMinutes(10));
        //    return Get(key);
        //}
        #endregion

        #region Update
        public static void Update(string key, object val)
        {

        }
        #endregion

        #region Remove
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            cache.Remove(key);
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            cache.Flush();
        }
        #endregion

        #region 会话缓存
        /// <summary>
        /// 添加会话缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddContext(object key, object value)
        {
            HttpContext.Current.Items[key] = value;
        }

        /// <summary>
        /// 获取会话缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetContext(object key)
        {
            return HttpContext.Current.Items[key];
        }

        /// <summary>
        /// 获取会话缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetContext<T>(object key)
        {
            return (T)HttpContext.Current.Items[key];
        }

        public static void RemoveContext(object key)
        {
            HttpContext.Current.Items.Remove(key);
        }
        #endregion
    }

    public enum CacheDataEnum
    {
        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        DataSet = 0,
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        Object = 1
    }
}
