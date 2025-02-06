public class RentalCreateViewModel
{
    public int Id { get; set; }
    // Rental properties
    public int CustomerId { get; set; }
    public int InventoryId { get; set; }
    public int TariffId { get; set; }
    public string PaymentMethod { get; set; }

    // Flag for choosing existing customer or creating new one
    public bool IsNotExistingCustomer { get; set; }

    // New Customer properties
    public string? NewCustomerFullName { get; set; }
    public string? NewCustomerPhone { get; set; }
    public string? NewCustomerEmail { get; set; }
    public string? NewCustomerAddress { get; set; }
    public string? NewCustomerPassportNumber { get; set; }
}
