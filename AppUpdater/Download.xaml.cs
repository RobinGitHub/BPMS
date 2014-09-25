using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using AppUpdaterContracts;
using System.Windows;
using DevExpress.Xpf.Core;
using System.Data;
using System.Diagnostics;


namespace AppUpdater
{
    /// <summary>
    /// Interaction logic for Download.xaml
    /// </summary>
    public partial class Download : DXWindow
    {
        #region 私有变量
        List<UpFileInfo> UpdateUpFileList = null;
        List<Thread> AllThreadList = new List<Thread>(); //所有线程
        int DownloadingThreadNum = 0; //当前活动线程数
        int DownloadedFileNum = 0;//下载完成的数量
        EFileStatus UpdaterStatus = EFileStatus.None;//当前更新状态
        #endregion

        public Download()
        {
            InitializeComponent();
            this.Loaded += Download_Loaded;
            this.btnDownload.Click += btnDownload_Click;
            this.Closing += Download_Closing;
        }

        #region Loaded
        void Download_Loaded(object sender, RoutedEventArgs e)
        {
            btnDownload.IsEnabled = false;
            UpdaterStatus = EFileStatus.ConnServer;

            this.Title = "正在下载列表文件...";
            UpdaterStatus = EFileStatus.DownloadFileList;
            Thread objThread = new Thread(new ThreadStart(DownloadFileList));
            AllThreadList.Add(objThread);
            objThread.Start();
        }
        #endregion

        #region Closing
        void Download_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //将临时文件更新替换中,禁止关闭
            if (UpdaterStatus == EFileStatus.ReplaceUpdateTargetFile)
            {
                e.Cancel = true;
                return;
            }

            if (UpdaterStatus != EFileStatus.None
                && UpdaterStatus != EFileStatus.ConnServer
                && UpdaterStatus != EFileStatus.WaitDownLoadFile
                && UpdaterStatus != EFileStatus.UpdateComplete
                && UpdaterStatus != EFileStatus.Error)
            {
                if (MessageBox.Show("若中断更新,需重新下载?", "警告", MessageBoxButton.YesNo , MessageBoxImage.Warning) != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
            //将线程终止
            foreach (Thread obj in AllThreadList)
            {
                try
                {
                    obj.Abort();
                }
                catch { }
            }
            Application.Current.Shutdown();
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateUpFileList == null || UpdateUpFileList.Count == 0)
                return;
            if (btnDownload.Content.ToString() == "启动程序")
            {
                Complete();
                return;
            }
            //标记开始下载状态
            UpdaterStatus = EFileStatus.DownloadFile;
            btnDownload.IsEnabled = false;
            DownloadingThreadNum = 0;//活动线程数
            DownloadedFileNum = 0; //下载完成数
            this.Title = String.Format("开始下载...   已完成[{0}/{1}]   活动线程数[{2}]",
                DownloadedFileNum, UpdateUpFileList.Count, DownloadingThreadNum);

            Thread objDownloadThread = new Thread(new ThreadStart(() => 
            {
                for (int i = 0; i < UpdateUpFileList.Count; i++)
                {
                    //限制线程数量
                    while (DownloadingThreadNum >= ConfigSet.Current.ThreadNum)
                    {
                        Thread.Sleep(200);//200毫秒，有空闲线程就开启下载
                    }
                    //正在更新的线程数
                    DownloadingThreadNum++;
                    this.Dispatcher.Invoke(new Action(() => 
                    {
                        this.Title = String.Format("开始下载...   已完成[{0}/{1}]   活动线程数[{2}]",
                            DownloadedFileNum, UpdateUpFileList.Count, DownloadingThreadNum);
                    }));
                    Thread objThread = new Thread(new ParameterizedThreadStart((object arg) =>
                    {
                        DownloadFile(UpdateUpFileList[Convert.ToInt32(arg)]);
                    }));
                    AllThreadList.Add(objThread); //将线程对象缓存
                    objThread.Start(i);
                }

                while (DownloadedFileNum < UpdateUpFileList.Count)
                {
                    //System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则控件将因为循环执行太快而来不及显示信息
                    Thread.Sleep(200);//等待全部下载完
                }

                /*****************开始更新Xml配置文件*****************/
                Stream xmlStream = null;
                try
                {
                    xmlStream = ServiceProxy.Current.CreateUpdateXmlStream();
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        MessageBox.Show("编码:ServiceProxy.CreateUpdateXmlStream\r\n描述:" + ex.Message);
                        UpdaterStatus = EFileStatus.Error;
                        this.Close();
                    }));
                }

                using (var targetStream = new FileStream(ConfigSet.Current.TempFolder + ConfigSet.Current.XmlFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                { 
                    //定义文件缓冲区
                    byte[] buffer = new byte[ConfigSet.Current.BufferSize];
                    int count = 0;
                    while ((count = xmlStream.Read(buffer, 0, (int)ConfigSet.Current.BufferSize)) > 0)
                    {
                        targetStream.Write(buffer, 0, count);
                        targetStream.Flush();
                    }
                    targetStream.Close();
                    xmlStream.Close();
                }

                /*********************************至此已完全下载完成*********************************/
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.Title = "正在更新...";
                }));
                UpdaterStatus = EFileStatus.ReplaceUpdateTargetFile;

