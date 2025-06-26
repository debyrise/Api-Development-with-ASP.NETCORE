using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        [HttpGet("hello")]
        public IActionResult HelloWorld()
        {
            return Ok(new {title = "Hello, World"});
        }
    }
}
