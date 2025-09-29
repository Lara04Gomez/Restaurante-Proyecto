using Application.Enums;
using Application.Interfaces.ICategory.Repository;
using Application.Interfaces.IDish.Repository;
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
        private readonly ICategoryQuery _categoryQuery;


        public DishQuery(AppDbContext context, ICategoryQuery categoryQuery)
        {
            _context = context;
            _categoryQuery = categoryQuery;
        }

        public async Task<IEnumerable<Dish>> GetAllAsync(string? name = null, int? categoryId = 0, OrderPrice? priceOrder = OrderPrice.ASC, bool? onlyActive = null)
        {
            var query = _context.Dishes.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => EF.Functions.Like(d.Name.ToLower(), $"%{name.ToLower()}%"));
            }


            if (categoryId.HasValue && categoryId.Value != 0)
            {
                var categoryExists = await _categoryQuery.CategoryExistsAsync(categoryId.Value);
                if (!categoryExists)
                {
                    throw new KeyNotFoundException($"La categoría con ID {categoryId.Value} no existe.");
                }
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

        public async Task<Dish?> GetDishById(Guid id)
        {
            return await _context.Dishes
            .Include(d => d.Category) 
            .FirstOrDefaultAsync(d => d.DishId == id);
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