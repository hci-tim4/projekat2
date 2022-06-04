using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace railway.clientTimetable
{
    public class DetailDrivinglineDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    
        public int StationScheduleId { get; set; }
        public string StationName { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DepartureTime { get; set; }
        private int _tour;

        public int Tour
        {
            get
            {
                return _tour;
            }
            set
            {
                if (_tour != value)
                {
                    _tour = value;
                    OnPropertyChanged("Tour");
                }
            }
        }
    }
}
