using Kolokwium2.DTOs;

namespace Kolokwium2.Services;

public interface IDbService
{
    Task<CustomerPurchasesDto?> GetCustomerPurchases(int customerId);
    Task<bool> DoesConcertExist(string concertName);
    Task<int> AddCustomerIfNotExists(CustomerCreateDto dto);
    Task AddPurchasedTickets(List<PurchaseCreateDto> purchases, int customerId);

}