﻿namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Category, string Description, decimal Price, int Stock, string ImageFile, int CategoryId);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
            async(CreateProductRequest request,ISender sender) => {
            
                var command = request.Adapt<CreateProductCommand>();
               
                var result =await sender.Send(command);

                var respose =result.Adapt<CreateProductResponse>(); 

                return Results.Created($"/products/{respose.Id}", respose);
            })
        .WithName("Create Product")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)   
        .WithSummary("Create Product")
        .WithDescription("Create Product");
        
    }
}
