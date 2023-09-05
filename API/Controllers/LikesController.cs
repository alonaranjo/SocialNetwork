using API.DTOs;
using API.Data.Entities;
using API.BussinesLogic.Extensions;
using API.BussinesLogic.Helpers;
using Microsoft.AspNetCore.Mvc;
using API.Data.Repositories.IRepositories;

namespace API.Controllers
{
    
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public LikesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;    
        }
       
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var likedUser = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);
            var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(UserId);

            if(likedUser == null) 
            {
                return NotFound();
            }

            if(sourceUser.UserName == username) 
            {
                return BadRequest("You cannot like yourself");
            }

            var userLike = await _unitOfWork.LikesRepository.GetUserLike(UserId, likedUser.Id);

            if(userLike != null)
            {
                return BadRequest("You already like this user");
            }

            sourceUser.LikedUsers.Add(new UserLike
            {
                SourceUserId = UserId,
                TargetUserId = likedUser.Id
            });

            if(await _unitOfWork.CompleteAsync())
            {
                return Ok();
            }

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes ([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = UserId;
            var users = await _unitOfWork.LikesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

    }
}