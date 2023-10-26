namespace WitTicket.View.Participant;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using WitTicket.Model;
using WitTicket.View.Popups;
using static OpenAI.ObjectModels.SharedModels.IOpenAiModels;

public partial class DashboardParticipant : ContentPage
{
    private ObservableCollection<EventModel> events = (new Services.Connection()).GetEventsList();
    public UserModel ActiveUser { get; set; }
	public DashboardParticipant(UserModel activeUser)
    {
		InitializeComponent();
		ActiveUser = activeUser;
		BindingContext = this;
        InitializeEvents();

    }

    public void InitializeEvents()
    {
        flEventsContainer.Children.Clear();
        foreach (EventModel eventModel in events)
        {
            if (true)
            {
                VerticalStackLayout cardLayout = new VerticalStackLayout();
                cardLayout.HorizontalOptions = LayoutOptions.Center;
                ImageButton cardView = new ImageButton();
                cardView.Margin = new Thickness(10, 10, 10, 10);
                cardView.HeightRequest = 200;
                cardView.WidthRequest = 200;
                cardView.CornerRadius = 10;
                cardView.BackgroundColor = Color.FromHex("#F0F0F0");
                cardView.Clicked += async (sender, e) => { await Navigation.PushAsync(new ParticipateEvent(eventModel)); };
                if (eventModel.Images.Count > 0)
                {
                    cardView.Source = ImageSource.FromFile($@"{Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), eventModel.Images[0])}");
                }
                else
                {
                    cardView.Source = ImageSource.FromFile("dotnet_bot.png");
                }
                Label lblEventName = new();
                lblEventName.Text = FormatStringWithEllipsis(eventModel.Name);
                lblEventName.HorizontalTextAlignment = TextAlignment.Center;
                lblEventName.FontAttributes = FontAttributes.Bold;
                Label lblEventDate = new();
                lblEventDate.Text = eventModel.StartDate.ToShortDateString();
                lblEventDate.HorizontalTextAlignment = TextAlignment.Center;
                cardLayout.Children.Add(cardView);
                cardLayout.Children.Add(lblEventName);
                cardLayout.Children.Add(lblEventDate);
                flEventsContainer.Children.Add(cardLayout);
            }

            
        }


    }
    private static string FormatStringWithEllipsis(string input, int maxLength = 16)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        if (input.Length <= maxLength)
        {
            return input;
        }
        else
        {
            return input.Substring(0, maxLength) + "...";
        }
    }
}