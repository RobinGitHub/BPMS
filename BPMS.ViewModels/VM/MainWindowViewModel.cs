using BPMS.Common;
using BPMS.Foundatoin;
using BPMS.Model;
using BPMS.References;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;

namespace BPMS.ViewModels
{
    /// <summary>
    /// 首页
    /// </summary>
    public class MainWindowViewModel : ObservableObject
    {
        ///获取第一级菜单
        ///获取子集菜单
        ///
        #region 属性
        /// <summary>
        /// 选择菜单变更
        /// </summary>
        ICommand _selectedIndexChangedCommand;
        /// <summary>
        /// 当前选中的菜单
        /// </summary>
        MenuInfo _selectedMenu = null;

        string _menuTitle = "子功能项-";
        /// <summary>
        /// 当前的选中的Title
        /// </summary>
        public string MenuTitle
        {
            get
            {
                return this._menuTitle;
            }
            set
            {
                if (!this._menuTitle.Equals(value))
                {
                    this._menuTitle = value;
                    base.RaisePropertyChanged("MenuTitle");
                }
            }
        }

        #region 当前选中的菜单
        /// <summary>
        /// 当前选中的菜单
        /// </summary>
        public MenuInfo SelectedMenu
        {
            get
            {
                if (_selectedMenu == null && MenuList.Count > 0)
                { 
                    _selectedMenu = MenuList[0];
                    MenuTitle = _menuTitle + _selectedMenu.Name;
                }
                return this._selectedMenu;
            }
            set
            {
                if (!this._selectedMenu.Equals(value))
                {
                    this._selectedMenu = value;
                    base.RaisePropertyChanged("SelectedMenu");
                }
            }
        }
        #endregion

        #region 获取第一级菜单
        //用户有权限的菜单
        public List<MenuInfo> MenuList
        {
            get
            {
                List<MenuInfo> list = new List<MenuInfo>();
                for (int i = 1; i < 3; i++)
                {
                    list.Add(new MenuInfo() { ID = i, Name = "权限应用", IconUrl = "Images/32/424.png" });
                    list.Add(new MenuInfo() { ID = i, Name = "系统应用", IconUrl = "Images/32/424.png" });
                    list.Add(new MenuInfo() { ID = i, Name = "智能开发", IconUrl = "Images/32/424.png" });
                }
                return list;
                //return GetMenuList(0);
            }
        }
        #endregion

        #region 获取菜单
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenuList(int parentId)
        {
            List<MenuInfo> list = new List<MenuInfo>();
            DataTable menuDt = RefProvider.BPMSServiceRefInstance.LogGetUserMenuList();
            foreach (DataRow item in menuDt.Rows)
            {
                if (int.Parse(item["ParentId"].ToString()) == parentId)
                {
                    MenuInfo menuInfo = new MenuInfo();
                    menuInfo.ID = int.Parse(item["ID"].ToString());
                    menuInfo.Name = item["Name"].ToString();
                    menuInfo.IconUrl = item["IconUrl"].ToString();
                    list.Add(menuInfo);
                }
            }
            return list;
        }
        #endregion

        #endregion

        #region 事件
        //ViewModel里面的事件，一般来说都是通知View做相应的UI的操作
        public event GetResultEventHandle GetResult;

        #endregion

        #region 命令
        public ICommand SelectedIndexChangedCommand
        {
            get
            {
                if (this._selectedIndexChangedCommand == null)
                {
                    this._selectedIndexChangedCommand = new RelayCommand<object>(
                        (t) => this.ExecSelectedIndexChanged(t),
                        (t) => true);
                }

                return this._selectedIndexChangedCommand;
            }
        }
        #endregion

        #region 对View可见的公共方法
        #endregion

        #region 私有方法
        #region 选择菜单
        void ExecSelectedIndexChanged(object parameter)
        {
            var item = parameter as MenuInfo;
            if (item.ID == 0)
            {
                //SelectRoleCanLogin = false;
            }
            else
            {
                MenuTitle = "子功能项-" + item.Name;
                //SelectRoleCanLogin = true;
            }
        }
        #endregion


        #endregion

    }
}
