using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Response;
using Application.Models.Request;

namespace Application.Interfaces.IOrderItem
{
    public interface IOrderItemService
    {
        
        Task<List<OrderItemResponse>> GetAllOrderItems();

        
        Task<OrderItemResponse> CreateOrderItem(OrderItemUpdateRequest orderItemRequest);

        
        Task<OrderItemResponse> UpdateOrderItem(int id, OrderItemUpdateRequest orderItemRequest);

        
        Task<OrderItemResponse> DeleteOrderItem(int id);

        
        Task<OrderItemResponse> GetOrderItemById(int id);
    }
}