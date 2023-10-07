using System.Collections.ObjectModel;
using System.ComponentModel;
using WitTicket.Model;

namespace WitTicket.View;

public partial class Registration : ContentPage
{

    public UserModel user = new UserModel();
	public Registration()
	{
		InitializeComponent();
        user.AccountType = "Participant";
        dpDateBirthdate.Date = DateTime.Today;
        dpDateBirthdate.MaximumDate = dpDateBirthdate.Date;
    }
	
	private void OnClickHomeIcon(object sender, EventArgs e)
	{
        Shell.Current.GoToAsync("//MainPage");
        
    }

	private void OnClickRegister(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtConfirmPassword.Text) || string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text))
        {
            DisplayAlert("Error", "Please fill in all the fields", "OK");
        }
        else
        { 
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                DisplayAlert("Error", "Passwords do not match", "OK");
            }
            else
            {
                user.Email = txtEmail.Text;
                user.Password = txtPassword.Text;
                user.FirstName = txtFirstName.Text;
                user.LastName = txtLastName.Text;
                user.DateOfBirth = DateOnly.FromDateTime(dpDateBirthdate.Date) ;
                ObservableCollection<UserModel> users = (new Services.Connection()).GetUsersList();
                foreach(UserModel user in users)
                {
                    if (user.Email == txtEmail.Text)
                    {
                        DisplayAlert("Error", "Email is already taken", "OK");
                        return;
                    }
                }
                if(users.Count > 0)
                {
                    user.AccountId = users.Last().AccountId + 1;
                }
                else
                {
                    user.AccountId = 0;
                }
                users.Add(user);
                (new Services.Connection()).SaveUsersList(users);
                DisplayAlert("Success", "Account created successfully", "OK");  
                Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
    private void OnClickUpdateAcountType(object sender, EventArgs e)
    {
        user.AccountType = (string)((RadioButton)sender).Value;
    }

}