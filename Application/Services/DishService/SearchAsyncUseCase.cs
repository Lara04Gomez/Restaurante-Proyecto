using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.ICategory.Repository;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.Repository;
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
    public class SearchAsyncUseCase : ISearchAsyncUseCase
    {
        private readonly IDishRepository _dishRepository;
        private readonly ICategoryRepository _categoryRepository;
        public SearchAsyncUseCase(IDishRepository dishRepository, ICategoryRepository categoryRepository)
        {
            _dishRepository = dishRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<DishResponse?>> SearchAsync(string? name, int? categoryId, OrderPrice? priceOrder = OrderPrice.ASC, bool? onlyActive = null)
        {
            if (categoryId.HasValue && categoryId.Value != 0)
            {
                var categoryExists = await _categoryRepository.CategoryExistsAsync(categoryId.Value);
                if (!categoryExists)
                {
                    throw new NotFoundException($"La categoría con ID {categoryId} no se encuentra.");
                }
            }

            var list = await _dishRepository.GetAllAsync(name, categoryId, priceOrder, onlyActive);



            return list.Select(dishes => new DishResponse
            {
                Id = dishes.DishId,
                Name = dishes.Name,
                Description = dishes.Description,
                Price = dishes.Price,
                Category = new GenericResponse { Id = dishes.CategoryId, Name = dishes.Category?.Name },
                isActive = dishes.Available,
                Image = dishes.Image,
                createAt = dishes.CreateDate,
                updateAt = dishes.UpdateDate
            }).ToList();
        }

       
    }
}