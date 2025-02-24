
using Basket.API.Data;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName)
    :ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is Required");
    }
}

public class DeleteBasketHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        //TODO : delete basket from database & redis
        await repository.DeleteBasketAsync(command.UserName, cancellationToken);
        return new DeleteBasketResult(true);
    }
}
