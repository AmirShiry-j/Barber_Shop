using Application.BarberService.Command;
using Application.CommentService.Command;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Barber;
using WebApi.ModelsAndDtoes.Comment;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IAddCommendService _addCommendService;
        public CommentController(IAddCommendService addCommendService)
        {
            _addCommendService = addCommendService;
        }

        /// <summary>
        /// برای ثبت یک نظر توسط کاربر (Auth)
        /// </summary>
        /// <param name="createCommentDto"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateCommentApiDto createCommentDto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //map
            var model = new CreateCommentDto
            {
                BarberId = createCommentDto.BarberId,
                SuggestionMode = (SuggestionModeDto)createCommentDto.SuggestionMode,
                Text = createCommentDto.Text,
                UserId = userId
            };

            //send for service
            var resultService = await _addCommendService.Execute(model);

            if (resultService.IsSuccess)
            {
                return Created("", null);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
    }
}
