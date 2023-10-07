namespace WitTicket
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnClickLogin(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new LoginPage());
            Shell.Current.GoToAsync("LoginPage");
        }

        private void OnClickSignUp(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new SignUpPage());
            Shell.Current.GoToAsync("Registration");
        }
    }
}