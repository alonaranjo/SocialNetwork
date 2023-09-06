
using API.BussinesLogic.Helpers;
using API.DTOs;

namespace API.BussinesLogic.Services.IServices
{
    public interface ILikeService
    {
        Task AddLikeAsync(string username, int UserId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams, int userId, HttpResponse response);
    }
}