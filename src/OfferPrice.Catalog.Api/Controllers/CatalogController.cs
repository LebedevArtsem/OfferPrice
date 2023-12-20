using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferPrice.Catalog.Api.Models;
using OfferPrice.Catalog.Domain;
using OfferPrice.Common;
using OfferPrice.Events.Events;
using OfferPrice.Events.Interfaces;

namespace OfferPrice.Catalog.Api.Controllers;

[ApiController]
[Route("api/products")]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository _products;
    private readonly IProducer _producer;
    private readonly IMapper _mapper;

    public CatalogController(IProductRepository products, IProducer producer, IMapper mapper)
    {
        _products = products;
        _producer = producer;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts
        ([FromQuery] GetProductsRequest productRequest, CancellationToken token)
    {
        var page = await _products.Get
            (
            productRequest.Name,
            productRequest.Username,
            productRequest.Category,
            productRequest.Page,
            productRequest.PerPage,
            token
            );

        var products = page.Items.Select(_mapper.Map<Domain.Product, Models.Product>).ToList();

        var pageResult = new PageResult<Models.Product>(page.Page, page.PerPage, page.Total, products);

        return Ok(pageResult);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] string id, CancellationToken token)
    {
        var product = await _products.GetById(id, token);

        var productResponse = _mapper.Map<Domain.Product, Models.Product>(product);

        return Ok(productResponse);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] InsertProductRequest productRequest, CancellationToken token)
    {
        var product = _mapper.Map<InsertProductRequest, Domain.Product>(productRequest);

        await _products.Insert(product, token);
        
        _producer.SendMessage(new ProductCreatedEvent(product.ToEvent()));

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] string id, [FromBody] UpdateProductRequest productRequest, CancellationToken token)
    {
        var product = await _products.GetById(id, token);

        if (product == null)
        {
            return NotFound();
        }

        var updatedProduct = _mapper.Map(productRequest, product);

        await _products.Update(updatedProduct, token);
        return Ok();
    }
}

