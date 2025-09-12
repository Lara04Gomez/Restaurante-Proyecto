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
    public class GetAllDishesAsyncService : IGetAllDishesAsync
    {
        private readonly IDishCommand _dishcommand;
        private readonly IDishQuery _dishQuery;
        private readonly ICategoryQuery _categoryQuery;

        public GetAllDishesAsyncService(IDishCommand dishcommand, IDishQuery dishQuery, ICategoryQuery categoryQuery)
        {
            _dishcommand = dishcommand;
            _dishQuery = dishQuery;
            _categoryQuery = categoryQuery;
        }

        public async Task<List<DishResponse>> GetAllDishesAsync()
        {
            var dishes = await _dishQuery.GetAllDishes();

            return dishes.Select(dishes => new DishResponse
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
