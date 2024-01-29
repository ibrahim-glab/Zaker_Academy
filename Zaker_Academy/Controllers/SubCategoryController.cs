using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ICategoryService CategoryService;

        public SubCategoryController(ICategoryService CategoryService)
        {
            this.CategoryService = CategoryService;
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id )
        {

            if (id <= 0)
                return BadRequest();
            try
            {
                var res = await CategoryService.GetSubCategoryById(id);
                if (res.Data is null)
                    return NotFound();

                return Ok(res);
            }
            catch (Exception)
            {

                return StatusCode(500); ;
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (!User.IsInRole("ADMIN"))
                return Forbid();
            try
            {
                var res = await CategoryService.DeleteSubCategoryById(id);
                if (!res.succeeded)
                    return NotFound();

                return Ok();
            }
            catch (Exception)
            {

                return StatusCode(500); ;
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] SubCategoryCreationDto category)
        {
            if (id <= 0)
                return BadRequest();
            if (category is null)
                return NoContent();
            if (!User.IsInRole("ADMIN"))
                return Forbid();
            try
            {
                var res = await CategoryService.Update(id, category);

                if (!res.succeeded)
                    return NotFound();
                return Ok(res);
            }
            catch (Exception)
            {

                return StatusCode(500); ;
            }
        }
    }
}
