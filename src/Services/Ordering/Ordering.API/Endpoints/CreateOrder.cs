﻿using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

//- Accepts a Create Order request object 
//- Maps the Request to a Create Order Command 
//- Use MediatR to send the command to the corresponding handler
//- Return a response with the created order's Id
public record CreateOrderRequest(OrderDto Order);

public record CreateOrderResponse(Guid Id);

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) => {
        var 
            command =request.Adapt<CreateOrderCommand>();
            
            var result =await sender.Send(command);
            
            var response = result.Adapt<CreateOrderResponse>();

           return Results.Created($"/orders/{response.Id}", response);

        })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Order")
            .WithDescription("Create Orders");
    }
}
