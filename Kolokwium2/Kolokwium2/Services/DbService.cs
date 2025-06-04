using Kolokwium2.data;
using Kolokwium2.DTOs;
using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<CustomerPurchasesDto?> GetCustomerPurchases(int customerId)
    {
        return await _context.Customers
            .Where(c => c.CustomerId == customerId)
            .Select(c => new CustomerPurchasesDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Purchases = c.PurchasedTickets.Select(pt => new PurchasedTicketDto
                {
                    Date = pt.PurchaseDate,
                    Price = pt.TicketConcert.Price,
                    Ticket = new TicketDto
                    {
                        SerialNumber = pt.TicketConcert.Ticket.SerialNumber,
                        SeatNumber = pt.TicketConcert.Ticket.SeatNumber
                    },
                    Concert = new ConcertDto
                    {
                        Name = pt.TicketConcert.Concert.Name,
                        Date = pt.TicketConcert.Concert.Date
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DoesConcertExist(string concertName)
    {
        return await _context.Concerts.AnyAsync(c => c.Name == concertName);
    }

    public async Task<int> AddCustomerIfNotExists(CustomerCreateDto dto)
    {
        var exists = await _context.Customers.AnyAsync(c => c.CustomerId == dto.CustomerId);
        if (exists)
            return dto.CustomerId;

        var customer = new Customer
        {
            CustomerId = dto.CustomerId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber
        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer.CustomerId;
    }

    public async Task AddPurchasedTickets(List<PurchaseCreateDto> purchases, int customerId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var purchase in purchases)
            {
                var concert = await _context.Concerts
                    .FirstAsync(c => c.Name == purchase.ConcertName);

                var ticket = new Ticket
                {
                    SerialNumber = Guid.NewGuid().ToString(),
                    SeatNumber = purchase.SeatNumber
                };
                await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();

                var ticketConcert = new TicketConcert
                {
                    TicketId = ticket.TicketId,
                    ConcertId = concert.ConcertId,
                    Price = purchase.Price
                };
                await _context.TicketConcerts.AddAsync(ticketConcert);
                await _context.SaveChangesAsync();

                var purchasedTicket = new PurchasedTicket
                {
                    TicketConcertId = ticketConcert.TicketConcertId,
                    CustomerId = customerId,
                    PurchaseDate = DateTime.Now
                };
                await _context.PurchasedTickets.AddAsync(purchasedTicket);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}