                //将文件从临时目录更新至工作区
                foreach (UpFileInfo objUpFile in UpdateUpFileList)
                {
                    CoptyFileToTarget(ConfigSet.Current.TempFolder + objUpFile.FileName, objUpFile.FileName);
                }
                CoptyFileToTarget(ConfigSet.Current.TempFolder + ConfigSet.Current.XmlFileName, ConfigSet.Current.XmlFileName);

                UpdaterStatus = EFileStatus.UpdateComplete;
                //将临时文件及文件夹删除
                Directory.Delete(ConfigSet.Current.TempFolder, true);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.Title = "更新成功";
                    btnDownload.Content = "启动程序";
                    btnDownload.IsEnabled = true;
                }));
            }));

            AllThreadList.Add(objDownloadThread);
            objDownloadThread.Start();
        }

        //首先假定fromFile一定是真实存在的
        private void CoptyFileToTarget(string fromFile, string toFile)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(toFile)));
            File.Copy(fromFile, toFile, true);
        }
        #endregion

        #region 下载列表文件
        private void DownloadFileList()
        {
            try
            {
                UpdateUpFileList = ServiceProxy.Current.GetUpFileList().OrderBy(t => t.FileName).ToList();
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show("编码:ServiceProxy.GetUpFileList\r\n描述:" + ex.Message);
                    UpdaterStatus = EFileStatus.Error;
                    this.Close();
                }));
            }

            this.Dispatcher.Invoke(new Action(() =>
            {
                DataTable source = new DataTable();
                source.Columns.Add("FileName");
                source.Columns.Add("FileSize");
                source.Columns.Add("DownloadSize");
                source.Columns.Add("Percent");
                foreach (UpFileInfo objUpFile in UpdateUpFileList)
                {
                    DataRow row = source.NewRow();
                    row[0] = objUpFile.FileName;
                    row[1] = Math.Ceiling((double)objUpFile.FileLength / 1024).ToString() + "K";
                    row[2] = "0K";
                    row[3] = "0%";
                    source.Rows.Add(row);
                }
                this.gridControl.ItemsSource = source;
                this.Title = String.Format("发现有[{0}]个文件需要更新", UpdateUpFileList.Count);
                if (UpdateUpFileList.Count > 0)
                    btnDownload.IsEnabled = true;

                UpdaterStatus = EFileStatus.WaitDownLoadFile;
            }));
        }
        #endregion

        #region 下载更新文件
        /// <summary>
        /// 下载更新文件
        /// </summary>
        /// <param name="objUpFile"></param>
        private void DownloadFile(UpFileInfo objUpFile)
        {
            Stream sourceStream = null;
            try
            {
                sourceStream = ServiceProxy.Current.CreateUpdateFileStream(objUpFile.FileName);
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show("编码:ServiceProxy.CreateUpdateFileStream\r\n描述:" + ex.Message);
                    UpdaterStatus = EFileStatus.Error;
                    this.Close();
                }));
            }

            //创建文件夹
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(ConfigSet.Current.TempFolder + objUpFile.FileName)));
            using (var targetStream = new FileStream(ConfigSet.Current.TempFolder + objUpFile.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //定义文件缓冲区
                byte[] buffer = new byte[ConfigSet.Current.BufferSize];
                int count = 0;

                long totalDownloadedByte = 0;
                while ((count = sourceStream.Read(buffer, 0, (int)ConfigSet.Current.BufferSize)) > 0)
                {
                    //总下载字节数
                    totalDownloadedByte = count + totalDownloadedByte;
                    targetStream.Write(buffer, 0, count);
                    targetStream.Flush();

                    if (ConfigSet.Current.DelayMillisecond > 0)
                    {
                        Thread.Sleep(ConfigSet.Current.DelayMillisecond);
                    }

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        long totalDownloadKB = (long)Math.Floor((double)totalDownloadedByte / 1024);
                        //百分比，小于等于最大整数
                        int percent = (int)Math.Floor((double)totalDownloadedByte / (double)objUpFile.FileLength * 100);

                        DataTable dt = gridControl.ItemsSource as DataTable;
                        foreach (DataRow objRow in dt.Rows)
                        {
                            if (objRow[0].ToString() == objUpFile.FileName)
                            {
                                objRow[2] = totalDownloadKB.ToString() + "K";
                                objRow[3] = percent.ToString() + "%";
                                break;
                            }
                        }
                    }));
                }
                targetStream.Close();
                sourceStream.Close();
            }

            //下载完成后，检查并更新
            this.Dispatcher.Invoke(new Action(() =>
            {
                DataTable dt = gridControl.ItemsSource as DataTable;
                foreach (DataRow objRow in dt.Rows)
                {
                    if (objRow[0].ToString() == objUpFile.FileName)
                    {
                        DownloadedFileNum++; //下载完成数
                        DownloadingThreadNum--; //正在更新的线程数
                        this.Title = String.Format("开始下载...   已完成[{0}/{1}]   活动线程数[{2}]",
                            DownloadedFileNum, UpdateUpFileList.Count, DownloadingThreadNum);
                        break;
                    }
                }
            }));
        }
        #endregion

        #region 完成按钮处理
        /// <summary>
        /// 完成按钮处理
        /// </summary>
        private void Complete()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = ConfigSet.Current.ExecutablePath;
            info.WorkingDirectory = Path.GetDirectoryName(info.FileName);
            Process process = new Process();
            process.StartInfo = info;
            process.Start();
            //命令行启动应用程序
            this.Close();
        }
        #endregion
    }
}
