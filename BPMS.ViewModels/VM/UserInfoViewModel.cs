using BPMS.Common;
using BPMS.Foundatoin;
using BPMS.Model;
using BPMS.References;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BPMS.ViewModels
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserInfoViewModel : ObservableObject
    {
        #region 私有字段

        readonly UserInfo _userInfo;

        #region 命令
        /// <summary>
        /// 登录命令
        /// </summary>
        ICommand _loginCommand;

        #endregion

        #endregion

        #region 构造函数
        public UserInfoViewModel()
        {
            this._userInfo = new UserInfo();
        }

        public UserInfoViewModel(UserInfo userInfo)
        {
            this._userInfo = userInfo;
        }
        #endregion

        #region 属性
        public int ID { get; set; }
        public string Code { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Spell { get; set; }
        public string Alias { get; set; }
        public int RoleId { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Email { get; set; }
        public string OICQ { get; set; }
        public int DutyId { get; set; }
        public Nullable<int> TitleId { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public int WorkgroupId { get; set; }
        public Nullable<System.DateTime> ChangePasswordDate { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public Nullable<int> LogOnCount { get; set; }
        public Nullable<System.DateTime> FirstVisit { get; set; }
        public Nullable<System.DateTime> PreviousVisit { get; set; }
        public Nullable<System.DateTime> LastVisit { get; set; }
        public string Remark { get; set; }
        public bool IsEnable { get; set; }
        public int SortIndex { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public System.DateTime ModifyDate { get; set; }
        public int ModifyUserId { get; set; }
        public string ModifyUserName { get; set; }
        #endregion

        #region 事件
        //ViewModel里面的事件，一般来说都是通知View做相应的UI的操作
        public event EventHandler Closed;

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
        #endregion

        #region 对View可见的公共方法

        public void Close()
        {
            //关闭之前的逻辑操作

            this.RaiseClosed();
        }

        #endregion

        #region 私有方法
        void ExecLogin()
        {
            BPMSServiceRef.Binding = EBinding.WsHttpBinding;
            BPMSServiceRef.IP = "localhost";
            BPMSServiceRef.Port = 8081;
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
            try
            {
                int rlt = RefProvider.BPMSServiceRefInstance.Login(Consts.ConstValue.SystemCode, Account, Password, out systemId, out userId, out userName, out roleList);
                if (rlt == 0)
                {
                    
                }
            }
            catch (Exception)
            {

                throw;
            }


            this.RaiseClosed();
        }

        bool CanLogin { get { return true; } }

        //void Save()
        //{
        //    //新建或者更新操作，此处略去100行代码

        //    this.RaiseClosed();
        //}
        //bool CanSave
        //{
        //    get { return this._customer.IsValid; }
        //}

        //void Delete()
        //{
        //    //删除操作，此处略去100行代码

        //    this.RaiseClosed();
        //}
        //bool CanDelete { get { return true; } }

        void RaiseClosed()
        {
            var handler = this.Closed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion
    }
}
