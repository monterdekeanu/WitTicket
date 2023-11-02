using WitTicket.Model;

namespace WitTicket.View.Participant;

public partial class CheckoutView : ContentPage
{

	public CheckoutView(EventModel activeEvent,List<TicketCartItem> ticketCartItems, int participantId)
	{
		InitializeComponent();
	}
}