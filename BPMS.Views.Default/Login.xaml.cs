using BPMS.Common;
using BPMS.Model;
using BPMS.ViewModels;
using DevExpress.Xpf.Core;
using System;
using System.Configuration;
using System.Windows;

namespace BPMS.Views.Default
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : DXWindow
    {
        LoginViewModel vm = null;
        public Login()
        {
            InitializeComponent();
            vm = new LoginViewModel();
            this.DataContext = vm;
            vm.GetResult += vm_GetResult;
            this.btnLogin.Click += btnLogin_Click;
        }

        void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            vm.IP = ConfigurationManager.AppSettings["IP"];
            vm.ServicePort = Convert.ToInt32(ConfigurationManager.AppSettings["ServicePort"]);
            vm.UpdatePort = Convert.ToInt32(ConfigurationManager.AppSettings["UpdatePort"]);
            vm.Binding = (EBinding)Enum.Parse(typeof(EBinding), ConfigurationManager.AppSettings["Binding"]);
        }

        void vm_GetResult(BPMS.Model.Result rlt)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (rlt.IsSuccess)
                {
                    if (User.Current.IsMultiRole)
                    {
                        LoginSelectRoleWindow selRole = new LoginSelectRoleWindow();
                        selRole.ShowDialog();
                    }
                    else
                    {
                        MainWindow main = new MainWindow();
                        main.Show();
                    }
                    this.Close();
                }
                else
                {
                    MessageDxUtil.ShowWarning(rlt.Message);
                }
            }));
        }
    }
}
