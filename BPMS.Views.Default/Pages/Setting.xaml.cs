using BPMS.ViewModels;
using BPMS.Model;
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
using System.Reflection;

namespace BPMS.Views.Default
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : UserControl
    {
        SettingViewModel vm = null;
        public Setting()
        {
            InitializeComponent();
            vm = new SettingViewModel();
            this.DataContext = vm;
            this.lstboxLeftMenu.SelectionChanged += lstboxLeftMenu_SelectionChanged;
        }

        void lstboxLeftMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MenuInfo menuInfo = e.AddedItems[0] as MenuInfo;
            System.Windows.Controls.UserControl control = (System.Windows.Controls.UserControl)Assembly.Load("BPMS.Views.Default").CreateInstance("BPMS.Views.Default." + menuInfo.FormName);
            ccContent.Content = control;
        }
    }
}
