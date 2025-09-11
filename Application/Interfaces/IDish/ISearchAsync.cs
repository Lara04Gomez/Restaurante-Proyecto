using Application.Enums;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDish
{
    public interface ISearchAsync
    {
        
        Task<IEnumerable<DishResponse?>> SearchAsync(string? name, int? categoryId, bool? onlyActive, OrderPrice? priceOrder);
    }
}
