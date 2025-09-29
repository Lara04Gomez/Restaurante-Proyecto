using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Response.Order;


namespace Application.Interfaces.IOrder
{
    public interface IGetOrderWithFilterUseCase
    {
        Task<IEnumerable<OrderDetailsResponse?>> GetOrderWithFilter( DateTime? from, DateTime? to, int? statusId);
    }
}
