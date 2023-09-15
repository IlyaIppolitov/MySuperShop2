using Microsoft.AspNetCore.Mvc;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories;

namespace MyShopBackend.Controllers;

[ApiController]
public class CatalogController : ControllerBase
{
    private readonly IRepository<Product> _repository;

    public CatalogController(IRepository<Product> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    [HttpGet("get_products")]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var products =  await _repository.GetAll(cancellationToken);
        return Ok(products);
    }
    
    [HttpGet("get_product")]
    public async Task<ActionResult<Product>> GetProductByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var foundProduct = await _repository.GetById(id, cancellationToken);
        return Ok(foundProduct);
    }
    
    [HttpPost ("add_product")]
    public async Task<IActionResult> AddProductAsync([FromBody]Product product,CancellationToken cancellationToken)
    {
        await _repository.Add(product, cancellationToken);
        return Created("Created!", product);
    }
    
    [HttpPost("update_product")]
    public async Task<ActionResult> UpdateProductAsync([FromBody] Product product, CancellationToken cancellationToken)
    {
        await _repository.Update(product, cancellationToken);
        return Ok();
    }
    
    [HttpPost("delete_product")]
    public async Task<ActionResult> DeleteProductAsync([FromBody] Product product, CancellationToken cancellationToken)
    {
        await _repository.Delete(product, cancellationToken);
        return Ok();
    }
}