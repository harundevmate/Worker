using Microsoft.AspNetCore.Mvc;

namespace Worker.Controllers
{
	[Route("api/[controller]")]
	public class GrpcController : ControllerBase
	{
		[HttpGet("ping")]
		public IActionResult Ping()
		{
			return Ok("Ping");
		}
	}
}
