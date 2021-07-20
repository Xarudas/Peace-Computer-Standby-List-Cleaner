using System;
using System.ServiceProcess;
using System.Threading;
using PСSLC.Core;
using NLog;

namespace PСSLC.Service
{
    public partial class Service : ServiceBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private Action _serviceStopped;
        private Thread _logicThread;
        private MemoryCounter _memoryCounter;
        private RegulationsData _data;

        public Service()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = false;
            _logger.Debug("Service Initialize");
        }

        protected override void OnStart(string[] args)
        {
            _logger.Info("Service starts work");
            _serviceStopped += OnServiceStop;
            _logger.Debug("Initialize MemoryCounter");
            _memoryCounter = new MemoryCounter();
            _logger.Debug("Initialize RegulationsDataReader");
            var dataReader = new RegulationsDataReader();
            try
            {
                _logger.Info("Get regulartions data");
                _data = dataReader.Read();
                _logger.Info($"Regulations data getting: s.m: {_data.StandbyMemory}; f.m: {_data.FreeMemory}; t.s: {_data.ServiceThreadSleepMilliseconds}");
            }
            catch (NullReferenceException ex)
            {
                _logger.Error(ex.Message);
            }
            try
            {
                ValidateRegulationsData(_data);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
            _logger.Debug("Initialize LogicThread");
            _logicThread = new Thread(PurgeLogic);
            try
            {
                _logger.Debug("LogicThread starts works");
                _logicThread.Start();
            }
            catch (OutOfMemoryException ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
            _logger.Debug("LogicThread started");
            _logger.Info("Service started");
            
        }
        protected override void OnStop()
        {
            _logger.Info("Service stop works");
            _serviceStopped.Invoke();
            _serviceStopped -= OnServiceStop;
            _logger.Info("Service stopped");
        }

        private void PurgeLogic()
        {
            while (true)
            {
                if (_memoryCounter.StanbyListMemory >= _data.StandbyMemory && _memoryCounter.FreeMemory <= _data.FreeMemory)
                {
                    _logger.Info("Clear Cache");
                    ClearStandbyList();
                }
                Thread.Sleep(_data.ServiceThreadSleepMilliseconds);
            }
        }
        private void OnServiceStop()
        {
            try
            {
                _logger.Debug("LogicThread Abort");
                _logicThread.Abort();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
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
