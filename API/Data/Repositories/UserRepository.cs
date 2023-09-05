using API.DTOs;
using API.Data.Entities;
using API.BussinesLogic.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.Data.Repositories.IRepositories;
using System.Linq.Expressions;

namespace API.Data.Repositories
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
        
        public async Task<AppUser> GetUserAsync(Expression<Func<AppUser, bool>> filter = null)
        {        
            return await _context.Users.Include(x => x.Photos).SingleOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Photos).ToListAsync(cancellationToken);
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
            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

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

        public async Task AddUserAsync(AppUser user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }
    }
}