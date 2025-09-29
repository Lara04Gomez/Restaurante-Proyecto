using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Request;
using Application.Models.Response.Order;

namespace Application.Interfaces.IOrder
{
    public interface IUpdateItemFromOrderUseCase
    {
        public Task<OrderUpdateResponse> UpdateItemQuantity(long orderId, OrderUpdateRequest listItems);
    }
}
