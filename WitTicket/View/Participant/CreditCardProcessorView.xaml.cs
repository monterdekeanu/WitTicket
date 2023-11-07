using CommunityToolkit.Maui.Views;

namespace WitTicket.View.Participant;

public partial class CreditCardProcessorView : Popup
{
	public CreditCardProcessorView()
	{
		InitializeComponent();
        ConfirmTransaction();
    }

    private async void ConfirmTransaction()
    {
        await HideActivityIndicatorAfterDelay();
    }
    private async Task HideActivityIndicatorAfterDelay()
    {
        await Task.Delay(4000); // 4 seconds
        lblNotice.Text = "Transaction Successful";
        HideActivityIndicator();
        await CloseTimer();
        this.Close();
    }
    private void HideActivityIndicator()
    {
        loadingIndicator.IsRunning = false;
        loadingIndicator.IsVisible = false;
    }
    private async Task CloseTimer()
    {
        lblTimer.IsVisible = true;
        lblMerchantNotice.IsVisible = true;
        for (int i = 0; i<4; i++)
        {
            lblTimer.Text = (4 - i).ToString();
            await Task.Delay(1000);
            
        }
    }

}