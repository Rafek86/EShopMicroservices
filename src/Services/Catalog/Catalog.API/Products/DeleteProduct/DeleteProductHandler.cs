
namespace Catalog.API.Products.DeleteProduct;


public record DeleteProductCommand(Guid Id)
    : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool isSuccess);

public class DeleteProductHandler(IDocumentSession session, ILogger<DeleteProductHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand commmand, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductHandler.Handle called with {@Command}", commmand);

        var product = await session.LoadAsync<Product>(commmand.Id, cancellationToken);

        if (product == null)
        {
            logger.LogWarning("Product with ID {ProductId} not found.", commmand.Id);
            return new DeleteProductResult(false); // Return false if product doesn't exist
        }

        session.Delete<Product>(commmand.Id);
        await session.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}
