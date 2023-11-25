namespace WitTicket.View.Participant;

using WitTicket.Services;
using WitTicket.Model;
using System.Diagnostics;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using QRCoder;

public partial class TicketView : ContentPage
{
    private ObservableCollection<EventModel> events = (new Services.Connection()).GetEventsList();
    private ObservableCollection<TicketModel> tickets = (new Services.Connection()).GetTicketList();
    private ObservableCollection<TicketModel> ActiveTickets { get; set; }
    public UserModel ActiveUser { get; set; }
    public TicketView(UserModel activeUser)
	{
		InitializeComponent();
        ActiveUser = activeUser;
        BindingContext = this;
        InitializeTickets();
        lblTitle.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => new NavigationController().OnClickHome(activeUser.AccountId, Navigation))
        });
        
	}

    private void InitializeTickets()
    {
        ActiveTickets =  tickets.Where(x => x.AccountId.Equals(ActiveUser.AccountId)).ToList().ToObservableCollection<TicketModel>();
        
        foreach(TicketModel ticket in ActiveTickets)
        {
            if (ticket == null)
            {
                ticketContainer.Children.Clear();
                break;
            }
            Frame ticketFrame = new();
            ticketFrame.Padding = 0;
            ticketFrame.CornerRadius = 0;
            ticketFrame.HeightRequest = 120;
            ticketFrame.Margin = new Thickness(0, 0,0,10);
            HorizontalStackLayout ticketStackLayout = new();
            ticketStackLayout.VerticalOptions = LayoutOptions.Center;
            Grid ticketGrid = new();
            ticketGrid.Padding = 20;
            ticketGrid.WidthRequest = 450;

            for(int i = 0; i < 4; i++) {
                ticketGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for(int i =0 ; i < 2; i++) {
                ticketGrid.RowDefinitions.Add(new RowDefinition());
            }
            ticketGrid.Add(new Label {Text = "Event Name",FontAttributes = FontAttributes.Bold},0,0);
            ticketGrid.Add(new Label { Text = events.FirstOrDefault(e => e.EventId == ticket.EventId).Name },1,0);
            ticketGrid.Add(new Label { Text = "Date", FontAttributes = FontAttributes.Bold }, 0,1);
            ticketGrid.Add(new Label { Text = events.FirstOrDefault(e => e.EventId == ticket.EventId).StartDate.ToShortDateString()}, 1, 1);
            ticketGrid.Add(new Label { Text = "Ticket #: ", FontAttributes = FontAttributes.Bold }, 2, 0);
            ticketGrid.Add(new Label { Text = ticket.TicketId.ToString() }, 3, 0);
            ticketGrid.Add(new Label { Text = "Ticket Type: ", FontAttributes = FontAttributes.Bold },2,1);
            ticketGrid.Add(new Label { Text = events.FirstOrDefault(e => e.EventId == ticket.EventId).EventClasses.FirstOrDefault(ec => ec.ClassId == ticket.TicketType).ClassName }, 3, 1);
            //ticketGrid.Add(new Label { Text = events.FirstOrDefault(e => e.EventId == ticket.EventId).EventClasses[0].ClassName }, 3, 1);
            Image currentTicketImage= new();
            currentTicketImage.Source = ImageSource.FromFile($@"{Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), events.FirstOrDefault(e => e.EventId == ticket.EventId).Images[0])}");
            //events.FirstOrDefault(e => e.EventId == ticket.EventId).Images[0];
            currentTicketImage.WidthRequest = 180;
            currentTicketImage.HeightRequest = 120;
            currentTicketImage.Aspect = Aspect.AspectFill;
            ticketFrame.Content = ticketStackLayout;
            ticketStackLayout.Add(ticketGrid);
            ticketStackLayout.Add(currentTicketImage);
            ticketStackLayout.Add(GenerateQRCode($"{ticket.AccountId}-{ticket.TicketId}-{ticket.EventId}", new Image() { WidthRequest = 120, HeightRequest = 110, Aspect = Aspect.AspectFit }));
            
            ticketContainer.Children.Add(ticketFrame);
        }
    }

    private Image GenerateQRCode(string InputText, Image QrCodeImage)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(InputText, QRCodeGenerator.ECCLevel.L);
        PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeBytes = qRCode.GetGraphic(20);
        QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
        return QrCodeImage;
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
        await Navigation.PushAsync(new TicketView(ActiveUser));
    }
}