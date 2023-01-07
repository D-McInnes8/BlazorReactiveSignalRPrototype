using Microsoft.AspNetCore.Mvc;
using SignalRPrototype.Server.Utility;

namespace SignalRPrototype.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreController : ControllerBase
{
    [HttpGet]
    [Route("GetStores")]
    public IActionResult GetStores()
    {
        return Ok(ProductHelper.GetAllStores());
    }
}