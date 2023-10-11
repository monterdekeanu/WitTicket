using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitTicket.Model;
using WitTicket.Services;

namespace WitTicket.ViewModel
{
    
    public class DisplayEventViewModel : PropertyChecker
    {
        public EventModel ActiveEvent { get; set; }
        public DateOnly EventEndDate { get; set; }
        public DateOnly EventStartDate { get; set; }
        public TimeOnly EventEndTime { get; set; }
        public TimeOnly EventStartTime { get; set; }

        public DisplayEventViewModel(EventModel activeEvent)
        {
            ActiveEvent = activeEvent;
            EventStartDate = DateOnly.FromDateTime(activeEvent.StartDate);
            EventStartTime = TimeOnly.FromDateTime(activeEvent.StartDate);
            EventEndDate = DateOnly.FromDateTime(activeEvent.EndDate);
            EventEndTime = TimeOnly.FromDateTime(activeEvent.EndDate);
            RaisePropertyChanged(nameof(ActiveEvent));
        }

        public void UpdateActiveEvent(EventModel newEvent)
        {
            ActiveEvent = newEvent;
            RaisePropertyChanged(nameof(ActiveEvent));
        }

    }
}
