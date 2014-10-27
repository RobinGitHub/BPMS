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
    /// TitleControl.xaml 的交互逻辑
    /// </summary>
    public partial class TitleControl : UserControl
    {
        /// <summary>
        /// 设置自定义的属性依赖
        /// 注：这里一定要设置 PropertyMetadata
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TitleControl), new PropertyMetadata(new PropertyChangedCallback(valuePropertyChangedCallback)));


        public TitleControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 显示内容
        /// </summary>
        [Description("显示内容"), Category("ExProperty")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        static void valuePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TitleControl ptb = (sender as TitleControl);
            if (ptb != null)
                ptb.txtTitle.Text = e.NewValue.ToString();
        } 
        
    }
}
