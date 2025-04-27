using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBrandController : BaseApiController
    {

       
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        

        public ProductBrandController(IProductRepository productRepository,
            IGenericRepository<ProductBrand> productBrandRepo)
        {
          
            _productBrandRepo = productBrandRepo;
          
        }



        [HttpPost("addproductBrand")]
        public async Task<ActionResult<ProductBrand>> addproductBrand(ProductBrand ProductBrand)
        {
           _productBrandRepo.Add(ProductBrand);
            var res = await _productBrandRepo.Complete();
            if (res <= 0) return BadRequest(new ApiResponse(400, "problem saving the productbrand"));
            return Ok(ProductBrand);
        }
        [HttpPut("editeproductBrand/{id}")]
        public async Task<ActionResult<ProductBrand>> updateproductBrand(int id, ProductBrand ProductBrand)
        {
            var productBrand = await _productBrandRepo.GetByIdAsync(id);
           

            _productBrandRepo.Update(ProductBrand);
            var res = await _productBrandRepo.Complete();
            if (res <= 0) return BadRequest(new ApiResponse(400, "problem updating the productBrand"));
            return Ok(ProductBrand);

        }
        [HttpDelete("deletproductBrand /{id}")]
        public async Task<ActionResult> DeleteproductBrand(int id)
        {
            var Productbrand = await _productBrandRepo.GetByIdAsync(id);

           _productBrandRepo.Delete(Productbrand);
            var res = await _productBrandRepo.Complete();
            if (res <= 0) return BadRequest(new ApiResponse(400, "problem Deleting the productBrand"));
            return Ok(Productbrand);


        }

    }
}
