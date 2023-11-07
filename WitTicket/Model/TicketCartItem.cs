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
        double _totalPrice;

        public TicketCartItem(EventClassModel eventClass, int quantity)
        {
            _eventClass = eventClass;
            _quantity = quantity;
        }
        public EventClassModel EventClass { get => _eventClass; }
        public int Quantity { get => _quantity; }
        public double TotalPrice { get => _totalPrice; set => _totalPrice = value; }
        public void UpdateQuantity(int quantity)
        {
            _quantity = quantity;
            TotalPrice = _eventClass.ClassPrice * _quantity;

        }

        

    }
}
