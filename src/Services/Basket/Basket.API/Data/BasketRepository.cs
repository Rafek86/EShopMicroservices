using Basket.API.Exceptions;

namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken)
    {
        var basket =await session.LoadAsync<ShoppingCart>(userName ,cancellationToken);
        return basket is null ? throw new BasketNotFoundException(userName) : basket;
    }


    public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellation)
    {
        session.Store(basket);
        await session.SaveChangesAsync(cancellation);
        return basket;  
    }


    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellation)
    {
        session.Delete<ShoppingCart>(userName);
        await session.SaveChangesAsync(cancellation);
        return true;
    }
}
