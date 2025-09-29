using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.ICategory;
using Application.Interfaces.ICategory.Repository;
using Application.Models.Response;


namespace Application.Services.CategoryService
{
    public class GetAllCategoriesAsyncUseCase : IGetAllCategoriesAsyncUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        public GetAllCategoriesAsyncUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllCategories();

            return categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                order = c.Order
            }).ToList();
        }
    }
}
