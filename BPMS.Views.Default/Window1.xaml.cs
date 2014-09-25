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
using DevExpress.Xpf.Core;
using System.Threading;

namespace BPMS.Views.Default
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Window1 : DXWindow
    {
        public Window1()
        {
            InitializeComponent();
            
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //maskControl.Inf_text = "系统提交，请稍候...";
            maskControl.Show();
        }
    }
}
