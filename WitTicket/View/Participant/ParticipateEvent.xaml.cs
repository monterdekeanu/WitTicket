using Microsoft.Maui.Storage;
using WitTicket.Model;

namespace WitTicket.View.Participant;

public partial class ParticipateEvent : ContentPage
{
    public EventModel ActiveEvent { get; set; }

    public ParticipateEvent(EventModel activeEvent)
	{
		InitializeComponent();
        ActiveEvent = activeEvent;
        BindingContext = this;
        InitializeImages();

    }

    private void InitializeImages()
    {
        if (ActiveEvent.Images.Count > 0)
        {


            foreach (string fileName in ActiveEvent.Images)
            {
                    Image image = new Image();
                    image.Source = ImageSource.FromFile($@"{Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), fileName)}");
                    image.WidthRequest = 400;
                    image.HeightRequest = 300;
                    image.Aspect = Aspect.AspectFill;
                    imgContainer.Children.Add(image);
            }
        }
        else
        {
            Image image = new Image();
            image.Source = ImageSource.FromFile("no_image.png");
            image.WidthRequest = 300;
            image.HeightRequest = 400;
            image.Aspect = Aspect.Fill;
            imgContainer.Children.Add(image);

        }
    }
}