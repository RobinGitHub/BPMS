using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.Model
{
    #region 权限类型
    /// <summary>
    /// 权限类型
    /// </summary>
    public enum EPurviewType
    {
        /// <summary>
        /// 模块 1
        /// </summary>
        模块 = 1,
        /// <summary>
        /// 功能 2
        /// </summary>
        功能 = 2,
        /// <summary>
        /// 操作 3
        /// </summary>
        操作 = 3,
    }
    #endregion

    #region 模块
    /// <summary>
    /// 模块
    /// </summary>
    public enum EModules
    {
        UnKnow,
        /// <summary>
        /// 权限应用
        /// </summary>
        PurviewMng,
        /// <summary>
        /// 系统应用
        /// </summary>
        SystemMng,
    }
    #endregion

    #region 功能
    /// <summary>
    /// 功能
    /// </summary>
    public enum EFunctions
    {
        UnKnow,
        #region 系统应用
        /// <summary>
        /// 字典管理
        /// </summary>
        DataDictMng,
        /// <summary>
        /// 登录日志
        /// </summary>
        LoginLogMng,
        /// <summary>
        /// 操作日志
        /// </summary>
        SysLogMng,
        /// <summary>
        /// 系统异常
        /// </summary>
        SystemExceptionMng,
        /// <summary>
        /// 系统信息
        /// </summary>
        SysInfo,
        /// <summary>
        /// 数据库管理
        /// </summary>
        DatabaseMng,
        #endregion

        #region 权限应用
        /// <summary>
        /// 系统管理
        /// </summary>
        SystemMng,
        /// <summary>
        /// 员工管理
        /// </summary>
        EmployeeMng,
        /// <summary>
        /// 用户管理
        /// </summary>
        UserMng,
        /// <summary>
        /// 角色管理
        /// </summary>
        RoleMng,
        /// <summary>
        /// 组织机构管理
        /// </summary>
        OrganMng,
        /// <summary>
        /// 用户权限管理
        /// </summary>
        UserPurviewMng,
        /// <summary>
        /// 角色权限管理
        /// </summary>
        RolePurviewMng,
        /// <summary>
        /// 缓存管理
        /// </summary>
        CacheMng,
        /// <summary>
        /// 权限管理
        /// </summary>
        PurviewMng,
        /// <summary>
        /// 菜单管理
        /// </summary>
        MenuMng,
        /// <summary>
        /// 用户访问控制
        /// </summary>
        IPBlackMng,
        #endregion

    }
    #endregion

    #region 操作
    /// <summary>
    /// 操作
    /// </summary>
    public enum EActions
    {
        UnKnow,
        /// <summary>
        /// 查看
        /// </summary>
        Vie,
        /// <summary>
        /// 添加
        /// </summary>
        Add,
        /// <summary>
        /// 删除
        /// </summary>
        Del,
        /// <summary>
        /// 修改
        /// </summary>
        Upd,
        /// <summary>
        ///导入
        /// </summary>
        Import,
        /// <summary>
        ///导出
        /// </summary>
        Export,
        /// <summary>
        ///打印
        /// </summary>
        Print,
        /// <summary>
        ///审核(有审核权限就有拒绝审核的权限)
        /// </summary>
        Check,
    }
    #endregion

    #region 字典类别
    /// <summary>
    /// 字典类别
    /// </summary>
    public enum EDictType
    {
        职称等级 = 1,
        职称 = 2,
        用工性质 = 3,
        政治面貌 = 4,
        职位 = 5,
        学位 = 6,
        学历 = 7,
        角色分类 = 8,
        国籍 = 9,
        民族 = 10,
    }
    #endregion

    #region 系统登录状态
    /// <summary>
    /// 系统登录状态
    /// </summary>
    public enum ELoginStatus
    {
        登录成功 = 1,
        登录失败 = 2,
        成功退出 = 3,
        退出异常 = 4,
    }
    #endregion

    #region 组织机构类别
    /// <summary>
    /// 组织机构类别
    /// </summary>
    public enum EOrgaCategory
    {
        集团 = 1,
        公司 = 2,
        部门 = 3,
        工作组 = 4
    }
    #endregion

    #region 是否启用
    /// <summary>
    /// 是否启用
    /// </summary>
    public enum EIsEnable
    {
        全部 = -1,
        启用 = 1,
        不启用 = 0,
    }
    #endregion

    #region 系统操作类别
    /// <summary>
    /// 系统操作类别
    /// </summary>
    public enum EOperationType
    {
        全部 = 0,
        新增 = 1,
        修改 = 2,
        删除 = 3,
        访问 = 4,
    }
    #endregion

    #region IP黑名单 类别
    /// <summary>
    /// IP黑名单 类别
    /// </summary>
    public enum EIPBlacklistCategory
    {
        登录 = 1
    } 
    #endregion

    #region 退出系统编码
    /// <summary>
    /// 退出系统编码
    /// </summary>
    public enum AppExitCode
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        OnceInstance = 1,
        /// <summary>
        /// 错误参数
        /// </summary>
        ErrorParams = 2,
        /// <summary>
        /// 服务不存在
        /// </summary>
        NothingService = 3,

        /// <summary>
        /// 不需更新
        /// </summary>
        NoUpdate = 4,

        /// <summary>
        /// 需要更新
        /// </summary>
        NeedUpdate = 5,
    }
    #endregion
}
