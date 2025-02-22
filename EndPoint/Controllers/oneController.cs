using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OneController : ControllerBase
    {
        // POST api/one
        [HttpPost]
        public IActionResult CreateItem([FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest("Item data is invalid.");
            }

            return Ok(item);  // Return HTTP 200 OK with the item data.
        }

    }

    // Sample Item model (you can modify this as needed)
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
