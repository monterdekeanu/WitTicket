namespace WitTicket.View;

using System.Collections.ObjectModel;
using WitTicket.Model;
public partial class LoginPage : ContentPage
{
    public UserModel tmpUser = new UserModel();
    public LoginPage()
	{
		InitializeComponent();
		tmpUser.AccountType = "Participant";//DEFAULT
	}
	//code me login methods	//code me login methods
	private async void OnClickLogin(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
		{
			await DisplayAlert("Error", "Please fill in all the fields", "OK");
		}
		else
		{
			ObservableCollection<UserModel> users = (new Services.Connection()).GetUsersList();
			foreach (UserModel user in users)
			{
				if ((user.Email.ToLower() == txtEmail.Text.ToLower()) && (user.Password == txtPassword.Text) && (user.AccountType == tmpUser.AccountType))
				{
					await DisplayAlert("Success", "Login successful", "OK");
					await Navigation.PushAsync(new DashboardOrganizer(user));
					return;
				}
			}
			await DisplayAlert("Error", "Email or password is incorrect", "OK");
		}
	}
	private void OnClickRegister(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//Registration");
	}
    private void OnClickUpdateType(object sender, EventArgs e)
    {
        tmpUser.AccountType = (string)((RadioButton)sender).Value;
    }
}