using PСSLC.Core;
using System.ComponentModel;

namespace PСSLC.WPF.Models
{
    public class RegulationdDataModel : PropertyChangedModelBase, IDataErrorInfo
    {
        public ulong StandbyMemory 
        { 
            get { return _standbyMemory; }
            set
            {
                _standbyMemory = value;
                OnPropertyChanged("StandbyMemory");
            }
        }
        public ulong FreeMemory 
        { 
            get { return _freeMemory; }
            set
            {
                _freeMemory = value;
                OnPropertyChanged("FreeMemory");
            }
        }
        public int ThreadSleepMilliseconds 
        { 
            get { return _threadSleepMilliseconds; }
            set
            {
                _threadSleepMilliseconds = value;
                OnPropertyChanged("ThreadSleepMilliseconds");
            }
        }
        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "StandbyMemory":
                        if (StandbyMemory <= 0 || StandbyMemory >= _memoryCounter.TotalSystemMemory)
                        {
                            error = $"Размер кеша должен быть больше 0 MB и меньше {_memoryCounter.TotalSystemMemory} MB";
                        }
                        break;
                    case "FreeMemory":
                        if (FreeMemory <= 0 || FreeMemory >= _memoryCounter.TotalSystemMemory)
                        {
                            error = $"Свободной памяти должно быть больше 0 MB и меньше  {_memoryCounter.TotalSystemMemory} MB";
                        }
                        break;
                    case "ThreadSleepMilliseconds":
                        if (ThreadSleepMilliseconds <= 300)
                        {
                            error = "Частота проверки условий должна быть больше 300 ms";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error => string.Empty;

        private ulong _standbyMemory;
        private ulong _freeMemory;
        private int _threadSleepMilliseconds;

        private readonly MemoryCounter _memoryCounter;
        public RegulationdDataModel()
        {
            _memoryCounter = new MemoryCounter();
        }
        
    }
           
}
