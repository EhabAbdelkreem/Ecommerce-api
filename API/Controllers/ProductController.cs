﻿using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {

        private readonly IProductRepository _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository,
            IGenericRepository<ProductBrand> productBrandRepo, IMapper mapper, IGenericRepository<ProductType> productTypeRepo, IGenericRepository<Product> productRepo)
        {
            _productRepository = productRepository;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProducctWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSepecification(productParams);

            var totalItems = await _productRepo.CountAsync(countSpec);

            var products = await _productRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProducctWithTypesAndBrandsSpecification(id);
            var product = await _productRepo.GetEntityWithSpec(spec);
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpPost("addproduct")]
        public async Task<ActionResult<Product>> addproduct(ProductToReturnDto ProductToReturnDto)
        {
            var prod = _mapper.Map<ProductToReturnDto, Product>(ProductToReturnDto);
            _productRepo.Add(prod);
            var res = await _productRepo.Complete();
            if (res <= 0) return BadRequest(new ApiResponse(400, "problem saving the product"));
            return Ok(prod);
        }



        [HttpPut("editeproduct/{id}")]
        public async Task<ActionResult<Product>> updateproduct(int id, ProductToReturnDto producttoupdate)
        {
            var product = await _productRepo.GetByIdAsync(id);
            _mapper.Map(producttoupdate, product);

            _productRepo.Update(product);
            var res = await _productRepo.Complete();
            if (res <= 0) return BadRequest(new ApiResponse(400, "problem updating the product"));
            return Ok(product);

        }
        [HttpDelete("deletproduct /{id}")]
        public async Task<ActionResult> Deleteproduct(int id)
        {
           var product = await _productRepo.GetByIdAsync(id);

            _productRepo.Delete(product);
            var res = await _productRepo.Complete();
            if (res <= 0) return BadRequest(new ApiResponse(400, "problem Deleting the product"));
            return Ok(product);


        }


    }
}
