using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.data;

public class DatabaseContext : DbContext
{
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Concert> Concerts { get; set; }
    public DbSet<PurchasedTicket> PurchasedTickets { get; set; }

    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<TicketConcert> TicketConcerts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}