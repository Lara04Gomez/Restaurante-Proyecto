using Application.Interfaces.IStatus;
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
    public class StatusController : ControllerBase
    {
        private readonly IGetAllStatusAsyncUseCase _getAllStatus;
        public StatusController(IGetAllStatusAsyncUseCase getAllStatus)
        {
            _getAllStatus = getAllStatus;
        }
        /// <summary>
        /// Obtener estados de órdenes
        /// </summary>
        /// <remarks>
        /// Obtiene todos los estados posibles para las órdenes y sus items.
        /// </remarks>
        [HttpGet]
        [SwaggerOperation(
        Summary = "Obtener estados de órdenes",
        Description = "Obtiene todos los estados posibles para las órdenes y sus items."
        )]
        [ProducesResponseType(typeof(IEnumerable<StatusResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStatuses()
        {
            var statuses = await _getAllStatus.GetAllStatuses();
            return Ok(statuses);
        }

    }
}
