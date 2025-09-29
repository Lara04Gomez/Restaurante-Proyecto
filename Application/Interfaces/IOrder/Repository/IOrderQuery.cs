using Domain.Entities;
using Application.Models.Response.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder.Repository
{
    public interface IOrderQuery
    {
        Task<Order?> GetOrderById(long id);
        Task<List<Order>> GetAllOrders();
        Task<IEnumerable<Order?>> GetOrderWithFilter( DateTime? from, DateTime? to, int? statusId);
    }
}
