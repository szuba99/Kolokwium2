using Kolokwium2.DTOs;
using Kolokwium2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium2.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly IDbService _dbService;

    public CustomersController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpGet("{id}/purchases")]
    public async Task<IActionResult> GetCustomerPurchases(int id)
    {
        var result = await _dbService.GetCustomerPurchases(id);

        if (result is null)
            return NotFound($"Customer with id {id} was not found");

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCustomerWithPurchases([FromBody] CustomerWithPurchasesCreateDto dto)
    {
        // Walidacja: max 5 biletów na jeden koncert
        var grouped = dto.Purchases.GroupBy(p => p.ConcertName);
        foreach (var group in grouped)
        {
            if (group.Count() > 5)
            {
                return BadRequest($"Customer cannot buy more than 5 tickets for one concert");
            }
        }

        // czy koncerty istnieją
        foreach (var p in dto.Purchases)
        {
            if (!await _dbService.DoesConcertExist(p.ConcertName))
            {
                return NotFound($"Concert '{p.ConcertName}' does not exist");
            }
        }

        // Dodanie klienta jeśli nie istnieje
        var customerId = await _dbService.AddCustomerIfNotExists(dto.Customer);

        // Dodanie zakupu 
        await _dbService.AddPurchasedTickets(dto.Purchases, customerId);

        return Created("", dto);
    }

}