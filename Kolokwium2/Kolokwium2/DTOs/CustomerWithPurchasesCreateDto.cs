namespace Kolokwium2.DTOs;

public class CustomerWithPurchasesCreateDto
{
    public CustomerCreateDto Customer { get; set; }
    public List<PurchaseCreateDto> Purchases { get; set; }
}

public class CustomerCreateDto
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
}

public class PurchaseCreateDto
{
    public int SeatNumber { get; set; }
    public string ConcertName { get; set; }
    public decimal Price { get; set; }
}