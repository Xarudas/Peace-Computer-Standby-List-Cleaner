using PСSLC.Core.Consts;
using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;

namespace PСSLC.Service
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {

        private readonly ServiceInstaller serviceInstaller;
        private readonly ServiceProcessInstaller processInstaller;

        public Installer()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = ServiceDataConsts.ServiceName;
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
        }

        protected override void OnAfterUninstall(IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
        }
    }
}
