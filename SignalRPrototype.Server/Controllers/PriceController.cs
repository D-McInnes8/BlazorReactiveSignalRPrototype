using Microsoft.AspNetCore.Mvc;
using SignalRPrototype.Server.Utility;
using SignalRPrototype.Shared.Models;

namespace SignalRPrototype.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriceController : ControllerBase
{
    [HttpGet]
    [Route("GetLatestStorePrices")]
    public IActionResult GetLatestStorePrices(Guid? storeId)
    {
        if (storeId is null)
            return BadRequest();
        
        var products = ProductHelper.GetAllProducts();
        var prices = products.Select(p => new ProductPrice()
        {
            ProductId = p.ProductId,
            StoreId = (Guid)storeId,
            TimeGenerated = DateTime.UtcNow,
            Price = ProductHelper.GenerateProductPrice().Price
        });
        return Ok(prices);
    }
}