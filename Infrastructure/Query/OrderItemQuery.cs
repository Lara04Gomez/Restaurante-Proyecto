using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.IOrderItem.Repository;


namespace Infrastructure.Query
{
    public class OrderItemQuery: IOrderItemQuery
    {
        private readonly AppDbContext _context;
    public OrderItemQuery(AppDbContext context)
    {
        _context = context;
    }

        public async Task<OrderItem?> GetOrderItemById(long id)
        {
            return await _context.OrderItems
                .Include(oi => oi.Dish)
                .Include(oi => oi.Status)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == id);
        }

        public async Task<List<OrderItem>> GetAllOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<bool> ExistsByDishId(Guid dishId)
        {
            return await _context.OrderItems.AnyAsync(oi => oi.DishId == dishId);
        }
    }
}
