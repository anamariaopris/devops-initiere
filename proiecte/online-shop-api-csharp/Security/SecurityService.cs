using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Data;

namespace OnlineShopApi.Security;

public class SecurityService(AppDbContext dbContext) : ISecurityService
{
    public async Task<bool> IsCustomerOwnerAsync(Guid customerId, string email, CancellationToken cancellationToken = default)
    {
        var ownerEmail = await dbContext.Customers.AsNoTracking()
            .Where(c => c.Id == customerId)
            .Select(c => c.User.Email)
            .FirstOrDefaultAsync(cancellationToken);
        return string.Equals(ownerEmail, email, StringComparison.Ordinal);
    }

    public async Task<bool> IsOrderOwnerAsync(Guid orderId, string email, CancellationToken cancellationToken = default)
    {
        var ownerEmail = await dbContext.Orders.AsNoTracking()
            .Where(o => o.Id == orderId && o.Customer != null)
            .Select(o => o.Customer!.User.Email)
            .FirstOrDefaultAsync(cancellationToken);
        return string.Equals(ownerEmail, email, StringComparison.Ordinal);
    }
}
