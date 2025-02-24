namespace Basket.API.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasketAsync(string userName,CancellationToken cancellationToken);

    Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket,CancellationToken cancellation);
    
    Task<bool> DeleteBasketAsync(string userName ,CancellationToken cancellation);
}
