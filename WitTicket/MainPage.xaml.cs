namespace WitTicket
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitializeDirectory();
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