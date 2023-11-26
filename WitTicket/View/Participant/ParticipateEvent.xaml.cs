using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WitTicket.Model;
using WitTicket.Services;

namespace WitTicket.View.Participant;

public partial class ParticipateEvent : ContentPage
{
    public EventModel ActiveEvent { get; set; }
    private int ParticipantID { get; set; }

    private List<TicketCartItem> TicketCartItems { get; set; } = new List<TicketCartItem>();
    public ParticipateEvent()
    {
        InitializeComponent();
    }
    public ParticipateEvent(EventModel activeEvent, int participantID)
	{
		InitializeComponent();
        ActiveEvent = activeEvent;
        ParticipantID = participantID;
        InitializeImages();
        BindingContext = this;
        InitializeCart();
        lblTitle.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => new NavigationController().OnClickHome(Navigation))
        });
        InitializeRemainingCapacity();
    }
    private void InitializeRemainingCapacity()
    {
        ObservableCollection<TicketModel> ticketList = (new Services.Connection()).GetTicketList();
        int remainingCapacity = ActiveEvent.TotalCapacity - ticketList.Where(x => x.EventId.Equals(ActiveEvent.EventId)).Count();
        lblRemainingCapacity.Text = $"Remaining Capacity: {remainingCapacity}";
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
    private async void OnClickProfileIcon(object sender, EventArgs e)
    {
        if (vslMenu.Opacity == 0)
        {
            vslMenu.Opacity = 1;
            try
            {
                foreach (Button btn in vslMenu.Children)
                {
                    btn.Opacity = 0;
                }
                foreach (Button btn in vslMenu.Children)
                {
                    await btn.FadeTo(1, 250, Easing.SinInOut);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        else if (vslMenu.Opacity == 1)
        {
            vslMenu.Opacity = 0;
            try
            {
                foreach (Button btn in vslMenu.Children)
                {
                    btn.Opacity = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
    private void OnPointerEnteredBtn(object sender, EventArgs e)
    {
        ((Button)sender).BackgroundColor = Color.FromHex("#DAA520");
        ((Button)sender).BorderColor = Color.FromHex("#DAA520");
    }
    private void OnPointerExitBtn(object sender, EventArgs e)
    {
        ((Button)sender).BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        ((Button)sender).BorderColor = Color.FromHex("#FFFFFF");
    }
    public async void OnClickLogout(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
    private async void OnClickViewTicketsBtn(object sender, EventArgs e)
    {
        
    }
}