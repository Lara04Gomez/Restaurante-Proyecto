using Application.Exceptions;
using Application.Exceptions.Application.Exceptions;
using Application.Interfaces.ICategory.Repository;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.Repository;
using Application.Models.Request;
using Application.Models.Response;
using Application.Models.Response.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DishServices
{
    public class UpdateDishUseCase : IUpdateDishUseCase
    {
        private readonly IDishRepository _dishRepository;
        private readonly ICategoryRepository _categoryRepository;
        public UpdateDishUseCase(IDishRepository dishRepository, ICategoryRepository categoryRepository)
        {
            _dishRepository = dishRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<DishResponse> UpdateDish(Guid id, DishUpdateRequest DishUpdateRequest)
        {
            var existingDish = await _dishRepository.GetDishById(id);

            if (existingDish == null)
            {
                throw new NotFoundException($"El plato con ID  {id} no se encuentra.");
            }
            var category = await _categoryRepository.GetCategoryById(DishUpdateRequest.CategoryId);
            if (category == null)
            {
                throw new NotFoundException($"La categoría con ID {DishUpdateRequest.CategoryId} no se encuentra.");
            }
            var alreadyExist = await _dishRepository.DishExists(DishUpdateRequest.Name, id);
            if (alreadyExist)
            {
                throw new ConflictException($"EL plato {DishUpdateRequest.Name} ya existe");
            }
            var Category = await _categoryRepository.GetCategoryById(DishUpdateRequest.CategoryId);

            existingDish.Name = DishUpdateRequest.Name;
            existingDish.Description = DishUpdateRequest.Description;
            existingDish.Price = DishUpdateRequest.Price;
            existingDish.Available = DishUpdateRequest.IsActive;
            existingDish.CategoryId = DishUpdateRequest.CategoryId;
            existingDish.Image = DishUpdateRequest.Image;
            existingDish.UpdateDate = DateTime.UtcNow;

            await _dishRepository.UpdateDish(existingDish);

            return new DishResponse
            {
                Id = existingDish.DishId,
                Name = existingDish.Name,
                Description = existingDish.Description,
                Price = existingDish.Price,
                Category = new GenericResponse { Id = category.Id, Name = category.Name },
                isActive = existingDish.Available,
                Image = existingDish.Image,
                createAt = existingDish.CreateDate,
                updateAt = existingDish.UpdateDate
            };
        }
    }
}