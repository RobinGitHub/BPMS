using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using BPMS.Model;
using BPMS.ViewModels;
using System.Reflection;


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
            this.Closing += MainWindow_Closing;
            btnHome.Click += btnHome_Click;
            btnSettings.Click += btnSettings_Click;
            btnExit.Click += btnExit_Click;
        }



        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            btnHome.Content = new BindButton() { Content = "首页", ImageSource = "Images/32/n1.png" };
            btnSettings.Content = new BindButton() { Content = "设置", ImageSource = "Images/32/n3.png" };
            btnExit.Content = new BindButton() { Content = "退出", ImageSource = "Images/32/n4.png" };
            //首页
            dpHome.Caption = new MenuInfo() { Name = "首页", IconUrl = "Images/32/4963_home.png" };
        }
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult rlt = MessageDxUtil.ShowYesNoAndTips("确定退出么？");
            e.Cancel = rlt != System.Windows.Forms.DialogResult.Yes;
        }

        #region 顶部按钮
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            AddTabs(new MenuInfo() { ID = -1, Name = "设置", IconUrl = "Images/32/4963_home.png", FormName = "Setting" });
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnHome_Click(object sender, RoutedEventArgs e)
        {
            docGroup.SelectedTabIndex = 0;
        }
        #endregion

        void vm_GetResult(Model.Result rlt)
        {

        }

        /// <summary>
        /// 添加选项卡
        /// </summary>
        private void AddTabs(MenuInfo menuInfo)
        {
            foreach (DocumentPanel item in docGroup.Items)
            {
                MenuInfo tmpMenuInfo = item.Caption as MenuInfo;
                if (tmpMenuInfo.ID == menuInfo.ID)
                {
                    docGroup.SelectedTabIndex = item.TabIndex;
                    return;
                }
            }

            DocumentPanel docPanel = new DocumentPanel();
            docPanel.Style = (Style)this.FindResource("homeDocumentPanel");
            System.Windows.Controls.UserControl control = (System.Windows.Controls.UserControl)Assembly.Load("BPMS.Views.Default").CreateInstance("BPMS.Views.Default." + menuInfo.FormName);  
            docPanel.Content = control;
            docPanel.TabIndex = docGroup.Items.Count;
            docPanel.Caption = menuInfo;
            docGroup.Items.Add(docPanel);
            docGroup.SelectedTabIndex = docGroup.Items.Count -1;
        }
    }


}
