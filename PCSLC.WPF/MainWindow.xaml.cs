using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using PСSLC.Core;
using PСSLC.WPF.Consts;
using PСSLC.WPF.Service;
using PСSLC.WPF.Utility;
using PСSLC.WPF.Models;

namespace PСSLC.WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MemoryCounter _memoryCounter;
        private readonly Thread _updateUIThread;
        private readonly AppDataModel _model;
        
        public MainWindow()
        {
            InitializeComponent();
            _model = new AppDataModel();
            DataContext = _model;
            try
            {
                _memoryCounter = new MemoryCounter();
                _updateUIThread = new Thread(UpdateUI);
                _updateUIThread.Start();
            }
            catch (Exception ex)
            {
                MessageBoxUtility.ShowException(ex);
                throw;
            }
            
        }

        private void button_PurgeStandbyList_Click(object sender, RoutedEventArgs e)
        {
            ClearStandbyList();
        }
        private void window_MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (_updateUIThread.IsAlive)
                {
                    _updateUIThread.Abort();
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtility.ShowException(ex);
                throw;
            }
        }
        private void menuItem_ServiceSettings_Click(object sender, RoutedEventArgs e)
        {
            ServiceWindow serviceWindow = new ServiceWindow();
            serviceWindow.Owner = this;
            serviceWindow.ShowDialog();
        }
        private void menuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void UpdateUI()
        {

            _model.TotalSystemMemory = _memoryCounter.TotalSystemMemory;
            do
            {
                UpdateMemoryCounterInfo();
                UpdateServiceStatusInfo();
                Thread.Sleep(1000);
            }
            while (true);
        }
        private void UpdateMemoryCounterInfo()
        {
            _model.UsedMemory = (_memoryCounter.TotalSystemMemory - _memoryCounter.FreeMemory) / (float)_memoryCounter.TotalSystemMemory * 100;
            _model.FreeMemory = _memoryCounter.FreeMemory;
            _model.StandbyMemory = _memoryCounter.StanbyListMemory;
        }
        private void UpdateServiceStatusInfo()
        {
            if (ServiceBase.IsInstalled)
            {
                if (ServiceRunnner.IsRunned)
                {
                    _model.ServiceInfo = ServiceInfoConsts.ServiceIsRunned;
                }
                else
                {
                    _model.ServiceInfo = ServiceInfoConsts.ServiceIsStopped;
                }
            }
            else
            {
                _model.ServiceInfo = ServiceInfoConsts.ServiceIsNotInstalled;
            }
        }
        private void ClearStandbyList()
        {
            try
            {
                new Win32_NtSystemInformation().ClearStandbyCache();
            }
            catch (Exception ex)
            {
                MessageBoxUtility.ShowException(ex);
            }
            
        }

        private void textBlock_ChannelLink_Click(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.youtube.com/c/ThePeaceDuke");
        }
    }
}
