namespace WitTicket.View.Participant;

using WitTicket.Services;
using WitTicket.Model;
using System.Diagnostics;

public partial class TicketView : ContentPage
{
    public UserModel ActiveUser { get; set; }
    public TicketView(UserModel activeUser)
	{
		InitializeComponent();
        ActiveUser = activeUser;
        BindingContext = this;
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