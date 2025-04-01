﻿using Core.Entities;
using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract;

public interface IOrderRepository : IBaseRepistory<Order>
{
	Task<Order> GetOrderWithOrderProductsAsync(Guid token, string userId);
}
