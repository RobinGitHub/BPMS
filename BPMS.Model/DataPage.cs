using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.Model
{
    /// <summary>
    /// 数据翻页
    /// </summary>
    public class DataPage
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 要跳转页
        /// </summary>
        public int Skip
        {
            get { return (PageIndex - 1) * PageSize; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage 
        {
            get
            {
                int totalPage = 0;
                if (Count % PageSize == 0)
                    totalPage = Count / PageSize;
                else
                    totalPage = (int)Math.Ceiling((double)(Count / PageSize));
                return totalPage;
            }
        }

    }
}
