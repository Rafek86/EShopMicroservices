using Basket.API.Data;
using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto)
    :ICommand<CheckoutBaskerResult>;

public record CheckoutBaskerResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketDto is empty");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("username is requiered");
    }
}


public class CheckoutBasketHandler 
    (IBasketRepository repository,IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBaskerResult>
{
    public async Task<CheckoutBaskerResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        //get existing basket with totalprice 
        //Set total price on basketcheckout event message 
        //send checkout event message to message broker using masstransit
        //delete the basket

        var basket = await repository.GetBasketAsync(request.BasketCheckoutDto.UserName, cancellationToken);

        if (basket == null)
        {
            return new CheckoutBaskerResult(false);
        }
        var eventMessage =request.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await repository.DeleteBasketAsync(request.BasketCheckoutDto.UserName, cancellationToken);
        
        return new CheckoutBaskerResult(true);
    }
}

