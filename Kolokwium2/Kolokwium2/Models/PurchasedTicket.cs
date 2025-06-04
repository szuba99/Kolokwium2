using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Models;


[Table("PurchasedTicket")]
[PrimaryKey("TicketConcertId", "CustomerId")]
public class PurchasedTicket
{
    [ForeignKey("TickerConcert")]
    public int TicketConcertId { get; set; }
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public DateTime PurchaseDate { get; set; }
   

    public TicketConcert TicketConcert { get; set; }
    public Customer Customer { get; set; }
}