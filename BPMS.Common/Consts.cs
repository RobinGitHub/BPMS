using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.Common
{
    public class Consts
    {
        public class ConstValue
        {
            /// <summary>
            /// 当前系统编码
            /// </summary>
            public const string SystemCode = "BPMS";
            /// <summary>
            /// 超级管理员账号
            /// </summary>
            public const string SuperAdminAccount = "admin";
            ///// <summary>
            ///// 会员卡号长度
            ///// </summary>
            //public const int CardLength = 9;
            ///// <summary>
            ///// 客户端与服务端连接的识别Key
            ///// </summary>
            //public const string AccessKey = "this is only key";
        }

        public class CookieKey
        {
            /// <summary>
            /// 当前登录用户名
            /// </summary>
            public const string Account = "Account";
            /// <summary>
            /// 当前用户角色编码
            /// </summary>
            public const string RoleCode = "RoleCode";
        }

        public class CacheKey
        {
            /// <summary>
            /// IM消息集合
            /// </summary>
            public const string IMMsgList = "IMMsgList";
            /// <summary>
            /// IM客户端集合
            /// </summary>
            public const string IMClientList = "IMClientList";

            /// <summary>
            /// 字典表缓存
            /// </summary>
            public const string DICTIONARY = "Dictionary";

            /// <summary>
            /// 权限服务缓存
            /// </summary>
            public const string PurviewService = "PurviewService";
            /// <summary>
            /// 菜单服务缓存
            /// </summary>
            public const string MenuService = "MenuService";

            /// <summary>
            /// 用户权限缓存
            /// </summary>
            public const string BaseService_Purview = "BaseServicePurview";
            /// <summary>
            /// 用户权限菜单缓存
            /// </summary>
            public const string BaseService_UserMenus = "BaseServiceUserMenus";
        }

        public class ConfigKey
        {
            /// <summary>
            /// 数据访问层类名KEY
            /// </summary>
            public const string DAL = "DAL";
            /// <summary>
            /// 样式模板KEY
            /// </summary>
            public const string Style = "Style";
            /// <summary>
            /// 调用服务时传入的系统级账号
            /// </summary>
            public const string ServiceAccount = "ServiceAccount";
            /// <summary>
            /// 调用服务时传入的系统级密码
            /// </summary>
            public const string ServicePassword = "ServicePassword";
            /// <summary>
            /// 权限验证WebService地址
            /// </summary>
            public const string PruviewServiceUrl = "PruviewServiceUrl";
            /// <summary>
            /// 获取菜单WebService地址
            /// </summary>
            public const string MenuServiceUrl = "MenuServiceUrl";
            /// <summary>
            /// 是否显示默认菜单
            /// </summary>
            public const string ShowDefaultMenus = "ShowDefaultMenus";
            /// <summary>
            /// 站点域名
            /// </summary>
            public const string HostNameToCheck = "HostNameToCheck";
        }

        /// <summary>
        /// 返回码
        /// </summary>
        public class ReturnCodeMessage
        {
            /// <summary>
            /// 返回编码信息
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public static string GetMessage(int code)
            {
                string message = string.Empty;
                switch (code)
                { 
                    case 0:
                        message = "操作失败，请联系管理员！";
                        break;
                    case 1:
                        message = "操作成功！";
                        break;

                    #region 登录判断
                    case 2:
                        message = "账号不存在！";
                        break;
                    case 3:
                        message = "用户已禁用！";
                        break;
                    case 4:
                        message = "密码错误！";
                        break;
                    case 5:
                        message = "当前IP不许登录！";
                        break;
                    case 6:
                        message = "当前用户没有任何权限！";
                        break;
                    #endregion

                    case 11:
                        message = "当前对象已不存在！";
                        break;
                    case 12:
                        message = "名称重复！";
                        break;
                    case 13:
                        message = "编码重复！";
                        break;
                    case 14:
                        message = "当前数据已经使用，不允许删除！";
                        break;
                    case 15:
                        message = "该成员不能删除，因为该成员只有一个角色！";
                        break;
                    case 16:
                        message = "当前数据有子集，不允许删除！";
                        break;
                    case 17:
                        message = "账号重复！";
                        break;
                    default:
                        break;
                }
                return message;
            }
        }
    }
}
