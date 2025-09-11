using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrderItem
{
    public interface IOrderItemQuery
    {
        Task<OrderItem?> GetOrderItemById(long id);
        Task<List<OrderItem>> GetAllOrderItems();
    }
}
