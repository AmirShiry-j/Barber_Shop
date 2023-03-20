using Application.BarberService.Command;
using Application.CommentService.Command;
using Application.CommentService.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Barber;
using WebApi.ModelsAndDtoes.Comment;
using WebApi.ModelsAndDtoes.Salon;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IAddCommendService _addCommendService;
        private readonly IGetCommentsService _getCommentsService;
        public CommentController(IAddCommendService addCommendService,
            IGetCommentsService getCommentsService)
        {
            _addCommendService = addCommendService;
            _getCommentsService = getCommentsService;
        }

        /// <summary>
        /// برای ثبت یک نظر در مورد آرایشگر توسط کاربر (Auth)
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

        /// <summary>
        /// برای دریافت نظرات ثبت شده برای آرایشگران یک سالن آرایشی یا یک آرایشگر
        /// </summary>
        /// <param name="searchCommentApiDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SearchCommentApiDto searchCommentApiDto)
        {
            //Validate 
            //One of the following two values ​​must have a value
            if (searchCommentApiDto.SalonId == null && searchCommentApiDto.BarberId == null)
            {
                return BadRequest("باید حداقل یکی از مقادیر، آیدی سالن آرایشی یا آیدی آرایشگر ارسال شود");
            }

            //map to model
            var model = new SearchCommentDto
            {
                CountInPage = searchCommentApiDto.CountInPage,
                Page = searchCommentApiDto.Page,
                SalonId = searchCommentApiDto.SalonId,
                BarberId = searchCommentApiDto.BarberId
            };

            //result service
            var resultService = await _getCommentsService.Execute(model);
            if (resultService.IsSuccess)
            {
                if (resultService?.Data?.Comments == null)
                {
                    return Ok();
                }

                //HATEAOS
                //Build url of image
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));

                foreach (var comment in resultService.Data.Comments)
                {
                    if (string.IsNullOrWhiteSpace(comment.ImageProfile) == false)
                    {
                        string imageUrl = domainName + "/Images/Profile/" + comment.ImageProfile;
                        comment.UrlImageProfile = imageUrl;
                    }

                }

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

    }
}
