using API.DTOs;
using API.Entities;
using API.Helpers;
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

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams, CancellationToken cancellationToken = default)
        {
            var query = _context.Users.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUserName);
            query = query.Where(u => u.Gender == userParams.Gender);
            query = query.Where(u => u.DateOfBirth >= userParams.MinDob && u.DateOfBirth <= userParams.MaxDob);

            return await PagedList<MemberDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), 
                userParams.pageNumber, 
                userParams.PageSize);            
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