using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete;

public class OrderProductRepository : BaseRepistory<OrderProduct>, IOrderProductRepository
{
    public OrderProductRepository(AppDbContext context) : base(context)
    {
        
    }
}
