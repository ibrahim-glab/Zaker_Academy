using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Zaker_Academy.infrastructure.Entities;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await CategoryService.GetAll());
            }
            catch (Exception)
            {

                return StatusCode(500); ;
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest();
            try
            {
                var res = await CategoryService.GetById(id);
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
                var res = await CategoryService.Delete(id);
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
        public async Task<IActionResult> Put(int id, [FromBody] CategoryCreationDto category)
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

        [HttpPost("{id}/subcategory")]
        public async Task<IActionResult> Post(int id ,[FromBody] SubCategoryCreationDto category)
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
                var res = await CategoryService.CreateSubCategory(id, category);
                if (!res.succeeded)
                    return NotFound();
                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpGet("{id}/subcategory" )]
        public async Task<IActionResult> GetAll(int id)
        {
          

            try
            {
                var res = await CategoryService.GetAllSubCategories(id);
                if (!res.succeeded)
                    return NotFound();
                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
     

    }
}
