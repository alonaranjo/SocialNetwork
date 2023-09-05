using API.BussinesLogic.Helpers;
using API.Data.Entities;
using API.DTOs;

namespace API.Data.Repositories.IRepositories
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targerUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes (LikesParams likesParam);
    }
}