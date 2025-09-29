using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ICategory.Repository
{
    public interface ICategoryCommand
    {
        Task InsertCategory(Category category);
        Task UpdateCategory(Category category);
        Task RemoveCategory(Category category);
    }
}
