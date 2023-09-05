using API.Data.Repositories.IRepositories;
using AutoMapper;

namespace API.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;            
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);

        public ILikesRepository LikesRepository => new LikesRepository(_context);  

        public async Task<bool> CompleteAsync() =>  await _context.SaveChangesAsync() > 0;

        public bool HasChanges() => _context.ChangeTracker.HasChanges();
    }
}