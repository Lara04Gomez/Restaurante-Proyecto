using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Response.Dish;

namespace Application.Interfaces.IDish
{
    public interface IDeleteDishUseCase
    {
       
        Task<DishResponse> DeleteDish(Guid id);
    }
}
