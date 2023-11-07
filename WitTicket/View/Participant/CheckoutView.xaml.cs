using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using WitTicket.Model;

namespace WitTicket.View.Participant;

public partial class CheckoutView : ContentPage
{

	public ObservableCollection<TicketCartItem> CartItems { get; set; }
    public int ParticipantId { get; set; }
    public EventModel ActiveEvent { get; set; }
	public CheckoutView(EventModel activeEvent,List<TicketCartItem> ticketCartItems, int participantId)
	{
		InitializeComponent();
		ActiveEvent = activeEvent;
		ParticipantId = participantId;
        
        CartItems = ticketCartItems.ToObservableCollection<TicketCartItem>();
        for (int i = CartItems.Count - 1; i >= 0; i--)
        {
            if (CartItems[i].Quantity == 0)
            {
                CartItems.RemoveAt(i);
            }
        }
        ComputePrice();
        BindingContext = this;
    }
	public void ComputePrice()
	{
		double CartTotalPrice = 0;
        CartItems.ToList().ForEach(x => CartTotalPrice += (double)x.TotalPrice); // get total price
		lblSubtotal.Text = "$ " + Math.Round(CartTotalPrice,2).ToString("0.00");
		lblTotal.Text = "$ " + Math.Round(CartTotalPrice, 2).ToString("0.00");
    }
	public void OnClickEdit(object sender, EventArgs e)
	{
        Navigation.PopAsync();
    }
    private void OnClickHomeIcon(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }
    private void OnChangeCreditCardNumber(object sender, TextChangedEventArgs e)

    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            // Remove any non-numeric characters from the entered text
            var numericText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

            // Format the numeric text with spaces
            if (numericText.Length <= 16)
            {
                var formattedText = string.Join(" ", Enumerable.Range(0, (int)Math.Ceiling((double)numericText.Length / 4))
                    .Select(i => numericText.Substring(i * 4, Math.Min(4, numericText.Length - i * 4))));

                // Update the entry text with the formatted credit card number
                txtCardNumber.Text = formattedText;
            }
        }
        if (ValidateCreditCardNumber(e.NewTextValue))
        {
            lblCreditCardNumberError.IsVisible = false;
        }
        else
        {
            lblCreditCardNumberError.IsVisible = true;
        }
    }
    private void OnClickCheckout(object sender, EventArgs e)
    {
        this.ShowPopup(new CreditCardProcessorView());
    }
    private bool ValidateCreditCardNumber(string creditCardNumber)
    {
        // Remove any spaces or non-numeric characters from the input
        creditCardNumber = new string(creditCardNumber.Where(char.IsDigit).ToArray());

        if (string.IsNullOrWhiteSpace(creditCardNumber))
        {
            return false;
        }

        // Check if the credit card number is of a valid length
        if (creditCardNumber.Length < 13 || creditCardNumber.Length > 19)
        {
            return false;
        }

        // Perform Luhn's algorithm to validate the credit card number
        int sum = 0;
        bool doubleDigit = false;

        for (int i = creditCardNumber.Length - 1; i >= 0; i--)
        {
            int digit = creditCardNumber[i] - '0';

            if (doubleDigit)
            {
                digit *= 2;

                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            doubleDigit = !doubleDigit;
        }

        return sum % 10 == 0;
    }
 
}