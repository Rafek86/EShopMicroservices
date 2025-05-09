﻿using BuildingBlocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
   public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid orderId)
            : base($"Order with id {orderId} was not found")
        {
        }
    }
}
