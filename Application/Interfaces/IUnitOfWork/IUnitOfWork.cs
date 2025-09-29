using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.ICategory.Repository;
using Application.Interfaces.IDish.Repository;

namespace Application.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        IDishRepository DishesRepository { get; }

        ICategoryRepository CategoryRepository { get; }
        Task SaveChangesAsync();

    }
    
    
}
