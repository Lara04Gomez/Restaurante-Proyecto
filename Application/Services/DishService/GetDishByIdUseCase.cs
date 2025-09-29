using Application.Interfaces.ICategory.Repository;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.Repository;
using Application.Models.Response;
using Application.Models.Response.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;


namespace Application.Services.DishServices
{
    public class GetDishByIdUseCase : IGetDishByIdUseCase
    {
        private readonly IDishRepository _dishRepository;
        public GetDishByIdUseCase(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }
        public async Task<DishResponse?> GetDishById(Guid id)
        {
            var dish = await _dishRepository.GetDishById(id);
            if (dish == null)
            {
                throw new NotFoundException($"El plato con el ID {id} no se encuentra.");

            }

            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse { Id = dish.CategoryId, Name = dish.Category.Name },
                isActive = dish.Available,
                Image = dish.Image,
                createAt = dish.CreateDate,
                updateAt = dish.UpdateDate
            };
        }
    }
}
