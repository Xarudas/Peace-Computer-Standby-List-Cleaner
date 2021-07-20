using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PСSLC.Core
{
    public sealed class Win32_NtSystemInformation
    {
		internal enum SYSTEM_INFORMATION_CLASS
		{
			SystemBasicInformation,
			SystemProcessorInformation,
			SystemPerformanceInformation,
			SystemTimeOfDayInformation,
			SystemPathInformation,
			SystemProcessInformation,
			SystemCallCountInformation,
			SystemDeviceInformation,
			SystemProcessorPerformanceInformation,
			SystemFlagsInformation,
			SystemCallTimeInformation,
			SystemModuleInformation,
			SystemLocksInformation,
			SystemStackTraceInformation,
			SystemPagedPoolInformation,
			SystemNonPagedPoolInformation,
			SystemHandleInformation,
			SystemObjectInformation,
			SystemPageFileInformation,
			SystemVdmInstemulInformation,
			SystemVdmBopInformation,
			SystemFileCacheInformation,
			SystemPoolTagInformation,
			SystemInterruptInformation,
			SystemDpcBehaviorInformation,
			SystemFullMemoryInformation,
			SystemLoadGdiDriverInformation,
			SystemUnloadGdiDriverInformation,
			SystemTimeAdjustmentInformation,
			SystemSummaryMemoryInformation,
			SystemMirrorMemoryInformation,
			SystemPerformanceTraceInformation,
			SystemCrashDumpInformation,
			SystemExceptionInformation,
			SystemCrashDumpStateInformation,
			SystemKernelDebuggerInformation,
			SystemContextSwitchInformation,
			SystemRegistryQuotaInformation,
			SystemExtendServiceTableInformation,
			SystemPrioritySeperation,
			SystemVerifierAddDriverInformation,
			SystemVerifierRemoveDriverInformation,
			SystemProcessorIdleInformation,
			SystemLegacyDriverInformation,
			SystemCurrentTimeZoneInformation,
			SystemLookasideInformation,
			SystemTimeSlipNotification,
			SystemSessionCreate,
			SystemSessionDetach,
			SystemSessionInformation,
			SystemRangeStartInformation,
			SystemVerifierInformation,
			SystemVerifierThunkExtend,
			SystemSessionProcessInformation,
			SystemLoadGdiDriverInSystemSpace,
			SystemNumaProcessorMap,
			SystemPrefetcherInformation,
			SystemExtendedProcessInformation,
			SystemRecommendedSharedDataAlignment,
			SystemComPlusPackage,
			SystemNumaAvailableMemory,
			SystemProcessorPowerInformation,
			SystemEmulationBasicInformation,
			SystemEmulationProcessorInformation,
			SystemExtendedHandleInformation,
			SystemLostDelayedWriteInformation,
			SystemBigPoolInformation,
			SystemSessionPoolTagInformation,
			SystemSessionMappedViewInformation,
			SystemHotpatchInformation,
			SystemObjectSecurityMode,
			SystemWatchdogTimerHandler,
			SystemWatchdogTimerInformation,
			SystemLogicalProcessorInformation,
			SystemWow64SharedInformationObsolete,
			SystemRegisterFirmwareTableInformationHandler,
			SystemFirmwareTableInformation,
			SystemModuleInformationEx,
			SystemVerifierTriageInformation,
			SystemSuperfetchInformation,
			SystemMemoryListInformation,
			SystemFileCacheInformationEx,
			SystemThreadPriorityClientIdInformation,
			SystemProcessorIdleCycleTimeInformation,
			SystemVerifierCancellationInformation,
			SystemProcessorPowerInformationEx,
			SystemRefTraceInformation,
			SystemSpecialPoolInformation,
			SystemProcessIdInformation,
			SystemErrorPortInformation,
			SystemBootEnvironmentInformation,
			SystemHypervisorInformation,
			SystemVerifierInformationEx,
			SystemTimeZoneInformation,
			SystemImageFileExecutionOptionsInformation,
			SystemCoverageInformation,
			SystemPrefetchPatchInformation,
			SystemVerifierFaultsInformation,
			SystemSystemPartitionInformation,
			SystemSystemDiskInformation,
			SystemProcessorPerformanceDistribution,
			SystemNumaProximityNodeInformation,
			SystemDynamicTimeZoneInformation,
			SystemCodeIntegrityInformation,
			SystemProcessorMicrocodeUpdateInformation,
			SystemProcessorBrandString,
			SystemVirtualAddressInformation,
			SystemLogicalProcessorAndGroupInformation,
			SystemProcessorCycleTimeInformation,
			SystemStoreInformation,
			SystemRegistryAppendString,
			SystemAitSamplingValue,
			SystemVhdBootInformation,
			SystemCpuQuotaInformation,
			SystemNativeBasicInformation,
			SystemErrorPortTimeouts,
			SystemLowPriorityIoInformation,
			SystemBootEntropyInformation,
			SystemVerifierCountersInformation,
			SystemPagedPoolInformationEx,
			SystemSystemPtesInformationEx,
			SystemNodeDistanceInformation,
			SystemAcpiAuditInformation,
			SystemBasicPerformanceInformation,
			SystemQueryPerformanceCounterInformation,
			SystemSessionBigPoolInformation,
			SystemBootGraphicsInformation,
			SystemScrubPhysicalMemoryInformation,
			SystemBadPageInformation,
			SystemProcessorProfileControlArea,
			SystemCombinePhysicalMemoryInformation,
			SystemEntropyInterruptTimingInformation,
			SystemConsoleInformation,
			SystemPlatformBinaryInformation,
			SystemThrottleNotificationInformation,
			SystemHypervisorProcessorCountInformation,
			SystemDeviceDataInformation,
			SystemDeviceDataEnumerationInformation,
			SystemMemoryTopologyInformation,
			SystemMemoryChannelInformation,
			SystemBootLogoInformation,
			SystemProcessorPerformanceInformationEx,
			SystemSpare0,
			SystemSecureBootPolicyInformation,
			SystemPageFileInformationEx,
			SystemSecureBootInformation,
			SystemEntropyInterruptTimingRawInformation,
			SystemPortableWorkspaceEfiLauncherInformation,
			SystemFullProcessInformation,
			MaxSystemInfoClass
		}

		private enum SYSTEM_MEMORY_LIST_COMMAND
		{
			MemoryCaptureAccessedBits,
			MemoryCaptureAndResetAccessedBits,
			MemoryEmptyWorkingSets,
			MemoryFlushModifiedList,
			MemoryPurgeStandbyList,
			MemoryPurgeLowPriorityStandbyList,
			MemoryCommandMax
		}

		private const int MemoryPurgeStandbyList = 4;

		[DllImport("ntdll.dll")]
		internal static extern uint NtSetSystemInformation(int infoClass, IntPtr info, int length);

		public void ClearStandbyCache()
		{
			if (Win32_PrivilegeElevation.SetIncreasePrivilege("SeProfileSingleProcessPrivilege"))
			{
				try
				{
					int systemInfoLength = Marshal.SizeOf(4);
					GCHandle gcHandle = GCHandle.Alloc(4, GCHandleType.Pinned);
					if (NtSetSystemInformation(80, gcHandle.AddrOfPinnedObject(), systemInfoLength) != 0)
					{
						throw new Exception("NtSetSystemInformation: ", new Win32Exception(Marshal.GetLastWin32Error()));
					}
					gcHandle.Free();
				}
				catch (Exception)
				{
					throw;
				}
			}
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GlobalMemoryStatusEx([In][Out] MemoryStatusEx lpBuffer);
	}
}
