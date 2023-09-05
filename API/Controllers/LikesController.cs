using API.DTOs;
using API.BussinesLogic.Extensions;
using API.BussinesLogic.Helpers;
using Microsoft.AspNetCore.Mvc;
using API.BussinesLogic.Services.IServices;

namespace API.Controllers
{
    
    public class LikesController : BaseApiController
    {
        private readonly ILikeService _likeService;
       
        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;    
        }
       
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLikeAsync(string username)
        {
            await _likeService.AddLikeAsync(username, UserId);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes ([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = UserId;
            var users = await _likeService.GetUserLikes(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

    }
}