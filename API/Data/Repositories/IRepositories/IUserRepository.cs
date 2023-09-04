using API.DTOs;
using API.Data.Entities;
using API.Helpers;

namespace API.Data.Repositories.IRepositories
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync(CancellationToken cancellationToken = default);
        Task<AppUser> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<AppUser> GetUserByUserNameAsync(string username, CancellationToken cancellationToken = default);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams, CancellationToken cancellationToken = default);
        Task<MemberDto> GetMemberAsync(string userName, CancellationToken cancellationToken = default);
    }
}