using WitTicket.View;
namespace WitTicket
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Registration), typeof(Registration));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }
    }
}