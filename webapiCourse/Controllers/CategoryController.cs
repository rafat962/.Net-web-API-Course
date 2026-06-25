using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapiCourse.models;
using webapiCourse.Reposotiry;

namespace webapiCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {

        public CategoryController(ICategoryRepo categoryRepo)
        {
            CategoryRepo = categoryRepo;
        }

        public ICategoryRepo CategoryRepo { get; }

        #region GetData
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await CategoryRepo.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return NotFound(ModelState);
            }

        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetByID(int Id) 
        {
            try
            {
            var cate = await CategoryRepo.GetByID(Id);
            return Ok(cate);
            }
            catch (Exception ex)
            {
                return NotFound(ModelState);
            }

        }

        #endregion

        #region Add - Update
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            try { 
                var cate = await CategoryRepo.Add(category);
                return CreatedAtAction(nameof(GetByID), new { Id = cate.Id }, cate);
            }
            catch (Exception ex)
            {
                return NotFound(ModelState);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id , Category category)
        {
            try
            {
                Category CurrentCategory = await CategoryRepo.GetByID(id);

                if(CurrentCategory == null)
                {
                    return NotFound();
                }

                await CategoryRepo.Update( category);

                return Ok(category);


            }
            catch (Exception ex)
            {
                return NotFound(ModelState);
            }

        }

        #endregion

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            try {
                await CategoryRepo.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
            

        }



    }
}
