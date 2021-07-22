namespace PСSLC.WPF.Consts
{
    internal static class RecoveryConsts
    {

        public const string FileName = "cmd.exe";
        public const string WorkingDirectory = "C:";
        public const string Arguments = "/c cd C:/windows/system32 & lodctr /r & cd C:/windows/SysWOW64 & lodctr /r";
    }
}
