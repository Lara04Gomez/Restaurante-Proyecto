using Application.Interfaces.ICategory;
using Application.Models.Response;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace Restaurante_Proyecto.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        private readonly IGetAllCategoriesAsyncUseCase _getAllCategoryAsyncUseCase;

        public CategoryController(IGetAllCategoriesAsyncUseCase getAllCategoryAsyncUseCase)
        {
            _getAllCategoryAsyncUseCase = getAllCategoryAsyncUseCase;
        }

        /// <summary>
        /// Obtener categorías de platos
        /// </summary>
        /// <remarks>
        /// Obtiene todas las categorías disponibles para clasificar platos.
        /// </remarks>
        [HttpGet]
        [SwaggerOperation(
        Summary = "Obtener categorías de platos",
        Description = "Obtiene todas las categorías disponibles para clasificar platos."
        )]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
      
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _getAllCategoryAsyncUseCase.GetAllAsync();
            return Ok(categories);
        }
    }
}
