using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MobSupport.Class
{
    public static class MainWindowActivator
    {
        /// <summary>
        /// アプリケーションのメインウィンドウをアクティブにします。
        /// 必要であればウィンドウを復元します。
        /// </summary>
        public static void ActivateMainWindow()
        {
            var mainWindow = Application.Current?.MainWindow;

            if (mainWindow != null)
            {
                if (mainWindow.WindowState == WindowState.Minimized)
                {
                    mainWindow.WindowState = WindowState.Normal; // 最小化を解除
                }

                if (!mainWindow.IsActive)
                {
                    mainWindow.Activate(); // ウィンドウをアクティブ化
                }
            }
        }
    }
}
