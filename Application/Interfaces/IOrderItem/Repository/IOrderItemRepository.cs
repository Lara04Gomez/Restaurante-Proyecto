using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.IOrderItem.Repository
{
    public interface IOrderItemRepository
    {
        
        Task<OrderItem?> GetOrderItemById(long id);
        Task<List<OrderItem>> GetAllOrderItems();
        Task<bool> ExistsByDishId(Guid dishId);

        Task InsertOrderItem(OrderItem orderItem);
        Task InsertOrderItemRange(List<OrderItem> orderItems);
        Task UpdateOrderItem(OrderItem orderItem);
        Task RemoveOrderItem(IEnumerable<OrderItem> orderItem);


    }
}
