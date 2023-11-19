namespace WitTicket
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitializeDirectory();
            vslMain.FadeTo(1, 2000,Easing.SinInOut);
        }

        private void OnClickLogin(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new LoginPage());
            Shell.Current.GoToAsync("LoginPage",false);
        }

        private void OnClickSignUp(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new SignUpPage());
            Shell.Current.GoToAsync("Registration",false);
        }



        private void InitializeDirectory()
        {
            if (!Directory.Exists(Environment.GetEnvironmentVariable("DATABASE_USERS")))
            {
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("DATABASE_USERS", EnvironmentVariableTarget.Process));
            }
            if (!Directory.Exists(Environment.GetEnvironmentVariable("DATABASE_EVENTS")))
            {
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("DATABASE_EVENTS", EnvironmentVariableTarget.Process));
            }
            if (!Directory.Exists(Environment.GetEnvironmentVariable("DATABASE_USERS_TICKET")))
            {
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("DATABASE_USERS_TICKET", EnvironmentVariableTarget.Process));
            }
        }
    }
}