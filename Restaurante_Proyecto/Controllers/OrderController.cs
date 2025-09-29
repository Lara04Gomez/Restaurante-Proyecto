using Application.Exceptions;
using Application.Interfaces.IOrder;
using Application.Models.Request;
using Application.Models.Response;
using Application.Models.Response.Dish;
using Application.Models.Response.Order;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Restaurante_Proyecto.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    public class OrderController : ControllerBase
    {
        private readonly ICreateOrderUseCase _createOrderUseCase;
        private readonly IGetOrderWithFilterUseCase _getOrderWithFilterUseCase;
        private readonly IGetOrderByIdUseCase _getOrderById;
        private readonly IUpdateItemFromOrderUseCase _updateItemFromOrderUseCase;
        private readonly IUpdateOrderItemStatusUseCase _updateOrderItemStatus;
        public OrderController
            (ICreateOrderUseCase createOrderUseCase,
            IGetOrderWithFilterUseCase getOrderWithFilterUseCase,
            IGetOrderByIdUseCase getOrderById,
            IUpdateItemFromOrderUseCase updateItemFromOrderUseCase,
            IUpdateOrderItemStatusUseCase updateOrderItemStatus)
        {
            _createOrderUseCase = createOrderUseCase;
            _getOrderWithFilterUseCase = getOrderWithFilterUseCase;
            _getOrderById = getOrderById;
            _updateItemFromOrderUseCase = updateItemFromOrderUseCase;
            _updateOrderItemStatus = updateOrderItemStatus;
        }

        // POST
        /// <summary>
        /// Crear nueva orden.
        /// </summary>
        /// <remarks>
        /// Crea un nueva orden en el menú del restaurante.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(OrderCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] Application.Models.Request.OrderRequest orderRequest)
        {
                var result = await _createOrderUseCase.CreateOrder(orderRequest);
                return CreatedAtAction(nameof(CreateOrder), new { id = result.orderNumber }, result);   
        }

       
        /// <summary>
        /// Buscar órdenes.
        /// </summary>
        /// <remarks>
        /// Obtiene una lista de órdenes con filtros opcionales.
        /// </remarks>
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderDetailsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> GetOrders([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? statusId)
        {
            var result = await _getOrderWithFilterUseCase.GetOrderWithFilter(from, to, statusId);
            return Ok(result);
        }

        
        /// <summary>
        /// Obtener orden por número.
        /// </summary>
        /// <remarks>
        /// Obtiene los detalles completos de una orden específica.
        /// </remarks>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
        Summary = "Buscar orders por ID",
        Description = "Obtiene los detalles completos de una orden específica."
        )]
        [ProducesResponseType(typeof(OrderDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var order = await _getOrderById.GetOrderById(id);
            return Ok(order);
        }
        /// <summary>
        ///Actualizar orden existente
        /// </summary>
        /// <remarks>
        ///Actualiza los items de una orden existente.
        /// </remarks>
        [HttpPut]
         [ProducesResponseType(typeof(OrderUpdateResponse), StatusCodes.Status200OK)]
         [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItems(long orderId,[FromBody] OrderUpdateRequest request)
        {
            var response = await _updateItemFromOrderUseCase.UpdateItemQuantity(orderId, request);
            return Ok(response);
        }


        /// <summary>
        /// Actualizar estado de item individual.
        /// </summary>
        /// <remarks>
        /// Actualiza el estado de un item específico dentro de una orden.
        /// </remarks>
        [HttpPut("{orderId}/item/{itemId}")]

        [ProducesResponseType(typeof(IEnumerable<OrderDetailsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderItemStatus(long orderId, int itemId, [FromBody] OrderItemUpdateRequest request)
        {
            var response = await _updateOrderItemStatus.UpdateItemStatus(orderId, itemId, request);
            return Ok(response);
        }
    }
}