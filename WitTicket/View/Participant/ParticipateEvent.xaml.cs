using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using WitTicket.Model;

namespace WitTicket.View.Participant;

public partial class ParticipateEvent : ContentPage
{
    public EventModel ActiveEvent { get; set; }
    private int ParticipantID { get; set; }

    private List<TicketCartItem> TicketCartItems { get; set; } = new List<TicketCartItem>();
    public ParticipateEvent(EventModel activeEvent, int participantID)
	{
		InitializeComponent();
        ActiveEvent = activeEvent;
        ParticipantID = participantID;
        InitializeImages();
        BindingContext = this;
        InitializeCart();
    }

    private void InitializeCart()
    {
        foreach (EventClassModel eventClass in ActiveEvent.EventClasses)
        {
            TicketCartItems.Add(new TicketCartItem(eventClass, 0));
        }
    }
    
    private void InitializeImages()
    {
        List<String> images = new List<String>();
        if (ActiveEvent.Images.Count > 0)
        {


            foreach (string fileName in ActiveEvent.Images)
            {
                    Image image = new Image();
                    image.Source = ImageSource.FromFile($@"{Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), fileName)}");
                    image.WidthRequest = 400;
                    image.HeightRequest = 300;
                    image.Aspect = Aspect.AspectFill;
                images.Add(Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), fileName));    
                //imgContainer.Children.Add(image);
            }
            ActiveEvent.Images = images;    
        }
        else
        {
            Image image = new Image();
            image.Source = ImageSource.FromFile("no_image.png");
            image.WidthRequest = 300;
            image.HeightRequest = 400;
            image.Aspect = Aspect.Fill;
            //imgContainer.Children.Add(image);

        }
        
    }
    private void OnClickTicketQuantityUpdate(object sender, EventArgs e)
    {
        double count = ((Stepper)sender).Value;
        try {
            TicketCartItems[Int32.Parse(((Stepper)sender).AutomationId)].UpdateQuantity((int)count);
        }
        catch(Exception ex) { 
        DisplayAlert("No Tickets Ordered", "You didn't select any ticket(s) yet. Select " + TicketCartItems.Count, "OK");
        }
        
        //DisplayAlert("Ticket Quantity", $"You have selected {count} {ActiveEvent.EventClasses[ticketType].ClassName} tickets", "OK");
    }

    private async void OnClickPurchase(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new CheckoutView(ActiveEvent, TicketCartItems, ParticipantID));
    }
}