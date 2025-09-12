using Application.Enums;
using Application.Interfaces.IDish;
using Application.Models.Response;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class DishQuery : IDishQuery
    {
        private readonly AppDbContext _context;

        public DishQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dish>> GetAllAsync(string? name = null, int? categoryId = null, bool? onlyActive = true, OrderPrice? priceOrder = OrderPrice.ASC)
        {
            var query = _context.Dishes.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }

            if (categoryId >= 1 && categoryId <= 10)
            {
                query = query.Where(d => d.CategoryId == categoryId.Value);
            }

            switch (priceOrder)
            {
                case OrderPrice.ASC:
                    query = query.OrderBy(d => d.Price);
                    break;
                case OrderPrice.DESC:
                    query = query.OrderByDescending(d => d.Price);
                    break;
                default:
                    throw new InvalidOperationException("Valor de ordenamiento inválido");

            }
            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(d => d.Available);


            return await query
            .Include(d => d.Category)
            .ToListAsync();

        }

        public async Task<List<Dish>> GetAllDishes()
        {
            return await _context.Dishes.ToListAsync();
        }
        public async Task<Dish?> GetDishById(Guid id)
        {
            return await _context.Dishes.FindAsync(id).AsTask();
        }

        public async Task<bool> DishExists(string name, Guid? id)
        {
            var query = _context.Dishes.AsQueryable();

            if (id.HasValue)
            {

                query = query.Where(d => d.DishId != id.Value);
            }


            return await query.AnyAsync(d => d.Name == name);
        }
    }
}