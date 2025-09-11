using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDish
{
    public interface IDishQuery
    {
        Task<Dish?> GetDishById(Guid id);
        Task<bool> DishExists(string name);
        Task<IEnumerable<Dish>> GetAllAsync(string? name = null, int? categoryId = null, bool? onlyActive = true, OrderPrice? priceOrder = OrderPrice.ASC);
        Task<List<Dish>> GetAllDishes();


    }
}
