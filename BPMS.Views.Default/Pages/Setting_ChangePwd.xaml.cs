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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BPMS.Model;
using BPMS.ViewModels;
using DevExpress.Xpf.Docking;

namespace BPMS.Views.Default
{
    /// <summary>
    /// Setting_ChangePwd.xaml 的交互逻辑
    /// </summary>
    public partial class Setting_ChangePwd : UserControl
    {
        SettingViewModel vm = null;
        public Setting_ChangePwd()
        {
            InitializeComponent();
            vm = new SettingViewModel();
            this.DataContext = vm;
            this.buttonTools.BtnSure += buttonTools_BtnSure;
            this.buttonTools.BtnClose += buttonTools_BtnClose;
        }
        
        void buttonTools_BtnSure(object sender, RoutedEventArgs e)
        {
            tipControl.ShowTip(buttonTools, "公司名称不能为空！", this, TipOrientation.right, 150, 30);
        }

        void buttonTools_BtnClose(object sender, RoutedEventArgs e)
        {
            DocumentPanel dp = (DocumentPanel)(((Setting)((Grid)((ContentControl)this.Parent).Parent).Parent).Parent);
            DocumentGroup dg = (DocumentGroup)dp.Parent;
            dg.Remove(dp);
        }
    }
}
