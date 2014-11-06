using BPMS.Model;
using BPMS.ViewModels;
using System.Reflection;
using System.Windows.Controls;

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
