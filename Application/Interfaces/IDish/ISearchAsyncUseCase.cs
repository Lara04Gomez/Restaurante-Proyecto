using Application.Enums;
using Application.Models.Response.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDish
{
    public interface ISearchAsyncUseCase
    {
        
        Task<IEnumerable<DishResponse?>> SearchAsync(string? name, int? categoryId, OrderPrice? priceOrder, bool? onlyActive);
    }
}
