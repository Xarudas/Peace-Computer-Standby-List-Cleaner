using System;
using System.ServiceProcess;
using System.Threading;
using PСSLC.WPF.Consts;

namespace PСSLC.WPF.Service
{
    internal class ServiceRunnner : ServiceBase
    {
        public static bool IsRunned
        {
            get
            {
                if (!IsInstalled)
                {
                    return false;
                }
                var service = Service;
                return service.Status == ServiceControllerStatus.Running ? true : false;
            }
        }
        private ServiceRunnner() { }
        public static void Run()
        {
            if (!IsInstalled)
            {
                throw new Exception(ServiceInfoConsts.ServiceIsNotInstalled);
            }
            if (IsRunned)
            {
                throw new Exception(ServiceInfoConsts.ServiceIsRunned);
            }
            try
            {
                Service.Start();
            }
            catch (Exception)
            {
                throw;
            }
            while (Service.Status != ServiceControllerStatus.Running)
            {
                Thread.Sleep(100);
            }
        }
        public static void Stop()
        {
            if (!IsInstalled)
            {
                throw new Exception(ServiceInfoConsts.ServiceIsNotInstalled);
            }
            if (!IsRunned)
            {
                throw new Exception(ServiceInfoConsts.ServiceIsStopped);
            }
            try
            {
                Service.Stop();
            }
            catch (Exception)
            {
                throw;
            }
            while(Service.Status != ServiceControllerStatus.Stopped)
            {
                Thread.Sleep(100);
            }
            
        }
        
    }
}
