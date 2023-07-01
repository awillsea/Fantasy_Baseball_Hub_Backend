using Microsoft.AspNetCore.Mvc;

namespace Fantasy_Baseball_Hub_Backend.Controllers
{
    [Route("player")]
    [ApiController]
    public class playerController : Controller
    {
        private readonly IConfiguration? _configuration;
        public playerController(IConfiguration? configuration)
        {
            _configuration = configuration;
        }
        //[HttpGet]
        //public JsonResult Get()
        //{
        //    string query
        //}
    }
}
