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
        InitializeImages();
        BindingContext = displayEventViewModel;

    }

    private void InitializeImages()
    {
        if (ActiveEvent.Images.Count > 0)
        {

            imgHighlight.Source = ImageSource.FromFile($@"{Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), ActiveEvent.Images[0])}");
            if (ActiveEvent.Images.Count > 1)
            {
                foreach(string fileName in ActiveEvent.Images)
                {
                    if(fileName != ActiveEvent.Images[0])
                    {
                        Image image = new Image();
                        image.Source = ImageSource.FromFile($@"{Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), fileName)}");
                        image.WidthRequest = 300;
                        image.HeightRequest = 169;
                        image.Aspect = Aspect.Fill;
                        svImageContainer.Children.Add(image);
                    }
                }
            }
            else
            {
                imgHighlight.HeightRequest = 450;
            }
        }
        else
        {
            imgHighlight.Source = ImageSource.FromFile("no_image.png");

        }
    }

}