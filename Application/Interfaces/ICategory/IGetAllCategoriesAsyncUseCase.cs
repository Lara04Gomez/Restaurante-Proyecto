using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.ICategory
{
    public interface IGetAllCategoriesAsyncUseCase
    {
        Task<List<CategoryResponse>> GetAllAsync();
    }
}
