using System.Collections.ObjectModel;
using WitTicket.Model;

namespace WitTicket.ViewModel;

public class DashboardViewModel : Services.PropertyChecker
{
    public UserModel ActiveUser { get; set; }
    public ObservableCollection<EventModel> Events { get; set; }
    public string FirstImage { get; set; }
    public DashboardViewModel(UserModel activeUser, ObservableCollection<EventModel> events)
    {
        ActiveUser = activeUser;
        Events = events;
        //FirstImage = Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES",EnvironmentVariableTarget.Process), events[0].Images[0]);
    }

    // Add methods or properties to update data in the ViewModel
    public void UpdateActiveUser(UserModel newUser)
    {
        ActiveUser = newUser;
        RaisePropertyChanged(nameof(ActiveUser));
    }

    public void UpdateEvents(ObservableCollection<EventModel> newEvents)
    {
        Events = newEvents;
        RaisePropertyChanged(nameof(Events));
    }
}
