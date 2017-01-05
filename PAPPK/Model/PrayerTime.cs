using System.ComponentModel;
using System.Diagnostics;

namespace PAPPK
{
    public class PrayerTime : INotifyPropertyChanged
    {
        private string _shubuh;
        private string _terbit;
        private string _dhuhur;
        private string _ashar;
        private string _maghrib;
        private string _isya;

        public string isha
        {
            get
            {
                return _isya;
            }

            set
            {
                _isya = value;
                OnPropertyChanged("isha");
            }
        }

        public string maghrib
        {
            get
            {
                return _maghrib;
            }

            set
            {
                _maghrib = value;
                OnPropertyChanged("maghrib");
            }
        }

        public string asr
        {
            get
            {
                return _ashar;
            }

            set
            {
                _ashar = value;
                OnPropertyChanged("asr");
            }
        }

        public string dhuhr
        {
            get
            {
                return _dhuhur;
            }

            set
            {
                _dhuhur = value;
                OnPropertyChanged("dhuhr");
            }
        }

        public string shurooq
        {
            get
            {
                return _terbit;
            }

            set
            {
                _terbit = value;
                OnPropertyChanged("shurooq");
            }
        }

        public string fajr
        {
            get
            {
                return _shubuh;
            }

            set
            {
                _shubuh = value;
                OnPropertyChanged("fajr");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
                Debug.WriteLine("halaoaoawowokwokawokaowk");
            }
            Debug.WriteLine(" maaf ya halaoaoawowokwokawokaowk");

        }
    }
}