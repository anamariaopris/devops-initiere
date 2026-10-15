namespace OnlineShopApi.Security;

public interface ISecurityService
{
    Task<bool> IsCustomerOwnerAsync(Guid customerId, string email, CancellationToken cancellationToken = default);
    Task<bool> IsOrderOwnerAsync(Guid orderId, string email, CancellationToken cancellationToken = default);
}
