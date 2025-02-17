
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdRequest(Guid Id);

public record GetProductByIdRespose(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid Id, ISender sender) => {
        
        var result =await sender.Send(new GetProductByIdQuery(Id));
            
            var response = result.Adapt<GetProductByIdRespose>(); 
            return Results.Ok(response);
        })
        .WithName("Get Product By Id")
        .Produces<GetProductByIdRespose>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");

    }
}
