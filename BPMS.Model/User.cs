using BPMS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BPMS.Model
{
    /// <summary>
    /// 用户对象
    /// </summary>
    public class User
    {
        private static User _user = null;
        public static User Current
        {
            get
            {
                if (_user == null)
                    _user = new User();
                return _user;
            }
        }

        /// <summary>
        /// 登录的凭证(帐号与密码)
        /// </summary>
        public ClientCredentials Credentials
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID
        {
            get;
            set;
        }
        /// <summary>
        /// 系统ID
        /// </summary>
        public int SystemId { get; set; }
        /// <summary>
        /// 系统Code
        /// </summary>
        public string SystemCode { get { return Consts.ConstValue.SystemCode; } }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有多个角色
        /// </summary>
        public bool IsMultiRole
        {
            get
            {
                return RoleList != null && RoleList.Count > 1;
            }
        }
        public List<RoleInfo> RoleList { get; set; }
        /// <summary>
        /// 当前用户指定角色RoleCode的所有权限
        /// </summary>
        public DataTable PurviewDt
        {
            get;
            set;
        }

        public bool IsAccess(EModules moduleCode, EFunctions funCode, EActions actionCode)
        {
            bool isAccess = true;
            int modulePurviewID = 0;
            int functionPurviewID = 0;

            if (moduleCode != EModules.UnKnow)
            {
                DataRow[] arrRow = PurviewDt.Select(string.Format("code='{0}' AND PurviewType='{1}'",
                    moduleCode.ToString(), EPurviewType.模块.GetHashCode().ToString()));
                if (arrRow.Count() == 0)
                    isAccess = false; //没有权限
                else
                    modulePurviewID = ConvertHelper.ObjectToInt(arrRow[0]["PurviewID"]);
            }
            if (isAccess && funCode != EFunctions.UnKnow)
            {
                DataRow[] arrRow = PurviewDt.Select(string.Format("code='{0}' AND PurviewType='{1}' and parentID={2}",
                    funCode.ToString(), EPurviewType.功能.GetHashCode().ToString(), modulePurviewID));

                if (arrRow.Count() == 0)
                    isAccess = false; //没有权限
                else
                    functionPurviewID = ConvertHelper.ObjectToInt(arrRow[0]["PurviewID"]);
            }
            if (isAccess && actionCode != EActions.UnKnow)
            {
                if (actionCode != EActions.UnKnow &&
                    PurviewDt.Select(string.Format("code='{0}' AND PurviewType='{1}' and parentID={2}",
                    actionCode.ToString(), EPurviewType.操作.GetHashCode().ToString(), functionPurviewID)).Count() == 0)
                    isAccess = false; //没有权限
            }
            return isAccess;
        }

    }
}
