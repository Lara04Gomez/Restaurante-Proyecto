using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.IDish;
using Application.Interfaces.ICategory;
using Application.Models.Response;

namespace Application.Services.DishServices
{
    public class DeleteDishService:IDeleteDish
    {
        private readonly IDishCommand _dishCommand;
        private readonly IDishQuery _dishQuery;
        private readonly ICategoryQuery _categoryQuery;

        public DeleteDishService(IDishCommand dishCommand, IDishQuery dishQuery, ICategoryQuery categoryQuery)
        {
            _dishCommand = dishCommand;
            _dishQuery = dishQuery;
            _categoryQuery = categoryQuery;
        }

        public Task<DishResponse> DeleteDish(Guid id)
        {
            throw new NotImplementedException();
        }

       
    }
}
