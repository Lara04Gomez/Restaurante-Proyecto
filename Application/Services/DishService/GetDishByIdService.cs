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
    public class GetDishByIdService : IGetDishById
    {
        private readonly IDishCommand _dishCommand;
        private readonly IDishQuery _dishQuery;
        private readonly ICategoryQuery _categoryQuery;

        public GetDishByIdService(IDishCommand dishCommand, IDishQuery dishQuery, ICategoryQuery categoryQuery)
        {
            _dishCommand = dishCommand;
            _dishQuery = dishQuery;
            _categoryQuery = categoryQuery;
        }

        public async Task<DishResponse?> GetDishById(Guid id)
        {
            var dish = await _dishQuery.GetDishById(id);
            if (dish == null)
            {
                return null;
            }

            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse { Id = dish.CategoryId, Name = dish.Category?.Name },
                isActive = dish.Available,
                ImageUrl = dish.ImageUrl,
                createAt = dish.CreateDate,
                updateAt = dish.UpdateDate
            };
        }
    }
}

