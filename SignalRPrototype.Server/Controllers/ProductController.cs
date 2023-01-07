using Microsoft.AspNetCore.Mvc;
using SignalRPrototype.Server.Utility;

namespace SignalRPrototype.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    [HttpGet]
    [Route("GetProducts")]
    public IActionResult GetProducts()
    {
        return Ok(ProductHelper.GetAllProducts());
    }
}