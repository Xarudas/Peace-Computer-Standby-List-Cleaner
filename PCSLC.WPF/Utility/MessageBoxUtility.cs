using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PСSLC.WPF.Consts;

namespace PСSLC.WPF.Utility
{
    public static class MessageBoxUtility
    {
        public static void ShowException(Exception exception)
        {
            MessageBox.Show(exception.Message, MessageBoxConsts.ErrorCaption);
        }
        public static void ShowWarn(string message)
        {
            MessageBox.Show(message, MessageBoxConsts.WarnCaption);
        }
    }
}
