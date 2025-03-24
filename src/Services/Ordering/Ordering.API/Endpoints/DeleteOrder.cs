
using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints;

//- Accepts a Delete Delete Order request object 
//- Maps the Request to a Delete Order Command 
//- Use MediatR to send the command to the corresponding handler
//- return success or not found response 

//public record DeleteOrderRequest(Guid Id);

public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
     app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
     {
         var command = new DeleteOrderCommand(id);
         var result = await sender.Send(command);
     
         var resopnse = result.Adapt<DeleteOrderResponse>();

         return Results.Ok(resopnse);
     })
         .WithName("DeleteOrder")
         .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithSummary("Delete Order")
         .WithDescription("Delete Orders");
    }
}
