using API.BussinesLogic.Services;
using API.BussinesLogic.Services.IServices;
using API.Data;
using API.Data.Repositories;
using API.Data.Repositories.IRepositories;
using API.BussinesLogic.Helpers;
using Microsoft.EntityFrameworkCore;

namespace API.BussinesLogic.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            return services.AddDbContext<DataContext>(opt => opt.UseSqlite(config.GetConnectionString("DefaultConnection")))
                           .Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")) 
                           .AddCors()                                                     
                           .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())                                                     
                           .AddScoped<LogUserActivity>()
                           .AddScoped<IUnitOfWork, UnitOfWork>()
                           .AddScoped<IPhotoService, PhotoService>()
                           .AddScoped<ITokenService, TokenServices>() 
                           .AddScoped<IAccountService, AccountService>() 
                           .AddScoped<ILikeService, LikeService>() 
                           .AddScoped<IMessageService, MessageService>() 
                           .AddScoped<IUserService, UserService>(); 
        }
    }
}