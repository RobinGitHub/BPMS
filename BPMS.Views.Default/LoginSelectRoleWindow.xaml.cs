using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using BPMS.ViewModels;


namespace BPMS.Views.Default
{
    /// <summary>
    /// Interaction logic for LoginSelectRoleWindow.xaml
    /// </summary>
    public partial class LoginSelectRoleWindow : DXWindow
    {
        LoginViewModel vm = null;
        public LoginSelectRoleWindow()
        {
            InitializeComponent();
            vm = new LoginViewModel();
            this.DataContext = vm;
            vm.GetResult += vm_GetResult;
        }

        void vm_GetResult(Model.Result rlt)
        {
            
        }
    }
}
