namespace WitTicket.View;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
	}
	
	private void OnClickHomeIcon(object sender, EventArgs e)
	{
        Shell.Current.GoToAsync("//MainPage");
    }
}