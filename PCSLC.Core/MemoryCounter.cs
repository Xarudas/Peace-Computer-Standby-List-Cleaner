using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PСSLC.Core
{
    public class MemoryCounter
    {
        public ulong TotalSystemMemory { get; }
        public ulong StanbyListMemory 
        {
            get
            {
                return Convert.ToUInt64(StandbyCacheNormal.NextValue()) + Convert.ToUInt64(StandbyCacheCoreBytes.NextValue()) + Convert.ToUInt64(StandbyCacheReserve.NextValue()) >> 20;
            } 
        } 
        public ulong FreeMemory
        {
            get
            {
                return Convert.ToUInt64(AvailableMemoryMBCounter.NextValue()) >> 20;
            }
        }
        private PerformanceCounter StandbyCacheNormal { get; }

        private PerformanceCounter StandbyCacheCoreBytes { get; }

        private PerformanceCounter StandbyCacheReserve { get; }

        private PerformanceCounter AvailableMemoryMBCounter { get; }
        public MemoryCounter()
        {
            try
            {
                StandbyCacheNormal = new PerformanceCounter("Memory", "Standby Cache Normal Priority Bytes");
                StandbyCacheCoreBytes = new PerformanceCounter("Memory", "Standby Cache Core Bytes");
                StandbyCacheReserve = new PerformanceCounter("Memory", "Standby Cache Reserve Bytes");
                AvailableMemoryMBCounter = new PerformanceCounter("Memory", "Free & Zero Page List Bytes");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Не удается загрузить данные имени счетчика, для исправления данной ошибки обратитесь к FAQ", ex);
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                MemoryStatusEx memoryStatus = new MemoryStatusEx();
                if (Win32_NtSystemInformation.GlobalMemoryStatusEx(memoryStatus))
                {
                    TotalSystemMemory = memoryStatus.ullTotalPhys >> 20;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
