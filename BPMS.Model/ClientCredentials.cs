using BPMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.Model
{
    /// <summary>
    /// 客户凭证
    /// </summary>
    public class ClientCredentials
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get { return Consts.ConstValue.SystemCode; } }
        /// <summary>
        /// 系统ID
        /// </summary>
        public int SystemId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
    }
}
