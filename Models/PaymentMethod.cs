public class PaymentMethod
{
    public const string Cash = "Cash";
    public const string CreditCard = "Credit Card";
    public const string OnlineTransfer = "Online Transfer";

    public static List<string> GetAllPaymentMethods()
    {
        return new List<string>
        {
            Cash,
            CreditCard,
            OnlineTransfer
        };
    }
}
