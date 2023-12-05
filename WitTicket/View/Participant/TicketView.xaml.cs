namespace WitTicket.View.Participant;

using WitTicket.Services;
using WitTicket.Model;
using System.Diagnostics;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using QRCoder;
using Microsoft.Maui.Controls.Internals;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;

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
            Command = new Command(() => new NavigationController().OnClickHome(Navigation))
        });
        
	}
    private async void OnClickDownloadImage(object sender, EventArgs e)
    {
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;
        HorizontalStackLayout parent = (HorizontalStackLayout)((Button)sender).Parent;
        ((Button)sender).IsVisible = false;
        var image = await parent.CaptureAsync();

        using MemoryStream memoryStream = new MemoryStream();

        await image.CopyToAsync(memoryStream);

        //File.WriteAllBytes($"C:\\Users\\user\\Desktop\\{ActiveUser.FirstName}_{((Button)sender).AutomationId}.png", memoryStream.ToArray());
        var fileSaverResult = await FileSaver.Default.SaveAsync($"{ActiveUser.FirstName}_{((Button)sender).AutomationId}.png", memoryStream, token);
        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make("Ticket Saved", ToastDuration.Short, 14).Show();
        }
        ((Button)sender).IsVisible = true;
        
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
            //if(events.FirstOrDefault(x => x.EventId == ticket.EventId).IsCancelled || events.FirstOrDefault(x => x.EventId == ticket.EventId).EventClasses.FirstOrDefault(x => x.ClassId == ticket.TicketType).IsDeleted)
            //{
            //   break;
            //}
            Grid gridParentContainer = new();
            Frame ticketFrame = new();
            ticketFrame.Padding = 0;
            ticketFrame.CornerRadius = 0;
            ticketFrame.HeightRequest = 120;
            ticketFrame.Margin = new Thickness(0, 0,0,10);
            HorizontalStackLayout ticketStackLayout = new();
            ticketStackLayout.VerticalOptions = LayoutOptions.Center;
            Grid ticketGrid = new();
            ticketGrid.BackgroundColor = Color.FromArgb("#99001f3f");
            ticketGrid.Padding = 20;
            ticketGrid.WidthRequest = 450;

            for(int i = 0; i < 4; i++) {
                ticketGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for(int i =0 ; i < 3; i++) {
                ticketGrid.RowDefinitions.Add(new RowDefinition());
            }
            Label lblEventTitle = new Label { TextColor= Color.FromHex("#ecf0f1"), Text = events.FirstOrDefault(e => e.EventId == ticket.EventId).Name, TextTransform = TextTransform.Uppercase, FontSize= 18, FontAttributes = FontAttributes.Bold };
            ticketGrid.Add(lblEventTitle,0,0);
            ticketGrid.SetColumnSpan(lblEventTitle, 4);
            ticketGrid.Add(new Label { TextColor = Color.FromHex("#ecf0f1"), Text = "Date", FontAttributes = FontAttributes.Bold }, 2,2);
            ticketGrid.Add(new Label { TextColor = Color.FromHex("#ecf0f1"), Text = events.FirstOrDefault(e => e.EventId == ticket.EventId).StartDate.ToShortDateString()}, 3, 2);
            ticketGrid.Add(new Label { TextColor = Color.FromHex("#ecf0f1"), Text = "Ticket #: ", FontAttributes = FontAttributes.Bold }, 0, 2);
            ticketGrid.Add(new Label { TextColor = Color.FromHex("#ecf0f1"), Text = ticket.TicketId.ToString() }, 1, 2);
            ticketGrid.Add(new Label { TextColor = Color.FromHex("#ecf0f1"), Text = "Ticket Type: ", FontAttributes = FontAttributes.Bold },2,1);
            ticketGrid.Add(new Label { TextColor = Color.FromHex("#ecf0f1"), Text = events.FirstOrDefault(e => e.EventId == ticket.EventId).EventClasses.FirstOrDefault(ec => ec.ClassId == ticket.TicketType).ClassName, FontSize = 12 }, 3, 1);
            //ticketGrid.Add(new Label { Text = events.FirstOrDefault(e => e.EventId == ticket.EventId).EventClasses[0].ClassName }, 3, 1);
            Image currentTicketImage= new();
            currentTicketImage.Source = ImageSource.FromFile($@"{Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES", EnvironmentVariableTarget.Process), events.FirstOrDefault(e => e.EventId == ticket.EventId).Images[0])}");
            //events.FirstOrDefault(e => e.EventId == ticket.EventId).Images[0];
            //currentTicketImage.WidthRequest = 180;
            //currentTicketImage.HeightRequest = 120;
            currentTicketImage.Aspect = Aspect.AspectFill;
            ticketFrame.Content = ticketStackLayout;
            gridParentContainer.Add(currentTicketImage,0,0);
            gridParentContainer.Add(ticketGrid, 0, 0);

            ticketStackLayout.Add(gridParentContainer);
            //ticketStackLayout.Add(currentTicketImage);
            ticketStackLayout.Add(GenerateQRCode($"{ticket.AccountId}-{ticket.TicketId}-{ticket.EventId}", new Image() { WidthRequest = 120, HeightRequest = 110, Aspect = Aspect.AspectFit }));
            Button saveTicketBtn = new();
            saveTicketBtn.Text = "Save Ticket";
            saveTicketBtn.AutomationId = ticket.TicketId.ToString();
            saveTicketBtn.CornerRadius = 0;
            saveTicketBtn.BackgroundColor = Color.FromHex("#DAA520");
            saveTicketBtn.Clicked += OnClickDownloadImage;
            ticketStackLayout.BackgroundColor = Color.FromHex("#ecf0f1");
            ticketStackLayout.Add(saveTicketBtn);
            
            ticketContainer.Children.Add(ticketFrame);
        }
    }

    private void SaveTicketBtn_Clicked(object sender, EventArgs e)
    {
        throw new NotImplementedException();
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