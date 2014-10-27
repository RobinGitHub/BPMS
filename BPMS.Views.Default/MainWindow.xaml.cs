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
using DevExpress.Xpf.Layout.Core;
using DevExpress.Xpf.Docking;
using BPMS.Model;
using BPMS.ViewModels;


namespace BPMS.Views.Default
{
    public partial class MainWindow : DXWindow
    {
        MainWindowViewModel vm = null;

        public MainWindow()
        {
            InitializeComponent();

            vm = new MainWindowViewModel();
            this.DataContext = vm;
            vm.GetResult += vm_GetResult;
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            btnHome.Content = new BindButton() { Content = "首页", ImageSource = "Images/32/n1.png" };
            btnSettings.Content = new BindButton() { Content = "设置", ImageSource = "Images/32/n3.png" };
            btnExit.Content = new BindButton() { Content = "退出", ImageSource = "Images/32/n4.png" };
            //首页
            dpHome.Caption = new MenuInfo() { Name = "首页", IconUrl = "Images/32/4963_home.png" };
        }

        void vm_GetResult(Model.Result rlt)
        {

        }
    }


}
