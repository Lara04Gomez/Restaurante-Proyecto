using Application.Models.Request;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrderItem
{
    public interface IOrderItemService
    {
       
        Task<List<OrderItemResponse>> GetAllOrderItems();

        Task<OrderItemResponse> CreateOrderItem(OrderItemRequest orderItemRequest);

       
        Task<OrderItemResponse> UpdateOrderItem(int id, OrderItemRequest orderItemRequest);

       
        Task<OrderItemResponse> DeleteOrderItem(int id);

        
        Task<OrderItemResponse> GetOrderItemById(int id);
    }
}

