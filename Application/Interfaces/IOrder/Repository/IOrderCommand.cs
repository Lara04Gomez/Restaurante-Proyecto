using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder.Repository
{
    public interface IOrderCommand
    {
        Task InsertOrder(Order order);
        Task UpdateOrder(Order order);
        Task RemoveOrder(Order order);
    }
}
