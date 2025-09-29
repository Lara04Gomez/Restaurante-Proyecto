﻿using Application.Exceptions;
using Application.Exceptions.Application.Exceptions;
using Application.Interfaces.ICategory.Repository;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.Repository;
using Application.Interfaces.IOrder.Repository;
using Application.Interfaces.IOrderItem.Repository;
using Application.Models.Response;
using Application.Models.Response.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DishServices
{
    public class DeleteDishUseCase : IDeleteDishUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        public DeleteDishUseCase(ICategoryRepository categoryRepository, IDishRepository dishRepository, IOrderItemRepository orderItemRepository)
        {
            _categoryRepository = categoryRepository;
            _dishRepository = dishRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<DishResponse?> DeleteDish(Guid id)
        {
            var dish = await _dishRepository.GetDishById(id);
            if (dish == null)
            {
                throw new NotFoundException($"El plato con ID {id} no se encuentra.");
            }

            bool usedInOrders = await _orderItemRepository.ExistsByDishId(id);
            if (usedInOrders)
            {
                throw new ConflictException($"El plato con ID {id} no puede ser borrado porque está en uso en órdenes existentes.");
            }
  
            dish.Available = false;
            await _dishRepository.UpdateDish(dish);

            return new DishResponse
            {
                    Id = id,
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

