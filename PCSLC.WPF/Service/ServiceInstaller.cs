using PСSLC.Core;
using PСSLC.WPF.Consts;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PСSLC.WPF.Service
{
    internal class ServiceInstaller : ServiceBase
    {
        private static string ServicePath
        {
            get
            {
                
                var currentDirectory = Directory.GetCurrentDirectory();
                currentDirectory = currentDirectory.Replace('\\', '/');
                var path = currentDirectory + ServiceInstallerConsts.ServicePath;
                return path;
            }
        }
        private ServiceInstaller() { }
        public static void Install()
        {
            if (IsInstalled)
            {
                throw new Exception(ServiceInfoConsts.ServiceIsInstalled);
            }
            var writter = new RegulationsDataWritter();
            try
            {
                writter.Write(RegulationsData.Default);
            }
            catch (Exception)
            {
                throw;
            }
            var arguments = $"{ServiceInstallerConsts.StartArguments} {ServicePath}";
            using (Process installProcess = new Process())
            {
                installProcess.StartInfo.WorkingDirectory = ServiceInstallerConsts.WorkingDirectory;
                installProcess.StartInfo.FileName = ServiceInstallerConsts.FileName;
                installProcess.StartInfo.Arguments = arguments;
                installProcess.StartInfo.CreateNoWindow = true;
                installProcess.StartInfo.UseShellExecute = false;
                try
                {
                    installProcess.Start();
                }
                catch (Exception)
                {
                    throw;
                }
                int retryCount = 0;
                while (!IsInstalled && retryCount < 50)
                {
                    Thread.Sleep(100);
                    retryCount++;
                }
                if (retryCount == 50)
                {
                    throw new InvalidOperationException(ServiceInfoConsts.ServiceIsNotSucceededInstall);
                }
            }
            
        }
        public static void Delete()
        {
            if (!IsInstalled)
            {
                throw new Exception(ServiceInfoConsts.ServiceIsNotInstalled);
            }
            var arguments = $"{ServiceInstallerConsts.StartArguments} {ServicePath} {ServiceInstallerConsts.DeleteArgument}";
            using (Process deleteProcess = new Process())
            {
                deleteProcess.StartInfo.WorkingDirectory = ServiceInstallerConsts.WorkingDirectory;
                deleteProcess.StartInfo.FileName = ServiceInstallerConsts.FileName;
                deleteProcess.StartInfo.Arguments = arguments;
                deleteProcess.StartInfo.CreateNoWindow = true;
                deleteProcess.StartInfo.UseShellExecute = false;
                try
                {
                    deleteProcess.Start();
                }
                catch (Exception)
                {
                    throw;
                }
                int retryCount = 0;
                while (IsInstalled && retryCount < 50)
                {
                    Thread.Sleep(100);
                    retryCount++;
                }
                if (retryCount == 50)
                {
                    throw new InvalidOperationException(ServiceInfoConsts.ServiceIsNotSucceededDelete);
                }
                else
                {
                    var writter = new RegulationsDataWritter();
                    writter.RemoveAll();
                }
            }
        }
    }
}
