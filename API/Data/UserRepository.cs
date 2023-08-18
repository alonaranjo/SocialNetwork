using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context; 
            _mapper = mapper;          
        }
        
        public async Task<AppUser> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FindAsync(id, cancellationToken);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Photos).SingleOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Photos).ToListAsync(cancellationToken);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync(CancellationToken cancellationToken = default)
        {
             return await _context
                        .Users
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
        }

        public async Task<MemberDto> GetMemberAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _context
                        .Users
                        .Where(x => x.UserName == userName)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync(cancellationToken);
        }
    }
}