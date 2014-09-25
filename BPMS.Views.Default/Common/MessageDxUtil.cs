using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using System.Windows.Forms;

//http://www.codeproject.com/Articles/20347/How-to-Get-IWin32Window-Handle-Used-in-Windows-For
//http://www.verydemo.com/demo_c293_i12681.html
//http://www.cnblogs.com/xiaofengfeng/archive/2011/09/16/2178637.html

namespace BPMS.Views.Default
{
    public class MessageDxUtil
    {
        /// <summary>
        /// 显示一般的提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        public static DialogResult ShowTips(string message)
        {
            RegisterSkin();
            return XtraMessageBox.Show(message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        public static DialogResult ShowWarning(string message)
        {
            RegisterSkin();
            return XtraMessageBox.Show(message, "警告信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowError(string message)
        {
            RegisterSkin();
            return XtraMessageBox.Show(message, "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示询问用户信息，并显示错误标志
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowYesNoAndError(string message)
        {
            RegisterSkin();
            return XtraMessageBox.Show(message, "错误信息", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowYesNoAndTips(string message)
        {
            RegisterSkin();
            return XtraMessageBox.Show(message, "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示询问用户信息，并显示警告标志
        /// </summary>
        /// <param name="message">警告信息</param>
        public static DialogResult ShowYesNoAndWarning(string message)
        {
            RegisterSkin();
            return XtraMessageBox.Show(message, "警告信息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowYesNoCancelAndTips(string message)
        {
            RegisterSkin();
            return XtraMessageBox.Show(UserLookAndFeel.Default, message, "提示信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 注册皮肤
        /// </summary>
        private static void RegisterSkin()
        {
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
        }
    }
}
