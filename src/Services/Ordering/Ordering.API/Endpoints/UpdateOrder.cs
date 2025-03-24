using Ordering.Application.Orders.Commands.UpdateOrder;


namespace Ordering.API.Endpoints;


//- Accepts a Update Order request object 
//- Maps the Request to a Update Order Command 
//- send command for prosessing
//- return success or error response based on the outcome .

public record UpdateOrderRequest(OrderDto Order);

public record UpdateOrderResponse(bool IsSuccess);

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) => {

            var command = request.Adapt<UpdateOrderCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<UpdateOrderResponse>();

            return Results.Ok(response);
        })
            .WithName("UpdateOrder")
            .Produces<UpdateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Order")
            .WithDescription("Update Orders");
    }
}
