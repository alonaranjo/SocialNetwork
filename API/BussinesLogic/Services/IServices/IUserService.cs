
using API.BussinesLogic.Helpers;
using API.DTOs;

namespace API.BussinesLogic.Services.IServices
{
    public interface IUserService
    {
        Task<PagedList<MemberDto>> GetUsers(UserParams userParams, string userName, HttpResponse reponse); 
        Task<MemberDto> GetUser(string username);
        Task UpdateUser(MemberUpdateDto memberUpdateDto, string username);
        Task<PhotoDto> AddPhoto(IFormFile file, string userName);
        Task SetMainPhoto(int photoId, string userName);
        Task DeletePhoto(int photoId, string userName);
    }
}