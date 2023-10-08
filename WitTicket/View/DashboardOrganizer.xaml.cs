using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using WitTicket.Model;
using WitTicket.View.Organizer;
using WitTicket.View.Popups;
using WitTicket.ViewModel;

namespace WitTicket.View;

public partial class DashboardOrganizer : ContentPage
{
	private UserModel activeUser = new();
	private ObservableCollection<EventModel> events = (new Services.Connection()).GetEventsList();
	private DashboardViewModel dashboardViewModel;
    public DashboardOrganizer(UserModel activeUser)
	{
		InitializeComponent();
		this.activeUser = activeUser;
		dashboardViewModel = new DashboardViewModel(activeUser, events);
        BindingContext = dashboardViewModel;
		InitializeEvents();


    }

	public async void OnLogout(object sender, EventArgs e)
	{
        await Navigation.PopToRootAsync();
    }
	public async void OnClickCreateEvent(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new CreateEventPage(activeUser));
		dashboardViewModel.UpdateEvents((new Services.Connection()).GetEventsList());
    }
    private async void OnClickLogout(object sender, EventArgs e)
    {
        bool response = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        if(response)
        {
            await Navigation.PopToRootAsync();
        }
    }
	public void InitializeEvents()
	{
		cardTargetLayout.Children.Clear();
		foreach (EventModel eventModel in events)
		{
			if(eventModel.OrganizerId == activeUser.AccountId)
            {
                VerticalStackLayout cardLayout = new VerticalStackLayout();
                cardLayout.HorizontalOptions = LayoutOptions.Center;
                ImageButton cardView = new ImageButton();
                cardView.Margin = new Thickness(10, 10, 10, 10);
                cardView.HeightRequest = 200;
                cardView.WidthRequest = 200;
                cardView.CornerRadius = 10;
                cardView.BackgroundColor = Color.FromHex("#F0F0F0");
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
                cardTargetLayout.Children.Add(cardLayout);
            }
        }

	}
    public static string FormatStringWithEllipsis(string input, int maxLength = 16)
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