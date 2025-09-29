using Application.Exceptions;
using Application.Exceptions.Application.Exceptions;
using Application.Interfaces.ICategory.Repository;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.Repository;
using Application.Models.Request;
using Application.Models.Response;
using Application.Models.Response.Dish;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DishServices
{
    public class CreateDishUseCase : ICreateDishUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDishRepository _dishRepository;

        public CreateDishUseCase(ICategoryRepository categoryRepository, IDishRepository dishRepository)
        {
            _categoryRepository = categoryRepository;
            _dishRepository = dishRepository;
        }
        public async Task<DishResponse?> CreateDish(DishRequest dishRequest)
        {
            
            var existingDish = await _dishRepository.DishExists(dishRequest.Name, null);

            if (existingDish)
            {
                throw new ConflictException($"EL plato con el nombre {dishRequest.Name} ya existe.");
            }
            var category = await _categoryRepository.GetCategoryById(dishRequest.CategoryId);
           
            if (category == null)
            {
                throw new NotFoundException($"La categoría con ID {dishRequest.CategoryId} no existe.");
            }

            var dish = new Dish
            {
                DishId = Guid.NewGuid(),
                Name = dishRequest.Name,
                Description = dishRequest.Description,
                Price = dishRequest.Price,
                Available = true,
                Image = dishRequest.Image,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CategoryId= dishRequest.CategoryId
            };
            await _dishRepository.InsertDish(dish);
            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse { Id = category.Id, Name = category.Name },
                isActive = dish.Available,
                Image = dish.Image,
                createAt = dish.CreateDate,
                updateAt = dish.UpdateDate
            };
        }
    }
}