using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Response.Order;


namespace Application.Interfaces.IOrder
{
    public interface IUpdateOrderItemStatusUseCase
    {
        Task<OrderUpdateResponse> UpdateItemStatus(long orderId, int itemId, OrderItemUpdateRequest request);

    }
}
