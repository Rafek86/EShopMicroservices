using Basket.API.Data;

namespace Basket.API.Basket.GetBasket;


public record GetBasketQuery(string UserName) 
    :IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart Cart);

public class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
     //TODO : Get basket from database 
     var basket =await repository.GetBasketAsync(query.UserName, cancellationToken);
        //var basket = await _repository.GetBasketAsync(query.UserName);
        return new GetBasketResult(basket);
    }
}
