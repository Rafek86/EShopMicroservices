using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProducts;

public record GetProductQuery()
    :IQuery<GetProductResult>;

public record GetProductResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session,ILogger<GetProductsQueryHandler> logger) 
    :IQueryHandler<GetProductQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
       logger.LogInformation("GetProductsQueryHandler.Handle"); 

        var products = await session.Query<Product>().ToListAsync(cancellationToken);

        return new GetProductResult(products);
    }
}
