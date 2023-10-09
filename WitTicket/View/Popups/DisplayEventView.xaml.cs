using CommunityToolkit.Maui.Views;
using Windows.System;
using WitTicket.Model;
using WitTicket.ViewModel;

namespace WitTicket.View.Popups;

public partial class DisplayEventView : Popup
{
	UserModel ActiveUser {  get; set; }
	int EventId { get; set; }
	EventModel ActiveEvent { get; set; }
    private DisplayEventViewModel displayEventViewModel;
    public DisplayEventView(UserModel user, int eventId)
	{
		InitializeComponent();
        ActiveUser = user;
		EventId = eventId;
        ActiveEvent = (new Services.Connection()).GetEvent(eventId);
        displayEventViewModel = new DisplayEventViewModel(ActiveEvent);
        BindingContext = displayEventViewModel;

    }


}