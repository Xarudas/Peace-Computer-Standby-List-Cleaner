using PСSLC.Core.Consts;
using PСSLC.WPF.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PСSLC.WPF.Service
{
    internal abstract class ServiceBase
    {
        public static bool IsInstalled
        {
            get
            {
                var service = Service;
                return service == null ? false : true;
            }
        }
        protected static ServiceController Service
        {
            get
            {
                return GetService();
            }
        }
        private static ServiceController GetService()
        {
            ServiceController[] services;
            try
            {
                services = ServiceController.GetServices();
            }
            catch (Exception)
            {
                throw;
            }
            var service = services.FirstOrDefault(s => s.ServiceName == ServiceDataConsts.ServiceName);
            return service;
        }
    }
}
