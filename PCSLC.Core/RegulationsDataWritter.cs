using System;
using Microsoft.Win32;
using PСSLC.Core.Consts;

namespace PСSLC.Core
{
    public class RegulationsDataWritter
    {
        public void Write(RegulationsData data)
        {
            RegistryKey localMachineKey = Registry.LocalMachine;
            RegistryKey softwareKey;
            try
            {
                softwareKey = localMachineKey.OpenSubKey(RegulationDataConsts.RegSoftwareKey, true);
            }
            catch (Exception)
            {
                throw;
            }
            RegistryKey peaceDukeKey;
            try
            {
                peaceDukeKey = softwareKey.OpenSubKey(RegulationDataConsts.RegISLCPeaceDukeKey, true);
            }
            catch (Exception)
            {
                throw;
            }
            if (peaceDukeKey == null)
            {
                try
                {
                    peaceDukeKey = softwareKey.CreateSubKey(RegulationDataConsts.RegISLCPeaceDukeKey, true);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            try
            {
                peaceDukeKey.SetValue(RegulationDataConsts.RegStandByMemoryKey, data.StandbyMemory);
                peaceDukeKey.SetValue(RegulationDataConsts.RegFreeMemoryKey, data.FreeMemory);
                peaceDukeKey.SetValue(RegulationDataConsts.RegThreadSleepMilliseconds, data.ServiceThreadSleepMilliseconds);
            }
            catch (Exception)
            {

                throw;
            }
            peaceDukeKey.Close();
        }
        public void RemoveAll()
        {
            RegistryKey localMachineKey = Registry.LocalMachine;
            var softwareKey = localMachineKey.OpenSubKey(RegulationDataConsts.RegSoftwareKey, true);
            var peaceDukeKey = softwareKey.OpenSubKey(RegulationDataConsts.RegISLCPeaceDukeKey, true);
            if (peaceDukeKey != null)
            {
                softwareKey.DeleteSubKeyTree(RegulationDataConsts.RegISLCPeaceDukeKey);
            }
        }
    }
}
