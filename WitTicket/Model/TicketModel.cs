using WitTicket.Services;

namespace WitTicket.Model;

public class TicketModel : PropertyChecker
{	
	private int _ticketId;
	private int _eventId;
	private int _accountId;
	private int _ticketType; 

	public int TicketId { get => _ticketId; set{
            _ticketId = value;
			RaisePropertyChanged(nameof(TicketId));
        } }
	public int EventId { get => _eventId; set
		{
            _eventId = value;
            RaisePropertyChanged(nameof(EventId));
        } }
	public int AccountId { get => _accountId; set
		{
            _accountId = value;
            RaisePropertyChanged(nameof(AccountId));
        } }
	public int TicketType { get => _ticketType; set
		{
            _ticketType = value;
            RaisePropertyChanged(nameof(TicketType));
        } }

	public TicketModel(int ticketId, int eventId, int accountId, int ticketType)
	{
        TicketId = ticketId;
        EventId = eventId;
        AccountId = accountId;
        TicketType = ticketType;
    }
}