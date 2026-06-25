using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapiCourse.Data;
using webapiCourse.models;
using webapiCourse.models.DTOs;

namespace webapiCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public DataDBContext DataDBContext { get; }

        public ProductController(DataDBContext dataDBContext)
        {
            DataDBContext = dataDBContext;
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public ActionResult<GeneralRespose> GetOneProduct(int id)
        {
            GeneralRespose generalRespose = new GeneralRespose();

            Product product = DataDBContext.Products.Include("Category").FirstOrDefault(pro => pro.Id == id);


            if (product == null)
            {
                generalRespose.IsSucced = false;
                generalRespose.data = new
                {
                    status="field",
                    message="there is no user with that ID"

                };
                return generalRespose;
            }

            OneProductDTO oneProductDTO = new OneProductDTO() {
            productID = product.Id,
            productName = product.Name,
            productPrice = product.price,
             CategoryName = product.Category.Name
            };

            generalRespose.IsSucced = true;
            generalRespose.data = new
            {
                status = "success",
                Product = oneProductDTO
            };



            return generalRespose;


        }
    }
}
