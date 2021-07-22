using System;
using System.ServiceProcess;
using System.Threading;
using PСSLC.Core;

namespace PСSLC.Service
{
    public partial class Service : ServiceBase
    {
        private Action _serviceStopped;
        private Thread _logicThread;
        private MemoryCounter _memoryCounter;
        private RegulationsData _data;

        public Service()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = false;
        }

        protected override void OnStart(string[] args)
        {
            _serviceStopped += OnServiceStop;
            _memoryCounter = new MemoryCounter();
            var dataReader = new RegulationsDataReader();
            try
            {
                _data = dataReader.Read();
            }
            catch (NullReferenceException)
            {
                throw;
            }
            try
            {
                ValidateRegulationsData(_data);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
            _logicThread = new Thread(PurgeLogic);
            try
            {
                _logicThread.Start();
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            
        }
        protected override void OnStop()
        {
            _serviceStopped.Invoke();
            _serviceStopped -= OnServiceStop;
        }

        private void PurgeLogic()
        {
            while (true)
            {
                if (_memoryCounter.StanbyListMemory >= _data.StandbyMemory && _memoryCounter.FreeMemory <= _data.FreeMemory)
                {
                    ClearStandbyList();
                }
                Thread.Sleep(_data.ServiceThreadSleepMilliseconds);
            }
        }
        private void OnServiceStop()
        {
            try
            {
                _logicThread.Abort();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void ClearStandbyList()
        {
            new Win32_NtSystemInformation().ClearStandbyCache();
        }

        private void ValidateRegulationsData(RegulationsData data)
        {
            if (data.FreeMemory <= 0)
            {
                throw new ArgumentOutOfRangeException("FreeMemory cannot be equal to or less than 0");
            }
            if (data.StandbyMemory <= 0)
            {
                throw new ArgumentOutOfRangeException("StandByMemory cannot be equal to or less than 0");
            }
            if (data.ServiceThreadSleepMilliseconds <= 0)
            {
                throw new ArgumentOutOfRangeException("ServiceThreadSleepMilliseconds cannot be equal to or less than 0");
            }
            if (data.FreeMemory >= _memoryCounter.TotalSystemMemory)
            {
                throw new ArgumentOutOfRangeException($"FreeMemory cannot be equal to or greater than {_memoryCounter.TotalSystemMemory}");
            }
            if (data.StandbyMemory >= _memoryCounter.TotalSystemMemory)
            {
                throw new ArgumentOutOfRangeException($"StandByMemory cannot be equal to or greater than {_memoryCounter.TotalSystemMemory}");
            }
        }
    }
}
