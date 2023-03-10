using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        public CommentController()
        {

        }
    }
}
