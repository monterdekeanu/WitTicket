using WitTicket.Services;

namespace WitTicket.Model;

public class EventModel : PropertyChecker
{
        private int _eventId;
        public int EventId { get => _eventId; set { _eventId = value; RaisePropertyChanged(nameof(EventId)); } }           // Unique identifier for the event
        public string Name { get; set; }           // Event name
        public string City { get; set; }           // City where the event is held
        public string Street { get; set; }
        public DateTime StartDate { get; set; }    // Date and time when the event starts
        public DateTime EndDate { get; set; }      // Date and time when the event ends
        public decimal Price { get; set; }         // Ticket price
        public int TotalCapacity { get; set; }     // Total capacity of the event
        public List<string> Images { get; set; }    // List of image URLs
        public List<EventClassModel> EventClasses { get; set; } // Classes Available for the event
        public int TotalAttendees { get; set; }     // Total number of attendees
        public bool IsCancelled { get; set; }      // Flag to indicate if the event is cancelled
        public string Organizer { get; set; }       // Name of the event organizer
        public int OrganizerId { get; set; }       // ID of the event organizer
        public string Description { get; set; }     // Description of the event
        public List<string> TicketIDList { get; }   // List of attendee names // Maybe change to ParticipantID
        //GO GO GO GAGAGA HUHUH MAMA
        public EventModel(List<EventClassModel> eventClasses, int eventId, string name, string city, string street, DateTime startDate, DateTime endDate, decimal price, int totalCapacity, string organizer, int organizerId, string description)
        {
            EventClasses = eventClasses;
            OrganizerId = organizerId;
            EventId = eventId;
            Name = name;
            City = city;
            StartDate = startDate;
            EndDate = endDate;
            Price = price;
            TotalCapacity = totalCapacity;
            Images = new List<string>();
            TotalAttendees = 0;
            IsCancelled = false;
            Organizer = organizer;
            Description = description;
            TicketIDList = new List<string>();
            Street = street;
        }

        // Method to add an image URL to the event
        public void AddImage(string imageUrl)
        {
            Images.Add(imageUrl);
            
        }

        // Method to register an attendee for the event
        public bool RegisterAttendee(string attendeeName)
        {
            if (!IsCancelled && TotalAttendees < TotalCapacity)
            {
                TotalAttendees++;
                TicketIDList.Add(attendeeName);
                return true; // Registration successful
            }
            else
            {
                return false; // Event is at full capacity or cancelled
            }
        }

        // Method to cancel the event
        public void CancelEvent()
        {
            IsCancelled = true;
        }
}