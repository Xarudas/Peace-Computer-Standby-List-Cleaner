namespace PСSLC.Core
{
    public struct RegulationsData
    {
        public ulong StandbyMemory { get; private set; }
        public ulong FreeMemory { get; private set; }
        public int ServiceThreadSleepMilliseconds { get; private set; }
        public static RegulationsData Default => new RegulationsData(1024, 1024, 5000);
        public RegulationsData(ulong standbyMemory, ulong freeMemory, int serviceThreadSleepMilliseconds)
        {
            StandbyMemory = standbyMemory;
            FreeMemory = freeMemory;
            ServiceThreadSleepMilliseconds = serviceThreadSleepMilliseconds;
        }
    }
}
