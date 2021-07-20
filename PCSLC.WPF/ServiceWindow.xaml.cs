using PСSLC.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using PСSLC.WPF.Service;
using System.Text.RegularExpressions;
using PСSLC.WPF.Models;
using PСSLC.WPF.Utility;
using PСSLC.WPF.Consts;

namespace PСSLC.WPF
{
    public partial class ServiceWindow : Window
    {
        private Action _serviceStateChanged;
        private readonly RegulationdDataModel _model;
        public ServiceWindow()
        {
            InitializeComponent();
            _model = new RegulationdDataModel();
            DataContext = _model;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _serviceStateChanged += OnServiceStateChange;
            ChangeGridVisibility();
            if (ServiceBase.IsInstalled)
            {
                SetTextBoxData();
                _serviceStateChanged.Invoke();
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _serviceStateChanged -= OnServiceStateChange;
        }
        private void button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationHasError())
            {
                MessageBoxUtility.ShowWarn("Ошибка в введенных данных. Отмена запуска службы!");
                return;
            }
            var standbyMemory = _model.StandbyMemory;
            var freeMemory = _model.FreeMemory;
            var milliseconds = _model.ThreadSleepMilliseconds;
            var data = new RegulationsData(standbyMemory, freeMemory, milliseconds);
            var writter = new RegulationsDataWritter();
            try
            {
                writter.Write(data);
            }
            catch (Exception ex)
            {
                MessageBoxUtility.ShowException(ex);
            } 
            try
            {
                ServiceRunnner.Run();
            }
            catch (Exception ex)
            {
                MessageBoxUtility.ShowException(ex);
            }
            _serviceStateChanged.Invoke();
        }
        private void button_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ServiceRunnner.Stop();
            }
            catch (Exception ex)
            {
                MessageBoxUtility.ShowException(ex);
            }
            SetTextBoxData();
            _serviceStateChanged.Invoke();
        }
        private void button_InstallService_Click(object sender, RoutedEventArgs e)
        {
            if (!ServiceBase.IsInstalled)
            {
                try
                {
                    ServiceInstaller.Install();
                }
                catch (Exception ex)
                {
                    MessageBoxUtility.ShowException(ex);
                }
            }
            else
            {
                MessageBoxUtility.ShowWarn("Служба уже установлена");
            }
            ChangeGridVisibility();
            SetTextBoxData();
            _serviceStateChanged.Invoke();
        }
        private void button_DeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceBase.IsInstalled)
            {
                try
                {
                    ServiceInstaller.Delete();
                }
                catch (Exception ex)
                {
                    MessageBoxUtility.ShowException(ex);
                }
            }
            else
            {
                MessageBoxUtility.ShowWarn(ServiceInfoConsts.ServiceIsNotInstalled);
            }
            ChangeGridVisibility();
        }
        private void ChangeGridVisibility()
        {
            if (ServiceBase.IsInstalled)
            {
                grid_InstallService.Visibility = Visibility.Hidden;
                grid_ServiceControl.Visibility = Visibility.Visible;
            }
            else
            {
                grid_InstallService.Visibility = Visibility.Visible;
                grid_ServiceControl.Visibility = Visibility.Hidden;
            }
        }
        private void OnServiceStateChange()
        {
            ChangeButtonsVisiblity();
            ChangeTextBoxsInteractable();
        }
        private void ChangeButtonsVisiblity()
        {
            if (ServiceBase.IsInstalled)
            {
                if (ServiceRunnner.IsRunned)
                {
                    button_Start.Visibility = Visibility.Hidden;
                    button_Stop.Visibility = Visibility.Visible;
                }
                else
                {
                    button_Start.Visibility = Visibility.Visible;
                    button_Stop.Visibility = Visibility.Hidden;
                }
            }
        }
        private void ChangeTextBoxsInteractable()
        {
            if (ServiceBase.IsInstalled)
            {
                if (ServiceRunnner.IsRunned)
                {
                    textBox_FreeMemory.IsEnabled = false;
                    textBox_StandbyMemory.IsEnabled = false;
                    textBox_ThreadSleepMilliseconds.IsEnabled = false;
                }
                else
                {
                    textBox_FreeMemory.IsEnabled = true;
                    textBox_StandbyMemory.IsEnabled = true;
                    textBox_ThreadSleepMilliseconds.IsEnabled = true;
                }
            }
        }
        private void SetTextBoxData()
        {
            var reader = new RegulationsDataReader();
            var data = reader.Read();
            _model.StandbyMemory = data.StandbyMemory;
            _model.FreeMemory = data.FreeMemory;
            _model.ThreadSleepMilliseconds = data.ServiceThreadSleepMilliseconds;
        }
        private bool ValidationHasError()
        {
            bool validationHasError = false;
            if (Validation.GetHasError(textBox_StandbyMemory))
            {
                validationHasError = true;
            }
            if (Validation.GetHasError(textBox_FreeMemory))
            {
                validationHasError = true;
            }
            if (Validation.GetHasError(textBox_ThreadSleepMilliseconds))
            {
                validationHasError = true;
            }
            return validationHasError;
        }
    }
}
