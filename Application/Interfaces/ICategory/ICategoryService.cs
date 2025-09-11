using Application.Models.Request;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.ICategory
{
    public interface ICategoryService
    { 
 
        Task<List<CategoryResponse>> GetAllCategories();

        
        Task<CategoryResponse> CreateCategory(CategoryRequest categoria);

        Task<CategoryResponse> UpdateCategory(int id, CategoryRequest categoria);

        
        Task<CategoryResponse> DeleteCategory(int id);

        Task<CategoryResponse> GetCategoryById(int id);

    }
}
