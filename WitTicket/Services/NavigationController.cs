using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitTicket.Model;
using WitTicket.View.Participant;

namespace WitTicket.Services
{
    public class NavigationController
    {
        public NavigationController() { }
        public async void OnClickHome(int um, INavigation navigation)
        {
            await navigation.PopAsync();
        }
        private UserModel FindUser(int um)
        {
            UserModel user = (new Services.Connection()).GetUsersList().FirstOrDefault(u => u.AccountId == um);
            return user;
        }
    }
}
