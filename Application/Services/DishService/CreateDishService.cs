using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services.DishServices
{
    public class CreateDishService : ICreateDish
    {
        private readonly IDishCommand _dishCommand;
        private readonly IDishQuery _dishQuery;
        private readonly ICategoryQuery _categoryQuery;

        public CreateDishService(IDishCommand dishCommand, IDishQuery dishQuery, ICategoryQuery categoryQuery)
        {
            _dishCommand = dishCommand;
            _dishQuery = dishQuery;
            _categoryQuery = categoryQuery;
        }

        public async Task<DishResponse?> CreateDish(DishRequest dishRequest)
        {
            //validaciones
            var existingDish = await _dishQuery.DishExists(dishRequest.Name);

            if (existingDish)
            {
                return null;
            }
            var category = await _categoryQuery.GetCategoryById(dishRequest.Category);
            var dish = new Dish
            {
                DishId = Guid.NewGuid(),
                Name = dishRequest.Name,
                Description = dishRequest.Description,
                Price = dishRequest.Price,
                Available = true,
                ImageUrl = dishRequest.ImageUrl,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CategoryId = dishRequest.Category
            };

            await _dishCommand.CreateDish(dish);

            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse { Id = category.Id, Name = category.Name },
                isActive = dish.Available,
                ImageUrl = dish.ImageUrl,
                createAt = dish.CreateDate,
                updateAt = dish.UpdateDate
            };
        }
    }
}
