using Application.Interfaces.IDish.Repository;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrder.Repository;
using Application.Interfaces.IOrderItem.Repository;
using Application.Models.Request;
using Application.Models.Response.Order;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Application.Exceptions;

namespace Application.Services.OrderService
{
    public class UpdateItemFromOrderUseCase : IUpdateItemFromOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IDishRepository _dishRepository;
        public UpdateItemFromOrderUseCase(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IDishRepository dishRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _dishRepository = dishRepository;
        }
        public async Task<OrderUpdateResponse> UpdateItemQuantity(long orderId,OrderUpdateRequest listItems)
        {
           
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new NotFoundException("Orden no encontrada");
            }


            if (order.StatusId != (int)Domain.Enums.OrderStatus.Pendiente)
                throw new BadHttpRequestException("No se puede modificar una orden que ya está en preparación");

            foreach (var itemRequest in listItems.items)
            {
                var dish = await _dishRepository.GetDishById(itemRequest.id);
                if (dish == null || !dish.Available)
                    throw new BadHttpRequestException("El plato especificado no está disponible");

                var existingItem = order.OrderItems.FirstOrDefault(i => i.DishId == itemRequest.id);

                if (existingItem != null)
                {
                    existingItem.Quantity = itemRequest.quantity;
                    existingItem.Notes = itemRequest.notes;
                }
                else
                {
                    var newItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        DishId = itemRequest.id,
                        Quantity = itemRequest.quantity,
                        Notes = itemRequest.notes,
                        StatusId = (int)Domain.Enums.ItemStatus.Pendiente
                    };
                    order.OrderItems.Add(newItem);
                    await _orderItemRepository.InsertOrderItem(newItem);
                }
            }

            decimal totalPrice = 0;
            foreach (var item in order.OrderItems)
            {
                var dish = await _dishRepository.GetDishById(item.DishId);
                if (dish != null)
                {
                    totalPrice += dish.Price * item.Quantity;
                }
            }
            order.Price = totalPrice;
            order.UpdateDate = DateTime.UtcNow;

            await _orderRepository.UpdateOrder(order);

            return new OrderUpdateResponse
            {
                orderNumber = (int)order.OrderId,
                totalAmount = (double)order.Price,
                UpdateAt = order.UpdateDate
            };
        }
    }
}