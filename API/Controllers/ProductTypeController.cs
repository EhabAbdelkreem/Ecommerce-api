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
    public class ProductTypeController : BaseApiController
    {

        private readonly IGenericRepository<ProductType> _productTypeRepo;
        
        public ProductTypeController(
           IGenericRepository<ProductType> productTypeRepo)
        {
           
            _productTypeRepo = productTypeRepo;
           
        }



        [HttpPost("addproductType")]
        public async Task<ActionResult<ProductType>> addproduct(ProductType ProductType)
        {
          _productTypeRepo .Add(ProductType);
            var res = await _productTypeRepo.Complete();
            if (res <= 0) return BadRequest(new ApiResponse(400, "problem saving the producttype"));
            return Ok(ProductType);
        }

        [HttpPut("editeproductType/{id}")]
        public async Task<ActionResult<ProductType>> updateproductType(int id, ProductType ProductType)
        {
            var producttype = await _productTypeRepo.GetByIdAsync(id);


            _productTypeRepo.Update(producttype);
            var res = await _productTypeRepo.Complete();
            if (res <= 0) return BadRequest(new ApiResponse(400, "problem updating the producttype"));
            return Ok(producttype);

        }
        [HttpDelete("deletproductType /{id}")]
        public async Task<ActionResult> DeleteproductType(int id)
        {
            var productType = await _productTypeRepo.GetByIdAsync(id);

           _productTypeRepo.Delete(productType);
            var res = await _productTypeRepo.Complete();
            if (res <= 0) return BadRequest(new ApiResponse(400, "problem Deleting the producttype"));
            return Ok(productType);


        }

    }
}