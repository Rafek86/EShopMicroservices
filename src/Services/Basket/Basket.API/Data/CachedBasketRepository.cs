using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository,IDistributedCache cache) 
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
         return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var basket = await repository.GetBasketAsync(userName, cancellationToken);

        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellation)
    {
        
        await repository.UpdateBasketAsync(basket, cancellation);
      
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellation);

        return basket;
    }


    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellation)
    {
        await repository.DeleteBasketAsync(userName, cancellation);

        await cache.RemoveAsync(userName, cancellation);

        return true;

    }

}
