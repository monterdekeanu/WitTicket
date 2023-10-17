using WitTicket.View;
using WitTicket.View.Participant;
namespace WitTicket;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Registration), typeof(Registration));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(DashboardOrganizer), typeof(DashboardOrganizer));
        Routing.RegisterRoute(nameof(DashboardParticipant), typeof(DashboardParticipant));
        //ENVIRONMENT
        Environment.SetEnvironmentVariable("DATABASE_EVENT_IMAGES", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database\\Events\\Images"), EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("DATABASE_EVENTS", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database\\Events"), EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("DATABASE_USERS", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database\\User"),EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("DATABASE_USERS_TICKET", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database\\User\\Ticket"), EnvironmentVariableTarget.Process);
    }
}