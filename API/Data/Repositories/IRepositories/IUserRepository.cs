using API.DTOs;
using API.Data.Entities;
using API.BussinesLogic.Helpers;
using System.Linq.Expressions;

namespace API.Data.Repositories.IRepositories
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task AddUserAsync(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync(CancellationToken cancellationToken = default);        
        Task<AppUser> GetUserAsync(Expression<Func<AppUser, bool>> filter = null);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string userName, CancellationToken cancellationToken = default);
        Task<bool> UserExistsAsync(string username);
    }
}