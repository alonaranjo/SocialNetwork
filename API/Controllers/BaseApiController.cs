using API.BussinesLogic.Extensions;
using API.BussinesLogic.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController: ControllerBase
    {
       public int UserId
       {
            get => User.GetUserId();
       }
       public string UserName
       {
            get => User.GetUserName();
       }

    }
}