using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Models.Request;
using Application.Models.Response;
using Application.Models.Response.Dish;
using Application.Services.DishServices;
using Asp.Versioning;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Restaurante_Proyecto.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    public class DishController : ControllerBase
    {
        private readonly ICreateDishUseCase _createDish;
        private readonly IUpdateDishUseCase _UpdateDish;
        private readonly ISearchAsyncUseCase _SearchAsync;
        private readonly ICategoryExistsUseCase _CategoryExists;
        private readonly IGetDishByIdUseCase _getDishByIdUseCase;
        private readonly IDeleteDishUseCase _deleteDishUseCase;
        public DishController(
            ICreateDishUseCase createDish,
            IUpdateDishUseCase UpdateDish,
            ISearchAsyncUseCase SearchAsync,
            ICategoryExistsUseCase CategoryExists,
            IGetDishByIdUseCase getDishByIdUseCase,
            IDeleteDishUseCase deleteDishUseCase)
        {
            _createDish = createDish;
            _UpdateDish = UpdateDish;
            _SearchAsync = SearchAsync;
            _CategoryExists = CategoryExists;
            _getDishByIdUseCase = getDishByIdUseCase;
            _deleteDishUseCase = deleteDishUseCase;
        }

        
        /// <summary>
        /// Crear nuevo plato.
        /// </summary>
        /// <remarks>
        /// Crea un nuevo plato en el menú del restaurante.
        /// </remarks>
        [HttpPost]
        [SwaggerOperation(
        Summary = "Crear nuevo plato",
        Description = "Crea un nuevo plato en el menú del restaurante."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDish([FromBody] DishRequest dishRequest)
        {

            var categoryExists = await _CategoryExists.CategoryExists(dishRequest.CategoryId);

            var createdDish = await _createDish.CreateDish(dishRequest);
            return CreatedAtAction(nameof(GetDishById), new { id = createdDish.Id }, createdDish);


        }
        /// <summary>
        /// Busca platos.
        /// </summary>
        /// <remarks>
        /// Obtiene una lista de platos del menú con opciones de filtrado y ordenamiento.
        /// </remarks>
       
        [HttpGet]
        [SwaggerOperation(
        Summary = "Buscar platos",
        Description = "Obtiene una lista de platos del menú con opciones de filtrado y ordenamiento."
        )]
        [ProducesResponseType(typeof(IEnumerable<DishResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] int? category,
            [FromQuery] OrderPrice? sortByPrice = OrderPrice.ASC,
            [FromQuery] bool? onlyActive = null)
        {
            var list = await _SearchAsync.SearchAsync(name, category, sortByPrice, onlyActive);
            return Ok(list);
        }
      
        /// <summary>
        /// Obtiene un plato por su ID.
        /// </summary>
        /// <remarks>
        /// Busca un plato específico en el menú usando su identificador único.
        /// </remarks>
        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "Obtener plato por ID",
        Description = "Obtiene los detalles completos de un plato específico.."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDishById(Guid id)
        {
            var dish = await _getDishByIdUseCase.GetDishById(id);
            return Ok(dish);
        }


        /// <summary>
        /// Actualizar plato existente.
        /// </summary>
        /// <remarks>
        /// Actualiza todos los campos de un plato existente en el menú.
        /// </remarks>

        [HttpPut("{id}")]
        [SwaggerOperation(
        Summary = "Actualizar plato existente",
        Description = "Actualiza todos los campos de un plato existente en el menú."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateDish(Guid id, [FromBody] DishUpdateRequest dishRequest)
        {
            var result = await _UpdateDish.UpdateDish(id, dishRequest);
            return Ok(result);
        }

        
        /// <summary>
        /// Eliminar plato
        /// </summary>
        /// <remarks>
        /// Elimina un plato del menú del restaurante.
        /// </remarks>
        /// 
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteDish(Guid id, [FromServices] IDeleteDishUseCase _deleteDish)
        {
            var result = await _deleteDish.DeleteDish(id);
            return Ok(result);
        }
    }
}
