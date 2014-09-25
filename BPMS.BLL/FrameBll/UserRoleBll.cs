using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMS.DAL;
using BPMS.Model;
using BPMS.Common;
using System.Data;

namespace BPMS.BLL 
{ 
    /// <summary> 
    /// 
    /// </summary>  
    public partial class UserRoleBll : BaseTableBLL<UserRole, UserRoleDal>
    { 
        #region 构造函数
        public UserRoleBll() { }

        public UserRoleBll(BllProvider provider)
            : base(provider)
        { }
        #endregion

        #region 当前选中的系统下 的所有角色
        /// <summary>
        /// 当前选中的系统下 的所有角色
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public DataTable GetList(int logUserId, string logUserName, int systemId, int userId)
        {
            try
            {
                #region 系统日志
                SysLog sysLogModel = new SysLog();
                sysLogModel.TableName = "UserRole";
                sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                sysLogModel.CreateUserId = logUserId;
                sysLogModel.CreateUserName = logUserName;
                sysLogModel.OperationType = EOperationType.访问.GetHashCode();
                this.BLLProvider.SysLogBLL.Add(null, sysLogModel, null);
                #endregion
                return dal.GetList(systemId, userId);
            }
            catch (Exception ex)
            {
                this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
            }
            return null;
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal int Add(List<UserRole> list)
        {
            int rlt = 1;
            if (!dal.Insert(list))
                rlt = 0;
            return rlt;
        }
        #endregion

        #region 设置用户角色
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <returns>
        /// 0操作失败，请联系管理员
        /// 1操作成功
        /// </returns>
        public int UserRoleSet(int logUserId, string logUserName, int systemId, int userId, List<int> lstRoleId)
        {
            int rlt = 1;
            using (var ctx = TranContext.BeginTran())
            {
                try
                {
                    #region 系统日志
                    SysLog sysLogModel = new SysLog();
                    sysLogModel.TableName = "DataDictionary";
                    sysLogModel.BusinessName = DatabasePDMHelper.GetDataTableName(sysLogModel.TableName);
                    sysLogModel.CreateUserId = logUserId;
                    sysLogModel.CreateUserName = logUserName;
                    sysLogModel.OperationType = EOperationType.修改.GetHashCode();

                    if (rlt == 1 && !this.BLLProvider.SysLogBLL.Add(ctx, sysLogModel, null))
                        rlt = 0;
                    #endregion

                    dal.Delete(ctx, systemId, userId);
                    List<UserRole> modeList = new List<UserRole>();
                    foreach (var item in lstRoleId)
                    {
                        UserRole model = new UserRole();
                        model.ID = GetNewID();
                        model.CreateDate = DateTime.Now;
                        model.CreateUserId = logUserId;
                        model.CreateUserName = logUserName;
                        model.RoleId = item;
                        model.UserId = userId;
                        modeList.Add(model);
                    }
                    if (rlt == 1 && !dal.Insert(ctx, modeList))
                        rlt = 0;
                    if (rlt == 1)
                        TranContext.Commit(ctx);
                    else
                        TranContext.Rollback(ctx);
                }
                catch (Exception ex)
                {
                    rlt = 0;
                    TranContext.Rollback(ctx);
                    this.BLLProvider.SystemExceptionLogBLL.Add(ex.Source, ex.InnerException.Message, ex.Message);
                }
            }
            return rlt;
        }
        #endregion

    }
} 
