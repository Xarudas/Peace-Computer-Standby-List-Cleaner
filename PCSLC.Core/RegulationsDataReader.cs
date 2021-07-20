using System;
using PСSLC.Core.Consts;
using Microsoft.Win32;

namespace PСSLC.Core
{
    public class RegulationsDataReader
    {

        public RegulationsData Read()
        {
            RegistryKey localMachineKey = Registry.LocalMachine;
            var softwareKey = localMachineKey.OpenSubKey(RegulationDataConsts.RegSoftwareKey);
            var peaceDukeKey = softwareKey.OpenSubKey(RegulationDataConsts.RegISLCPeaceDukeKey);
            if (peaceDukeKey == null)
            {
                throw new NullReferenceException("PeaceDukeKey is null");
            }
            ulong standbyMemory = Convert.ToUInt64(peaceDukeKey.GetValue(RegulationDataConsts.RegStandByMemoryKey));
            ulong freeMemoey = Convert.ToUInt64(peaceDukeKey.GetValue(RegulationDataConsts.RegFreeMemoryKey));
            int threadSleepMilliseconds = Convert.ToInt32(peaceDukeKey.GetValue(RegulationDataConsts.RegThreadSleepMilliseconds));
            var data = new RegulationsData(standbyMemory, freeMemoey, threadSleepMilliseconds);
            return data;
        }
    }
}
