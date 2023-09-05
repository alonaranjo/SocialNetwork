using API.BussinesLogic.Helpers;
using API.BussinesLogic.Services.IServices;
using API.Data.Entities;
using API.Data.Repositories.IRepositories;
using API.DTOs;

namespace API.BussinesLogic.Services
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LikeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }
        public async Task AddLikeAsync(string username, int UserId)
        {
            var likedUser = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == username);
            var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(UserId);

            if(likedUser == null) 
            {
                throw new Exception($"Username not found");
            }

            if(sourceUser.UserName == username) 
            {
                throw new Exception($"You cannot like yourself");
            }

            var userLike = await _unitOfWork.LikesRepository.GetUserLike(UserId, likedUser.Id);

            if(userLike != null)
            {
                throw new Exception("You already like this user");
            }

            sourceUser.LikedUsers.Add(new UserLike
            {
                SourceUserId = UserId,
                TargetUserId = likedUser.Id
            });

            if(!await _unitOfWork.CompleteAsync())
            {
                throw new Exception("Failed to like user");
            }
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            return await _unitOfWork.LikesRepository.GetUserLikes(likesParams);
        }
    }
}