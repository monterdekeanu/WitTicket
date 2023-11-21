namespace WitTicket.View.Participant;

using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using WitTicket.Model;
using WitTicket.Services;
using WitTicket.View.Popups;
using static OpenAI.ObjectModels.SharedModels.IOpenAiModels;

public partial class DashboardParticipant : ContentPage
{
    private ObservableCollection<EventModel> events = (new Services.Connection()).GetEventsList();
    public ObservableCollection<EventModel> searchEvents { get; set; }
    public UserModel ActiveUser { get; set; }
	public DashboardParticipant(UserModel activeUser)
    {
		InitializeComponent();
		ActiveUser = activeUser;
		BindingContext = this;
        InitializeEvents(events);

    }

    public async void InitializeEvents(ObservableCollection<EventModel> obsCollectionEvents)
    {
        flEventsContainer.Children.Clear();
        foreach (EventModel eventModel in obsCollectionEvents)
        {
            if (true)
            {
                VerticalStackLayout cardLayout = new VerticalStackLayout();
                cardLayout.HorizontalOptions = LayoutOptions.Center;
                ImageButton cardView = new ImageButton();
                cardView.Margin = new Thickness(10, 10, 10, 10);
                cardView.HeightRequest = 250;
                cardView.WidthRequest = 150;
                cardView.CornerRadius = 5;
                cardView.BackgroundColor = Color.FromHex("#F0F0F0");
                cardView.Clicked += async (sender, e) => { await Navigation.PushAsync(new ParticipateEvent(eventModel,ActiveUser.AccountId)); };
                if (eventModel.Images.Count > 0)
                {
                    Debug.Write("Image path: " + $@"{Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), eventModel.Images[0])}");
                    Debug.WriteLine(Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), eventModel.Images[0]));
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
                cardLayout.Opacity = 0;
                
                flEventsContainer.Children.Add(cardLayout);
            }

            
        }
        try {
            foreach (VerticalStackLayout cardLayout in flEventsContainer.Children)
            {
                await cardLayout.FadeTo(1, 250,Easing.SinInOut);
            }
        }catch(Exception e)
        {
            Debug.WriteLine(e.Message);
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
    private void OnSearchBarTextChange(object sender, EventArgs e)
    {
        if(((Entry)sender).Text == null || ((Entry)sender).Text == "")
        {
            searchEvents = null;
            OnPropertyChanged(nameof(searchEvents));
            InitializeEvents(events);
            return;
        }
        
        searchEvents = events.Where(x => x.Name.ToLower().Contains(((Entry)sender).Text.ToLower())).ToObservableCollection<EventModel>();
        OnPropertyChanged(nameof(searchEvents));
        InitializeEvents(searchEvents);
    }
    private async void OnClickSearch(object sender, EventArgs e)
    {
        Debug.WriteLine(((Frame)sender).AutomationId);
        int index = Int32.Parse(((Frame)sender).AutomationId);
        await Navigation.PushAsync(new ParticipateEvent(events[index], ActiveUser.AccountId));
    }

    private async void OnClickProfileIcon(object sender, EventArgs e)
    {
        if(vslMenu.Opacity == 0)
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
        }else if(vslMenu.Opacity == 1)
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
        await Navigation.PushAsync(new TicketView(ActiveUser));
    }
}