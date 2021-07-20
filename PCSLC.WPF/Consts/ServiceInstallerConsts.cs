using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PСSLC.WPF.Consts
{
    internal static class ServiceInstallerConsts
    {
        public const string ServicePath = "/PСSLC_Service.exe";
        public const string StartArguments = "/c cd C:/Windows/Microsoft.NET/Framework64/v4.0.30319 & InstallUtil.exe";
        public const string DeleteArgument = "/u";
        public const string FileName = "cmd.exe";
        public const string WorkingDirectory = "C:";

        public const int RetryCount = 50;
    }
}
