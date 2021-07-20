using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace PСSLC.Core
{
    public class Win32_PrivilegeElevation
    {
		private const int SE_PRIVILEGE_ENABLED = 2;

		private const string SE_INCREASE_QUOTA_NAME = "SeIncreaseQuotaPrivilege";

		public const string SE_PROFILE_SINGLE_PROCESS_NAME = "SeProfileSingleProcessPrivilege";

		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

		public static bool SetIncreasePrivilege(string privilegeName)
		{
			using (WindowsIdentity current = WindowsIdentity.GetCurrent(TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges))
			{
				TokPriv1Luid newst = default(TokPriv1Luid);
				newst.Count = 1;
				newst.Luid = 0L;
				newst.Attr = 2;
				if (!LookupPrivilegeValue(null, privilegeName, ref newst.Luid))
				{
					throw new Exception("Error in LookupPrivilegeValue: ", new Win32Exception(Marshal.GetLastWin32Error()));
				}
				int num = AdjustTokenPrivileges(current.Token, disall: false, ref newst, 0, IntPtr.Zero, IntPtr.Zero) ? 1 : 0;
				if (num == 0)
				{
					throw new Exception("Error in AdjustTokenPrivileges: ", new Win32Exception(Marshal.GetLastWin32Error()));
				}
				return num != 0;
			}
		}
	}
}
