using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.IOrder.Repository
{
    public interface IOrderRepository
    {
        
        Task<Order?> GetOrderById(long id);
        Task<IEnumerable<Order?>> GetOrderWithFilter( DateTime? from, DateTime? to, int? statusId);
        Task<List<Order>> GetAllOrders();
        
        Task InsertOrder(Order order);
        Task UpdateOrder(Order order);
        Task RemoveOrder(Order order);
    }
}
