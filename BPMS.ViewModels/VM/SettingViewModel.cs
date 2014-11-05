using BPMS.Common;
using BPMS.Foundatoin;
using BPMS.Model;
using BPMS.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;


namespace BPMS.ViewModels
{
    public class SettingViewModel
    {
        #region 属性
        #region 获取第一级菜单
        //用户有权限的菜单
        public List<MenuInfo> MenuList
        {
            get
            {
                List<MenuInfo> list = new List<MenuInfo>();
                list.Add(new MenuInfo() { ID = 1, Name = "个人资料", IconUrl = "Images/32/424.png", FormName = "Setting_Profile" });
                list.Add(new MenuInfo() { ID = 2, Name = "修改密码", IconUrl = "Images/32/424.png", FormName = "Setting_ChangePwd" });
                return list;
            }
        }
        #endregion
        #endregion

        #region 事件
        #endregion

        #region 命令

        #endregion

        #region 对View可见的公共方法
        #endregion

        #region 私有方法
        #endregion

    }
}
