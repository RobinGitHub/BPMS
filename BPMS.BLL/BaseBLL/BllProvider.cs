using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.BLL
{
    public class BllProvider : IDisposable
    {
        public BllProvider() { }

        #region Provider
        private DataDictionaryBll _DataDictionaryBll;
        /// <summary>
        /// 数据字典
        /// </summary>
        public DataDictionaryBll DataDictionaryBLL
        {
            get
            {
                if (_DataDictionaryBll == null)
                {
                    _DataDictionaryBll = new DataDictionaryBll(this);
                }
                return _DataDictionaryBll;
            }
        }

        private EmployeeBll _EmployeeBll;
        /// <summary>
        /// 员工
        /// </summary>
        public EmployeeBll EmployeeBLL
        {
            get
            {
                if (_EmployeeBll == null)
                {
                    _EmployeeBll = new EmployeeBll(this);
                }
                return _EmployeeBll;
            }
        }

        private IPBlacklistBll _IPBlacklistBll;
        /// <summary>
        /// IP 黑名单
        /// </summary>
        public IPBlacklistBll IPBlacklistBLL
        {
            get
            {
                if (_IPBlacklistBll == null)
                {
                    _IPBlacklistBll = new IPBlacklistBll(this);
                }
                return _IPBlacklistBll;
            }
        }

        private MenuInfoBll _MenuInfoBll;
        /// <summary>
        /// 菜单
        /// </summary>
        public MenuInfoBll MenuInfoBLL
        {
            get
            {
                if (_MenuInfoBll == null)
                {
                    _MenuInfoBll = new MenuInfoBll(this);
                }
                return _MenuInfoBll;
            }
        }

        private OrganizationBll _OrganizationBll;
        /// <summary>
        /// 公司组织架构
        /// </summary>
        public OrganizationBll OrganizationBLL
        {
            get
            {
                if (_OrganizationBll == null)
                {
                    _OrganizationBll = new OrganizationBll(this);
                }
                return _OrganizationBll;
            }
        }

        private PurviewInfoBll _PurviewInfoBll;
        /// <summary>
        /// 权限信息
        /// </summary>
        public PurviewInfoBll PurviewInfoBLL
        {
            get
            {
                if (_PurviewInfoBll == null)
                {
                    _PurviewInfoBll = new PurviewInfoBll(this);
                }
                return _PurviewInfoBll;
            }
        }

        private RoleInfoBll _RoleInfoBll;
        /// <summary>
        /// 用户角色信息
        /// </summary>
        public RoleInfoBll RoleInfoBLL
        {
            get
            {
                if (_RoleInfoBll == null)
                {
                    _RoleInfoBll = new RoleInfoBll(this);
                }
                return _RoleInfoBll;
            }
        }

        private RolePurviewBll _RolePurviewBll;
        /// <summary>
        /// 角色权限信息
        /// </summary>
        public RolePurviewBll RolePurviewBLL
        {
            get
            {
                if (_RolePurviewBll == null)
                {
                    _RolePurviewBll = new RolePurviewBll(this);
                }
                return _RolePurviewBll;
            }
        }

        private SysLogBll _SysLogBll;
        /// <summary>
        /// 系统日志
        /// </summary>
        public SysLogBll SysLogBLL
        {
            get
            {
                if (_SysLogBll == null)
                {
                    _SysLogBll = new SysLogBll(this);
                }
                return _SysLogBll;
            }
        }

        private SysLoginLogBll _SysLoginLogBll;
        /// <summary>
        /// 登录日志
        /// </summary>
        public SysLoginLogBll SysLoginLogBLL
        {
            get
            {
                if (_SysLoginLogBll == null)
                {
                    _SysLoginLogBll = new SysLoginLogBll(this);
                }
                return _SysLoginLogBll;
            }
        }


        private SystemExceptionLogBll _SystemExceptionLogBll;
        /// <summary>
        /// 系统异常日志
        /// </summary>
        public SystemExceptionLogBll SystemExceptionLogBLL
        {
            get
            {
                if (_SystemExceptionLogBll == null)
                {
                    _SystemExceptionLogBll = new SystemExceptionLogBll(this);
                }
                return _SystemExceptionLogBll;
            }
        }

        private SystemInfoBll _SystemInfoBll;
        /// <summary>
        /// 系统信息
        /// </summary>
        public SystemInfoBll SystemInfoBLL
        {
            get
            {
                if (_SystemInfoBll == null)
                {
                    _SystemInfoBll = new SystemInfoBll(this);
                }
                return _SystemInfoBll;
            }
        }

        private UserInfoBll _UserInfoBll;
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoBll UserInfoBLL
        {
            get
            {
                if (_UserInfoBll == null)
                {
                    _UserInfoBll = new UserInfoBll(this);
                }
                return _UserInfoBll;
            }
        }

        private UserPurviewBll _UserPurviewBll;
        /// <summary>
        /// 用户权限信息
        /// </summary>
        public UserPurviewBll UserPurviewBLL
        {
            get
            {
                if (_UserPurviewBll == null)
                {
                    _UserPurviewBll = new UserPurviewBll(this);
                }
                return _UserPurviewBll;
            }
        }


        private UserRoleBll _UserRoleBll;
        /// <summary>
        /// 用户权限信息
        /// </summary>
        public UserRoleBll UserRoleBLL
        {
            get
            {
                if (_UserRoleBll == null)
                {
                    _UserRoleBll = new UserRoleBll(this);
                }
                return _UserRoleBll;
            }
        }
        #endregion

        #region Dispose 成员
        public void Dispose()
        {
            this._DataDictionaryBll = null;
            this._EmployeeBll = null;
            this._IPBlacklistBll = null;
            this._MenuInfoBll = null;
            this._OrganizationBll = null;
            this._PurviewInfoBll = null;
            this._RoleInfoBll = null;
            this._RolePurviewBll = null;
            this._SysLogBll = null;
            this._SysLoginLogBll = null;
            this._SystemExceptionLogBll = null;
            this._SystemInfoBll = null;
            this._UserInfoBll = null;
            this._UserPurviewBll = null;
            this._UserRoleBll = null;
        }
        #endregion
    }
}
