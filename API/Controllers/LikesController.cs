using API.DTOs;
using API.Data.Entities;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;
using API.Data.Repositories.IRepositories;

namespace API.Controllers
{
    
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
       
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;            
        }
       
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var likedUser = await _userRepository.GetUserByUserNameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(UserId);

            if(likedUser == null) 
            {
                return NotFound();
            }

            if(sourceUser.UserName == username) 
            {
                return BadRequest("You cannot like yourself");
            }

            var userLike = await _likesRepository.GetUserLike(UserId, likedUser.Id);

            if(userLike != null)
            {
                return BadRequest("You already like this user");
            }

            sourceUser.LikedUsers.Add(new UserLike
            {
                SourceUserId = UserId,
                TargetUserId = likedUser.Id
            });

            if(await _userRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes ([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = UserId;
            var users = await _likesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

    }
}