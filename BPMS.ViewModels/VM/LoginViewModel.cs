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
    public class LoginViewModel : ObservableObject
    {
        #region 属性
        /// <summary>
        /// 登录命令
        /// </summary>
        ICommand _loginCommand;
        /// <summary>
        /// 选择登录角色变更
        /// </summary>
        ICommand _selectedIndexChangedCommand;

        bool _canLogin = true;
        /// <summary>
        /// 登录选择角色 是否允许登录
        /// </summary>
        bool _selectRoleCanLogin = false;
        string _account = "admin";
        string _password = "admin";
        /// <summary>
        /// 当前选中的角色
        /// </summary>
        DropdownItem _selectedRole = DropdownItem.GetPlease; 

        public string Account { get { return _account; } set { _account = value; } }
        public string Password { get { return _password; } set { _password = value; } }
        public string IP { get; set; }
        public int ServicePort { get; set; }
        public int UpdatePort { get; set; }
        public EBinding Binding { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<DropdownItem> RoleList
        {
            get
            {
                List<DropdownItem> list = new List<DropdownItem>();
                list.Add(DropdownItem.GetPlease);
                for (int i = 0; i < 3; i++)
                {
                    DropdownItem item = new DropdownItem();
                    item.ID = i + 1;
                    item.Text = "用户" + i;
                    list.Add(item);
                }
                foreach (RoleInfo item in User.Current.RoleList)
                {
                    DropdownItem di = new DropdownItem();
                    di.ID = item.ID;
                    di.Text = item.Name;
                    list.Add(di);
                }
                return list;
            }
            set
            {
                RoleList = value;
                base.RaisePropertyChanged("RoleList"); 
            }
        }

        public DropdownItem SelectedRole
        {
            get
            {
                return this._selectedRole;
            }
            set
            {
                if (!this._selectedRole.Equals(value))
                {
                    this._selectedRole = value;
                    base.RaisePropertyChanged("SelectedRole");
                }
            }
        }

        public bool CanLogin
        {
            get
            {
                return _canLogin;
            }
            set
            {
                _canLogin = value;
                base.RaisePropertyChanged("CanLogin");
            }
        }
        /// <summary>
        /// 选择角色登录 是否允许登录
        /// </summary>
        public bool SelectRoleCanLogin
        {
            get
            {
                return _selectRoleCanLogin;
            }
            set
            {
                _selectRoleCanLogin = value;
                base.RaisePropertyChanged("SelectRoleCanLogin");
            }
        }
        #endregion

        #region 事件
        //ViewModel里面的事件，一般来说都是通知View做相应的UI的操作
        public event GetResultEventHandle GetResult;

        #endregion

        #region 命令
        public ICommand LoginCommand
        {
            get
            {
                if (this._loginCommand == null)
                {
                    this._loginCommand = new RelayCommand(
                        () => this.ExecLogin(),
                        () => this.CanLogin);
                }

                return this._loginCommand;
            }
        }

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
        #region 登录
        void ExecLogin()
        {
            if (_selectRoleCanLogin)
            {
                RoleInfo role = User.Current.RoleList.First(t => t.ID == _selectedRole.ID);
                User.Current.RoleId = role.ID;
                User.Current.RoleCode = role.Code;
                User.Current.RoleName = role.Name;
                //选择角色后登录
                User.Current.PurviewDt = RefProvider.BPMSServiceRefInstance.PubGetUserPurviewList(User.Current.SystemId, User.Current.ID, User.Current.ID);
            }
            else
            {
                //直接登录
                #region
                BPMSServiceRef.Binding = EBinding.WsHttpBinding;
                BPMSServiceRef.IP = IP;
                BPMSServiceRef.Port = ServicePort;

                RefProvider.Init();
                int systemId = 0;
                int userId = 0;
                string userName = "";
                List<RoleInfo> roleList = null;
                /// 0登录失败
                /// 1登录成功
                /// 2账号不存在
                /// 3用户已禁用
                /// 4密码不正确
                /// 5当前IP不许登录
                /// 6当前用户没有任何权限
                Result rltModel = new Result();
                int result = 0;
                CanLogin = false;
                Action action = new Action(() =>
                {
                    try
                    {
                        result = RefProvider.BPMSServiceRefInstance.Login(Consts.ConstValue.SystemCode, Account, Password, out systemId, out userId, out userName, out roleList);
                        if (result == 1)
                        {
                            User.Current.Credentials = new ClientCredentials() { Account = this.Account, Password = this.Password };
                            User.Current.ID = userId;
                            User.Current.Name = userName;
                            User.Current.SystemId = systemId;
                            User.Current.RoleList = roleList;
                        }
                    }
                    catch (Exception ex)
                    {
                        rltModel.Message = ex.Message;
                    }
                });
                action.BeginInvoke(new AsyncCallback((rlt) =>
                {
                    if (rlt.IsCompleted)
                    {
                        CanLogin = true;
                        if (string.IsNullOrEmpty(rltModel.Message))
                            rltModel.Message = Consts.ReturnCodeMessage.GetMessage(result);
                        if (result == 1)
                            rltModel.IsSuccess = true;
                        else
                            rltModel.IsSuccess = false;
                        if (GetResult != null)
                            GetResult(rltModel);
                    }
                }), null);
                #endregion
            }
        }
        #endregion

        #region 选择登录角色
        void ExecSelectedIndexChanged(object parameter)
        {
            DropdownItem item = parameter as DropdownItem;
            if (item.ID == 0)
            {
                SelectRoleCanLogin = false;
            }
            else
            {
                SelectRoleCanLogin = true;
            }
        }
        #endregion
        #endregion

    }
}
