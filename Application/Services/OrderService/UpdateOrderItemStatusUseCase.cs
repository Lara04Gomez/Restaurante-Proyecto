using Application.Exceptions;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrder.Repository;
using Application.Models.Request;
using Application.Models.Response.Order;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Services.OrderService
{
    public class UpdateOrderItemStatusUseCase : IUpdateOrderItemStatusUseCase
    {
            private readonly IOrderRepository _orderRepository;

            public UpdateOrderItemStatusUseCase(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }

            public async Task<OrderUpdateResponse> UpdateItemStatus(long orderId, int itemId, OrderItemUpdateRequest request)
            {
                
                var order = await _orderRepository.GetOrderById(orderId);
                if (order == null)
                throw new NotFoundException("Orden no encontrada");

            var item = order.OrderItems.FirstOrDefault(i => i.OrderItemId == itemId);
                if (item == null)
                throw new NotFoundException("Item no encontrado en la orden");

            if (!Enum.IsDefined(typeof(ItemStatus), request.status))
                throw new InvalidateParameterException("El estado especificado no es válido");

            var State = (ItemStatus)item.StatusId;
            var NewState = (ItemStatus)request.status;

            if (!IsTransitionValid(item.StatusId, request.status))
                throw new InvalidateParameterException(
                    $"No se puede cambiar de '{State}' a '{NewState}'");

            item.StatusId = request.status;

                UpdateOrderStatus(order);

                await _orderRepository.UpdateOrder(order);

                return new OrderUpdateResponse
                {
                    orderNumber = (int)order.OrderId,
                    totalAmount = (double)order.Price,
                    UpdateAt = DateTime.UtcNow
                };
            }

        private void UpdateOrderStatus(Order order)
        {
            if (order.OrderItems.All(i => i.StatusId == (int)ItemStatus.Listo))
                order.StatusId = (int)ItemStatus.Listo;
            else if (order.OrderItems.Any(i => i.StatusId == (int)ItemStatus.EnPreparacion))
                order.StatusId = (int)ItemStatus.EnPreparacion;
            else if (order.OrderItems.Any(i => i.StatusId == (int)ItemStatus.Entregado))
                order.StatusId = (int)ItemStatus.Entregado;
            else if (order.OrderItems.All(i => i.StatusId == (int)ItemStatus.Cancelado))
                order.StatusId = (int)ItemStatus.Cancelado;
            else
                order.StatusId = (int)ItemStatus.Pendiente;
        }
        private bool IsTransitionValid(int State, int NewState)
        {
            var ValidTransitions = new Dictionary<ItemStatus, List<ItemStatus>>
    {
        { ItemStatus.Pendiente, new List<ItemStatus> { ItemStatus.EnPreparacion, ItemStatus.Cancelado } },
        { ItemStatus.EnPreparacion, new List<ItemStatus> { ItemStatus.Listo, ItemStatus.Cancelado } },
        { ItemStatus.Listo, new List<ItemStatus> { ItemStatus.Entregado } },
        { ItemStatus.Entregado, new List<ItemStatus>() },
        { ItemStatus.Cancelado, new List<ItemStatus>() }
    };

            var Actual = (ItemStatus)State;
            var New = (ItemStatus)NewState;

            return ValidTransitions.ContainsKey(Actual) &&
                  ValidTransitions[Actual].Contains(New);
        }

    }
}
