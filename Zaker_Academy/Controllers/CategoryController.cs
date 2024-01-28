using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService CategoryService;
        public CategoryController(ICategoryService categoryService)
        {
            CategoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreationDto category)
        {
            if (category is null)
               return BadRequest();

            if (!User.IsInRole("ADMIN"))
                return Forbid();
            if (!Regex.IsMatch(category.Name, "^[A-Za-z&\\s]*$"))
            {
                return BadRequest();
            }
            try
            {
                return Ok(await CategoryService.Create(category));
            }
            catch (Exception)
            {
                return StatusCode(500);           
            }
        }
        
    }
}
