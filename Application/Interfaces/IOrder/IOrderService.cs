using Application.Models.Request;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder
{
    public interface IOrderService
    {
        
        Task<List<OrderResponse>> GetAllOrders();

        
        Task<OrderResponse> CreateOrder(OrderRequest orderRequest);

       
        Task<OrderResponse> UpdateOrder(int id, OrderRequest orderRequest);

        
        Task<OrderResponse> DeleteOrder(int id);

        
        Task<OrderResponse> GetOrderById(int id);
    }
}
