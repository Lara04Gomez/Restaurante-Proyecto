using Application.Interfaces.ICategory;
using Application.Interfaces.ICategory.Repository;
using Application.Interfaces.IDish.Repository;
using Application.Interfaces.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CategoryService
{
    public class CategoryExistsUseCase : ICategoryExistsUseCase
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IDishRepository _dishRepository;
            public CategoryExistsUseCase(ICategoryRepository categoryRepository, IDishRepository dishRepository)
            {
                _categoryRepository = categoryRepository;
                _dishRepository = dishRepository;
            }
            public async Task<bool> CategoryExists(int id)
            {
                var category = await _categoryRepository.CategoryExistsAsync(id);
                return category;
            }
        }
    }
