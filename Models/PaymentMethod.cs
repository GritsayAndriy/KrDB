public class PaymentMethod
{
    public const string Cash = "Cash";
    public const string CreditCard = "Credit Card";
    public const string OnlineTransfer = "Online Transfer";

    public const string CashKey = "cash";
    public const string CreditCardKey = "credit_card";
    public const string OnlineTransferKey = "online_transfer";
    public string Name { get; set; }
    public string Value { get; set; }

    public PaymentMethod (string Name, string Value) 
    {
        this.Name = Name;
        this.Value = Value;
    }

    public static List<PaymentMethod> GetAllPaymentMethods()
    {
        return new List<PaymentMethod>
        {
            new PaymentMethod(Cash, CashKey),
            new PaymentMethod(CreditCard, CreditCardKey),
            new PaymentMethod(OnlineTransfer, OnlineTransferKey)
        };
    }
}
