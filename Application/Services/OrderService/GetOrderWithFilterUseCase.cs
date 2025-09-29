using Application.Interfaces.IDeliveryType.Repository;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrder.Repository;
using Application.Interfaces.IStatus.Repository;
using Application.Models.Response;
using Application.Models.Response.Dish;
using Application.Models.Response.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;

namespace Application.Services.OrderService
{
    public class GetOrderWithFilterUseCase : IGetOrderWithFilterUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IStatusQuery _statusQuery;
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;
        public GetOrderWithFilterUseCase(IOrderRepository orderRepository, IStatusQuery statusQuery, IDeliveryTypeQuery deliveryTypeQuery)
        {
            _orderRepository = orderRepository;
            _statusQuery = statusQuery;
            _deliveryTypeQuery = deliveryTypeQuery;
        }
        public async Task<IEnumerable<OrderDetailsResponse?>> GetOrderWithFilter( DateTime? from, DateTime? to, int? statusId)
        {
            var orders = await _orderRepository.GetOrderWithFilter( from, to, statusId);

            if (orders == null || !orders.Any())
                throw new NotFoundException("No hay órdenes encontradas con esos filtros específicos.");

            var orderResponses = orders.Select(order =>
            new OrderDetailsResponse
            {
                orderNumber = (int)order.OrderId,
                totalAmount = (double)order.Price,
                deliveryTo = order.DeliveryTo,
                notes = order.Notes,
                status = new GenericResponse { Id = order.StatusId, Name = order.OverallStatus?.Name ?? "Desconocido" },
                deliveryType = new GenericResponse { Id = order.DeliveryTypeId, Name = order.DeliveryType?.Name ?? "Desconocido" },
                items = order.OrderItems.Select(item => new OrderItemResponse
                {
                    Id = 2,
                    Quantity = item.Quantity,
                    notes = item.Dish?.Name,
                    dish = new DishShortResponse { Id = item.DishId, Name = item.Dish?.Name ?? "Desconocido", Image = item.Dish?.Image ?? "No encontrada" },
                    status = new GenericResponse { Id = item.Status.Id, Name = item.Status?.Name ?? "Desconocido" }
                }).ToList(),
                createAt = order.CreateDate,
                UpdateAt = order.UpdateDate
            });
            return orderResponses;
        }
    }
}
