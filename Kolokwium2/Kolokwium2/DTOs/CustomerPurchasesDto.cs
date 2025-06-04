using Kolokwium2.Models;

namespace Kolokwium2.DTOs;

public class CustomerPurchasesDto
{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public List<PurchasedTicketDto> Purchases { get; set; }
    }

    public class PurchasedTicketDto
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public TicketDto Ticket { get; set; } 
        public ConcertDto Concert { get; set; }
    }

    public class TicketDto
    {
        public String SerialNumber { get; set; }
        public int SeatNumber { get; set; }
    }

    public class ConcertDto
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

