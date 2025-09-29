using Application.Interfaces.IDeliveryType;
using Application.Models.Response;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Restaurante_Proyecto.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    public class DeliveryTypeController : ControllerBase
    {
        private readonly IGetAllDeliveryAsyncUseCase _getallDeliverys;
        public DeliveryTypeController(IGetAllDeliveryAsyncUseCase getallDeliverys)
        {
            _getallDeliverys = getallDeliverys;
        }
        /// <summary>
        /// Obtener tipos de entrega
        /// </summary>
        /// <remarks>
        /// Obtiene todos los tipos de entrega disponibles para las órdenes.
        /// </remarks>
        [HttpGet]
        [SwaggerOperation(
        Summary = "Obtener tipos de entrega",
        Description = "Obtiene todos los tipos de entrega disponibles para las órdenes."
        )]
        [ProducesResponseType(typeof(IEnumerable<DeliveryTypeResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDeliveryTypes()
        {
            var deliveryTypes = await _getallDeliverys.GetAllAsync();
            return Ok(deliveryTypes);
        }
    }
}