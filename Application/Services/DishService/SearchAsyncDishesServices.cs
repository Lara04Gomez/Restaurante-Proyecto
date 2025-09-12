using Application.Enums;
using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DishServices
{
    public class SearchAsyncDishesService : ISearchAsync
    {
        private readonly IDishCommand _dishCommand;
        private readonly IDishQuery _dishQuery;
        private readonly ICategoryQuery _categoryQuery;

        public SearchAsyncDishesService(IDishCommand dishCommand, IDishQuery dishQuery, ICategoryQuery categoryQuery)
        {
            _dishCommand = dishCommand;
            _dishQuery = dishQuery;
            _categoryQuery = categoryQuery;
        }

        public async Task<IEnumerable<DishResponse?>> SearchAsync(string? name, int? categoryId, bool? onlyActive = true, OrderPrice? priceOrder = OrderPrice.ASC)
        {

            var list = await _dishQuery.GetAllAsync(name, categoryId, onlyActive, priceOrder);



            return list.Select(dishes => new DishResponse
            {
                Id = dishes.DishId,
                Name = dishes.Name,
                Description = dishes.Description,
                Price = dishes.Price,
                Category = new GenericResponse { Id = dishes.CategoryId, Name = dishes.Category?.Name },
                isActive = dishes.Available,
                ImageUrl = dishes.ImageUrl,
                createAt = dishes.CreateDate,
                updateAt = dishes.UpdateDate
            }).ToList();
        }


    }
}

