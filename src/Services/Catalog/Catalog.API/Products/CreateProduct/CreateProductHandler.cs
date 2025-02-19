namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name,List<string> Category ,string Description, decimal Price, int Stock,string ImageFile ,int CategoryId)
    :ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand> {

    public CreateProductCommandValidator() { 
    
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
    }
}

internal class CreateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //Business logic to create a product
        //1- create a product entity from command object 
        //2- save the product entity to the database
        //3- return the product Result (ID here)

        var product = new Product { 
            Name = command.Name,
            Categories = command.Category,
            ImageFile =command.ImageFile,
            Description = command.Description,
            Price = command.Price
        };

        //TODO : Save the product to the database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
