using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.ICategory.Repository;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ICategoryQuery _categoryQuery;
        private readonly ICategoryCommand _categoryCommand;
        public CategoryRepository(ICategoryQuery categoryQuery, ICategoryCommand categoryCommand)
        {
            _categoryQuery = categoryQuery;
            _categoryCommand = categoryCommand;
        }
        
        public Task<List<Category>> GetAllCategories()
        {
            return _categoryQuery.GetAllCategories();
        }
        public Task<Category?> GetCategoryById(int id)
        {
            return _categoryQuery.GetCategoryById(id);
        }
        public Task<bool> CategoryExistsAsync(int id)
        {
            return _categoryQuery.CategoryExistsAsync(id);
        }
        
        public Task InsertCategory(Category category)
        {
            return _categoryCommand.InsertCategory(category);
        }
        public Task UpdateCategory(Category category)
        {
            return _categoryCommand.UpdateCategory(category);
        }
        public Task RemoveCategory(Category category)
        {
            return _categoryCommand.RemoveCategory(category);
        }
    }
}