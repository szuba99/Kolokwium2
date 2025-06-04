using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium2.Models;

[Table("TicketConcert")]
public class TicketConcert
{
    [Key]
    public int TicketConcertId { get; set; }
    
    [ForeignKey("Ticket")]
    public int TicketId { get; set; }
    [ForeignKey("Concert")]
    public int ConcertId { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    public ICollection<PurchasedTicket> PurchasedTickets { get; set; }

    public Ticket Ticket { get; set; }
    public Concert Concert { get; set; }
}