using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.IDish
{
    public interface IDishCommand
    {
        Task CreateDish(Dish dish);
        Task UpdateDish(Dish dish);
        Task RemoveDish(Dish dish);

    }
}
