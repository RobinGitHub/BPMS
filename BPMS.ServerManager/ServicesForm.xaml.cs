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
using BPMS.Common;


namespace BPMS.ServerManager
{
    /// <summary>
    /// Interaction logic for ServicesForm.xaml
    /// </summary>
    public partial class ServicesForm : DXWindow
    {
        public ServicesForm()
        {
            InitializeComponent();
            Config.Current.LoadXml();
            this.Loaded += ServicesForm_Loaded;
            this.btnInstall.Click += btnInstall_Click;
            this.btnUnInstall.Click += btnUnInstall_Click;
            this.btnSave.Click += btnSave_Click;
            this.btnClose.Click += btnClose_Click;
        }

        void ServicesForm_Loaded(object sender, RoutedEventArgs e)
        {
            cmbBindType.Items.Add(EBinding.WsHttpBinding.ToString());
            cmbBindType.Items.Add(EBinding.NetTcpBinding.ToString());

            for (int i = 0; i < cmbBindType.Items.Count; i++)
            {
                var item = cmbBindType.Items[i];
                if (item.ToString().ToUpper() == Config.Current.Binding.ToString().ToUpper())
                {
                    cmbBindType.SelectedIndex = i;
                    break;
                }
            }

            txtIP.Text = Config.Current.IP;
            txtServicePort.Text = Config.Current.Port.ToString();
            txtUpdPort.Text = Config.Current.UpdatePort.ToString();
        }

        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            EBinding binding = (EBinding)Enum.Parse(typeof(EBinding), cmbBindType.SelectedItem.ToString());
            string IP = txtIP.Text.Trim();
            int serverPort = 0;
            int updatePort = 0;
            try
            {
                serverPort = Convert.ToInt32(txtServicePort.Text.Trim());
                updatePort = Convert.ToInt32(txtUpdPort.Text.Trim());
            }
            catch
            {
            }

            if (string.IsNullOrEmpty(IP))
            {
                lblTips.Content = "请输入IP地址";
                txtIP.Focus();
                return;
            }
            if (serverPort == 0)
            {
                lblTips.Content = "请输入服务连接端口";
                txtServicePort.Focus();
                return;
            }
            if (updatePort == 0)
            {
                lblTips.Content = "请输入服务更新端口";
                txtUpdPort.Focus();
                return;
            }
            Config.Current.Binding = binding;
            Config.Current.IP = IP;
            Config.Current.Port = serverPort;
            Config.Current.UpdatePort = updatePort;
            Config.Current.Save();
            lblTips.Content = "保存成功!";
        }

        void btnUnInstall_Click(object sender, RoutedEventArgs e)
        {
            lblTips.Content = "";
            try
            {
                ServicesHelper.Current.UnInstallService();
                lblTips.Content = "卸载服务完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CheckButtonAccess();
        }

        void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            lblTips.Content = "";
            try
            {
                ServicesHelper.Current.InstallService();
                lblTips.Content = "安装服务完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CheckButtonAccess();
        }

        private void CheckButtonAccess()
        {
            btnUnInstall.IsEnabled = false;
            btnInstall.IsEnabled = false;
            if (ServicesHelper.Current.ServiceIsExisted())
                btnUnInstall.IsEnabled = true;
            else
                btnInstall.IsEnabled = true;
        }
    }
}
