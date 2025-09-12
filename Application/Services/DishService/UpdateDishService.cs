using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Models.Request;
using Application.Models.Response;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Application.Services.DishServices
{
    public class UpdateDishService : IUpdateDish
    {
        private readonly IDishCommand _dishCommand;
        private readonly IDishQuery _dishQuery;
        private readonly ICategoryQuery _categoryQuery;

        public UpdateDishService(IDishCommand dishCommand, IDishQuery dishQuery, ICategoryQuery categoryQuery)
        {
            _dishCommand = dishCommand;
            _dishQuery = dishQuery;
            _categoryQuery = categoryQuery;
        }

        public async Task<UpdateDishResult> UpdateDish(Guid id, DishUpdateRequest DishUpdateRequest)
        {
            var existingDish = await _dishQuery.GetDishById(id);

            if (existingDish == null)
            {
                return new UpdateDishResult { NotFound = true };
            }
            var alreadyExist = await _dishQuery.DishExists(DishUpdateRequest.Name, id);
            if (alreadyExist)
            {
                return new UpdateDishResult { NameConflict = true };
            }
            var category = await _categoryQuery.GetCategoryById(DishUpdateRequest.Category);

            existingDish.Name = DishUpdateRequest.Name;
            existingDish.Description = DishUpdateRequest.Description;
            existingDish.Price = DishUpdateRequest.Price;
            existingDish.Available = DishUpdateRequest.IsActive;
            existingDish.CategoryId = DishUpdateRequest.Category;
            existingDish.ImageUrl = DishUpdateRequest.Image;
            existingDish.UpdateDate = DateTime.UtcNow;

            await _dishCommand.UpdateDish(existingDish);

            return new UpdateDishResult
            {
                Success = true,
                UpdatedDish = new DishResponse
                {
                    Id = existingDish.DishId,
                    Name = existingDish.Name,
                    Description = existingDish.Description,
                    Price = existingDish.Price,
                    Category = new GenericResponse { Id = category.Id, Name = category.Name },
                    isActive = existingDish.Available,
                    ImageUrl = existingDish.ImageUrl,
                    createAt = existingDish.CreateDate,
                    updateAt = existingDish.UpdateDate
                }
            };
        }
    }
}
