using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace BPMS.Views.Default
{
    /// <summary>
    /// 遮罩
    /// </summary>
    public partial class MaskControl : UserControl
    {
        public MaskControl()
        {
            InitializeComponent();
        }

        #region 对外公布属性
        [Category("Display Options")]
        [DefaultValue("Please Wait")]
        [Description("标题")]
        public string Caption
        {
            get { return tbCaption.Text; }
            set { tbCaption.Text = value.Trim(); }
        }

        #endregion
        /// <summary>
        /// 显示遮罩层
        /// </summary>
        public void Show()
        {
            try
            {
                MaskFloor.Visibility = Visibility.Visible;
                InfFloor.Visibility = Visibility.Visible;

                Storyboard myStoryboard = GetStoryboard(0, 0.3, 0, 1, 0.2, 0.2);
                myStoryboard.Begin(this);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 显隐藏遮罩层
        /// </summary>
        public void Hidden()
        {
            try
            {
                Storyboard myStoryboard = GetStoryboard(null, 0, null, 0, 0.1, 0.1);

                myStoryboard.Completed += new EventHandler(Hidden_Completed);

                myStoryboard.Begin(this);
            }
            catch
            {

            }
        }

        void Hidden_Completed(object sender, EventArgs e)
        {
            MaskFloor.Visibility = Visibility.Hidden;
            InfFloor.Visibility = Visibility.Hidden;
        }

        //得到动画画板
        private Storyboard GetStoryboard(double? From1, double To1, double? From2, double To2, double Time1, double Time2)
        {
            Storyboard myStoryboard = new Storyboard();

            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            if (From1 != null)
            {
                myDoubleAnimation.From = From1;
            }
            myDoubleAnimation.To = To1;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(Time1));

            myStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTargetName(myDoubleAnimation, MaskFloor.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Rectangle.OpacityProperty));

            DoubleAnimation myDoubleAnimation1 = new DoubleAnimation();
            if (From2 != null)
            {
                myDoubleAnimation1.From = From2;
            }
            myDoubleAnimation1.To = To2;
            myDoubleAnimation1.Duration = new Duration(TimeSpan.FromSeconds(Time2));

            myStoryboard.Children.Add(myDoubleAnimation1);
            Storyboard.SetTargetName(myDoubleAnimation1, InfFloor.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation1, new PropertyPath(Rectangle.OpacityProperty));

            return myStoryboard;
        }
    }
}
