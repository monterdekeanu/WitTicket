using WitTicket.Model;

namespace WitTicket.View;

public partial class DashboardOrganizer : ContentPage
{
	public DashboardOrganizer(UserModel activeUser)
	{
		InitializeComponent();
		BindingContext = activeUser;
	}
}