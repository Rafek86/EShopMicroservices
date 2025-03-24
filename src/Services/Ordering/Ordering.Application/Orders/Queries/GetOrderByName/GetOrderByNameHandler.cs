using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrderByName
{
    public class GetOrderByNameHandler(IApplicationDbContext dbContext)
         : IQueryHandler<GetOrderByNameQuery, GetOrderByNameResult>
    {
        public async Task<GetOrderByNameResult> Handle(GetOrderByNameQuery query, CancellationToken cancellationToken)
        {
            //get order by name using dbcontext 
            //retun order list

            var orders =await dbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.OrderName.Value.Contains(query.Name))
                .OrderBy(o => o.OrderName.Value)    
                .ToListAsync(cancellationToken);

           var orderDtos = orders.ToOrderDtoList();

            return new GetOrderByNameResult(orderDtos); 
        }
    }
}
