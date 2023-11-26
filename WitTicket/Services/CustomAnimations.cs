using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitTicket.Services
{
    public class CustomAnimations
    {
        public CustomAnimations() { }
        public void OnPointerEnteredBtn(object sender, EventArgs e)
        {
            ((Button)sender).BackgroundColor = Color.FromHex("#F0F0F0");
        }
        public void OnPointerExitBtn(object sender, EventArgs e)
        {
            ((Button)sender).BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        }
    }
}
