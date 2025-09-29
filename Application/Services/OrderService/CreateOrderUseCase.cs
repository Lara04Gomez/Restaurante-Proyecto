using Application.Exceptions;
using Application.Exceptions.Application.Exceptions;
using Application.Interfaces.IDeliveryType.Repository;
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
using Application.Exceptions;

namespace Application.Services.OrderService
{
    public class CreateOrderUseCase : ICreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;
        private readonly IDishRepository _dishRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        public CreateOrderUseCase(IOrderRepository orderRepository,
            IDeliveryTypeQuery deliveryTypeQuery,
            IDishRepository dishRepository,
            IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _deliveryTypeQuery = deliveryTypeQuery;
            _dishRepository = dishRepository;
            _orderItemRepository = orderItemRepository;
        }
        public async Task<OrderCreateResponse> CreateOrder(OrderRequest orderRequest)
        {
            
            if (orderRequest.delivery == null)
                throw new RequiredParameterException("El campo delivery es obligatorio.");

            var deliveryType = await _deliveryTypeQuery.GetDeliveryTypeById(orderRequest.delivery.id);
            if (deliveryType == null)
                throw new NotFoundException($"No existe un tipo de entrega con ID {orderRequest.delivery.id}");

            if (orderRequest.items == null || !orderRequest.items.Any())
                throw new RequiredParameterException("Debe especificar al menos un item en la orden.");

            foreach (var item in orderRequest.items)
            {
                var dish = await _dishRepository.GetDishById(item.id);
                if (dish == null)
                    throw new NotFoundException($"El plato con ID {item.id} no existe.");
                if (!dish.Available)
                    throw new InvalidateParameterException($"El plato {dish.Name} no está disponible actualmente.");
                if (item.quantity <= 0)
                    throw new InvalidateParameterException($"La cantidad del plato {dish.Name} debe ser mayor a 0.");
            }

            var order = new Order
            {
                DeliveryTypeId = orderRequest.delivery.id,
                Price = 0,
                StatusId = 1,
                DeliveryTo = orderRequest.delivery.to,
                Notes = orderRequest.notes,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                 OrderDate = DateTime.Now
            };

            await _orderRepository.InsertOrder(order);

            var orderItems = orderRequest.items.Select(item => new OrderItem
            {
                DishId = Guid.Parse(item.id.ToString()),
                Quantity = item.quantity,
                Notes = item.notes,
                StatusId = 1,
                OrderId = order.OrderId,
                 CreateDate = DateTime.Now
            }).ToList();

            order.Price = await CalculateTotalPrice(orderRequest.items);
            await _orderItemRepository.InsertOrderItemRange(orderItems);
            await _orderRepository.UpdateOrder(order);

            return new OrderCreateResponse
            {
                orderNumber = (int)order.OrderId,
                totalAmount = (double)order.Price,
                createAt = DateTime.Now
            };
        }
        private async Task<decimal> CalculateTotalPrice(List<Items> orderItems)
        {
            decimal total = 0;
          
            foreach (var item in orderItems)
            {
                var dish = await _dishRepository.GetDishById(item.id);
                if (dish == null || !dish.Available)
                {
                    throw new NotFoundException($"El plato con ID {item.id} no existe o no está disponible.");
                }
                total += dish.Price * item.quantity;
            }
            return total;
        }
    }
}
