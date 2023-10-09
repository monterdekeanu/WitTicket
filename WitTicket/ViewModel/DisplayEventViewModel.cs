using System;
using System.Collections.Generic;
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

        public DisplayEventViewModel(EventModel activeEvent)
        {
            ActiveEvent = activeEvent;
            RaisePropertyChanged(nameof(ActiveEvent));
        }

        public void UpdateActiveEvent(EventModel newEvent)
        {
            ActiveEvent = newEvent;
            RaisePropertyChanged(nameof(ActiveEvent));
        }

    }
}
