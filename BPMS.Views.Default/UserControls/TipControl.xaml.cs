using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace BPMS.Views.Default
{
    /// <summary>
    /// 提示框方向
    /// </summary>
    public enum TipOrientation
    {
        /// <summary>
        /// 左上 0.9,1.5
        /// </summary>
        upLeft,
        /// <summary>
        /// 右上 0.05,1.4
        /// </summary>
        upRight,
        /// <summary>
        /// 正上方 0.26,1.375
        /// </summary>
        up,
        /// <summary>
        /// 正右边 -0.09,0.3
        /// </summary>
        right,
        /// <summary>
        /// 右下 0.05,-0.4
        /// </summary>
        downRight,
        /// <summary>
        /// 正下方 0.26,-0.375
        /// </summary>
        down,
        /// <summary>
        /// 左下方 0.88,-0.4
        /// </summary>
        downLeft,
        /// <summary>
        /// 正左边 1.09,0.3
        /// </summary>
        left
    }

    /// <summary>
    /// TipControl.xaml 的交互逻辑
    /// </summary>
    public partial class TipControl : UserControl
    {
        public TipControl()
        {
            InitializeComponent();
            InitPosition();
        }
        private Dictionary<TipOrientation, Point> tipPosition = new Dictionary<TipOrientation, Point>();
        /// <summary>
        /// 初始化 提示框箭头坐标
        /// </summary>
        private void InitPosition()
        {
            tipPosition.Add(TipOrientation.upLeft, new Point(0.9, 1.5));
            tipPosition.Add(TipOrientation.upRight, new Point(0.05, 1.4));
            tipPosition.Add(TipOrientation.up, new Point(0.26, 1.375));
            tipPosition.Add(TipOrientation.right, new Point(-0.09, 0.3));
            tipPosition.Add(TipOrientation.downRight, new Point(0.05, -0.4));
            tipPosition.Add(TipOrientation.down, new Point(0.26, -0.375));
            tipPosition.Add(TipOrientation.downLeft, new Point(0.88, -0.4));
            tipPosition.Add(TipOrientation.left, new Point(1.09, 0.3));
        }


        /// <summary>
        /// 显示提示框   （BY 罗鹏 2012-07-07 加了个Commain 参数）
        /// </summary>
        /// <param name="control">需要定位的控件</param>
        /// <param name="msg">信息</param>
        /// <param name="Commain">需要定位的控件所在的页面(一般传入 this 即可)</param>   
        /// <param name="orientation">方向 默认右上方</param>
        /// <param name="width">提示气泡的宽度</param>
        /// <param name="height">提示气泡的高度</param>
        public void ShowTip(FrameworkElement control, object msg, ContentControl Commain, TipOrientation orientation = TipOrientation.upRight, double width = 150, double height = 30)
        {
            if (isDoing)
            {
                t.Abort();
            }
            isDoing = true;
            this.tipCallout.Height = height;
            this.tipCallout.Width = width;

            ChangeTipStatus(System.Windows.Visibility.Visible);
            this.tipCallout.Content = msg;
            FixedPosition(control, orientation, Commain);

            this.tipCallout.Opacity = 1.0;
            ReduceOpacity();
        }

        #region 降低透明度
        bool isDoing = false;
        Thread t = null;

        private void ReduceOpacity()
        {
            t = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(2000);
                for (int i = 0; i <= 10; i++)
                {
                    Thread.Sleep(100);
                    this.Dispatcher.BeginInvoke(new Action<int>(ReduceOpacity), i);
                }
                ChangeTipStatus(System.Windows.Visibility.Collapsed);
                isDoing = false;
            }));
            t.Start();
        }
        /// <summary>
        /// 更改显示状态
        /// </summary>
        /// <param name="visibility"></param>
        private void ChangeTipStatus(Visibility visibility)
        {
            this.Dispatcher.Invoke(new Action(() => { this.Visibility = visibility; }));
        }

        /// <summary>
        /// 降低透明度
        /// </summary>
        /// <param name="i"></param>
        private void ReduceOpacity(int i)
        {
            this.Dispatcher.Invoke(new Action(() => { this.tipCallout.Opacity = 1.0 - i / 10.0; }));
        }
        #endregion

        #region 定位
        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="control"></param>
        /// <param name="orientation"></param>
        private void FixedPosition(FrameworkElement control, TipOrientation orientation, ContentControl Commain)
        {
            //尖角的长度
            int angleLength = 5;
            double tipWidht = tipCallout.Width;
            double tipHeight = tipCallout.Height;

            //控件距离窗体边界的距离
            Point screenPoint = control.TranslatePoint(new Point(0, 0), Commain);

            switch (orientation)
            {
                case TipOrientation.up:
                    if (screenPoint.Y > tipHeight + angleLength &&
                        Commain.ActualWidth - screenPoint.X - tipWidht > 0)
                    {
                        this.tipCallout.AnchorPoint = tipPosition[TipOrientation.up];
                        this.Margin = new Thickness(screenPoint.X, screenPoint.Y - tipHeight - angleLength, 0, 0);
                    }
                    else
                        goto case TipOrientation.upRight;
                    break;
                case TipOrientation.upRight:
                    if (Commain.ActualWidth - screenPoint.X - control.ActualWidth / 2 > tipWidht &&
                        screenPoint.Y > tipHeight + angleLength
                        )
                    {
                        this.tipCallout.AnchorPoint = tipPosition[TipOrientation.upRight];
                        this.Margin = new Thickness(screenPoint.X + control.ActualWidth / 2, screenPoint.Y - tipHeight - angleLength, 0, 0);
                    }
                    else
                        goto case TipOrientation.right;
                    break;
                case TipOrientation.right:
                    if (Commain.ActualWidth - screenPoint.X - control.ActualWidth > tipWidht + angleLength &&
                        Commain.ActualHeight - screenPoint.Y - tipHeight > 0)
                    {
                        this.tipCallout.AnchorPoint = tipPosition[TipOrientation.right];
                        this.Margin = new Thickness(screenPoint.X + control.ActualWidth + angleLength, screenPoint.Y, 0, 0);
                    }
                    else
                        goto case TipOrientation.downRight;
                    break;
                case TipOrientation.downRight:
                    if (Commain.ActualWidth - screenPoint.X - control.ActualWidth / 2 > tipWidht &&
                        Commain.ActualHeight - screenPoint.Y - control.ActualHeight > tipHeight + angleLength
                        )
                    {
                        this.tipCallout.AnchorPoint = tipPosition[TipOrientation.downRight];
                        this.Margin = new Thickness(screenPoint.X + control.ActualWidth / 2, screenPoint.Y + control.ActualHeight + angleLength, 0, 0);
                    }
                    else
                        goto case TipOrientation.down;
                    break;
                case TipOrientation.down:
                    if (Commain.ActualHeight - screenPoint.Y - control.ActualHeight > tipHeight + angleLength &&
                        Commain.ActualWidth - screenPoint.X - tipWidht > 0
                        )
                    {
                        this.tipCallout.AnchorPoint = tipPosition[TipOrientation.down];
                        this.Margin = new Thickness(screenPoint.X, screenPoint.Y + control.ActualHeight + angleLength, 0, 0);
                    }
                    else
                    {
                        goto case TipOrientation.downLeft;
                    }
                    break;
                case TipOrientation.downLeft:
                    if (screenPoint.X + control.ActualWidth / 2 > tipWidht &&
                        Commain.ActualHeight - screenPoint.Y - control.ActualHeight > tipHeight + angleLength
                        )
                    {
                        this.tipCallout.AnchorPoint = tipPosition[TipOrientation.downLeft];
                        this.Margin = new Thickness(screenPoint.X + control.ActualWidth / 2 - tipWidht, screenPoint.Y + control.ActualHeight + angleLength, 0, 0);
                    }
                    else
                        goto case TipOrientation.left;
                    break;
                case TipOrientation.left:
                    if (screenPoint.X > tipWidht + angleLength &&
                        Commain.ActualHeight - screenPoint.Y - tipHeight > 0)
                    {
                        this.tipCallout.AnchorPoint = tipPosition[TipOrientation.left];
                        this.Margin = new Thickness(screenPoint.X - tipWidht - angleLength, screenPoint.Y, 0, 0);
                    }
                    else
                        goto case TipOrientation.upLeft;
                    break;
                case TipOrientation.upLeft:
                    if (screenPoint.X + control.ActualWidth / 2 > tipWidht && screenPoint.Y > tipHeight + angleLength)
                    {
                        this.tipCallout.AnchorPoint = tipPosition[TipOrientation.upLeft];
                        this.Margin = new Thickness(screenPoint.X + control.ActualWidth / 2 - tipWidht, screenPoint.Y - tipHeight - angleLength, 0, 0);
                    }
                    else
                        goto case TipOrientation.up;
                    break;
            }
        }
        #endregion
    }

}
