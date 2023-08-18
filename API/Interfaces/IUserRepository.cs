using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync(CancellationToken cancellationToken = default);
        Task<AppUser> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<AppUser> GetUserByUserNameAsync(string username, CancellationToken cancellationToken = default);
        Task<IEnumerable<MemberDto>> GetMembersAsync(CancellationToken cancellationToken = default);
        Task<MemberDto> GetMemberAsync(string userName, CancellationToken cancellationToken = default);
    }
}