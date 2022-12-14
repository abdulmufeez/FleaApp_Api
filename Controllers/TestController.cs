using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleaApp_Api.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet("admin")]
        [Authorize(Policy = "RequireAdminRole")]
        public ActionResult GetAdmin()        
        {
            return BadRequest("Only admin can see this");
        }

        [HttpGet("shopkeeper")]
        [Authorize(Policy = "RequireShopKeeperRole")]
        public ActionResult GetShop()        
        {
            //return BadRequest("Only shopleeper can see this");
            throw new Exception();
        }
    }
}