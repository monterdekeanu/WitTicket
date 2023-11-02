using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitTicket.Model
{
    public class TicketCartItem
    {
        EventClassModel _eventClass;
        int _quantity;
        public TicketCartItem(EventClassModel eventClass, int quantity)
        {
            _eventClass = eventClass;
            _quantity = quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            _quantity = quantity;
        }
    }
}
