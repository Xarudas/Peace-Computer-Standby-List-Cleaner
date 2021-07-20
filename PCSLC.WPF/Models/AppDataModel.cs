using PСSLC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PСSLC.WPF.Models
{
    public class AppDataModel : PropertyChangedModelBase
    {
        public float UsedMemory
        {
            get
            {
                return _usedMemory;
            }
            set
            {
                _usedMemory = value;
                OnPropertyChanged("UsedMemory");
            }
        }
        public ulong TotalSystemMemory
        {
            get
            {
                return _totalSystemMemory;
            }
            set
            {
                _totalSystemMemory = value;
                OnPropertyChanged("TotalSystemMemory");
            }
        }
        public ulong FreeMemory 
        {
            get
            {
                return _freeMemory;
            }
            set
            {
                _freeMemory = value;
                OnPropertyChanged("FreeMemory");
            }
        }
        public ulong StandbyMemory 
        {
            get
            {
                return _standbyMemory;
            }
            set
            {
                _standbyMemory = value;
                OnPropertyChanged("StandbyMemory");
            }
        }
        public string ServiceInfo
        {
            get
            {
                return _serviceInfo;
            }
            set
            {
                _serviceInfo = value;
                OnPropertyChanged("ServiceInfo");
            }
        }

        private float _usedMemory;
        private ulong _totalSystemMemory;
        private ulong _freeMemory;
        private ulong _standbyMemory;
        private string _serviceInfo;
    }
}
