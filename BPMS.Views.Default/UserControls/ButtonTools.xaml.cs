using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BPMS.Views.Default
{
    /// <summary>
    /// ButtonTools.xaml 的交互逻辑
    /// </summary>
    public partial class ButtonTools : UserControl
    {
        #region 构造函数
        public ButtonTools()
        {
            InitializeComponent();

            this.btnSure.Click += btnSure_Click;
            this.btnClose.Click += btnClose_Click;

            btnSure.Visibility = System.Windows.Visibility.Visible;
            btnClose.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

        #region 显示隐藏

        [Description("确认按钮是否可见"), DefaultValue(true), Browsable(true), Category("Button工具栏")]
        public Visibility BtnSureVisibility { get { return btnSure.Visibility; } set { btnSure.Visibility = value; } }

        [Description("关闭按钮是否可见"), DefaultValue(true), Browsable(true), Category("Button工具栏")]
        public Visibility BtnCloseVisibility { get { return btnClose.Visibility; } set { btnClose.Visibility = value; } }
        #endregion

        #region Events
        [Description("确认"), DefaultValue(true), Browsable(true), Category("Button工具栏")]
        public event RoutedEventHandler BtnSure;

        [Description("关闭"), DefaultValue(true), Browsable(true), Category("Button工具栏")]
        public event RoutedEventHandler BtnClose;
        #endregion

        #region Click Events
        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (BtnClose != null)
                BtnClose(sender, e);
        }

        void btnSure_Click(object sender, RoutedEventArgs e)
        {
            if (BtnSure != null)
                BtnSure(sender, e);
        }
        #endregion


    }
}
