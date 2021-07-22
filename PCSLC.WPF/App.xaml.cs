using PСSLC.WPF.Consts;
using PСSLC.WPF.Utility;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace PСSLC.WPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain currentDomain;
            currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            switch (exception)
            {
                case XamlParseException ex:
                    RecoveryCounters();
                    break;
                default:
                    MessageBoxUtility.ShowException(exception);
                    break;
            }
        }
        private void RecoveryCounters()
        {
            using (Process recoveryProcess = new Process())
            {
                recoveryProcess.StartInfo.WorkingDirectory = RecoveryConsts.WorkingDirectory;
                recoveryProcess.StartInfo.FileName = RecoveryConsts.FileName;
                recoveryProcess.StartInfo.Arguments = RecoveryConsts.Arguments;
                recoveryProcess.StartInfo.CreateNoWindow = true;
                recoveryProcess.StartInfo.UseShellExecute = false;
                try
                {
                    recoveryProcess.Start();
                }
                catch (Exception ex)
                {
                    MessageBoxUtility.ShowException(ex);
                    throw;
                }
            }
            MessageBoxUtility.ShowWarn("Ваши счетчики производительности находились в ошибке. Мы их восстановили. Перезапустите программу.");
        }
    }
}
