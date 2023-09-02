using API.BussinesLogic.Services;
using API.BussinesLogic.Services.IServices;
using API.Data;
using API.Data.Repositories;
using API.Data.Repositories.IRepositories;
using API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            return services.AddDbContext<DataContext>(opt => opt.UseSqlite(config.GetConnectionString("DefaultConnection")))
                           .AddCors()
                           .AddScoped<ITokenService, TokenServices>()
                           .AddScoped<IUserRepository, UserRepository>()
                           .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                           .Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"))
                           .AddScoped<IPhotoService, PhotoService>()
                           .AddScoped<LogUserActivity>()
                           .AddScoped<ILikesRepository, LikesRepository>()
                           .AddScoped<IMessageRepository, MessageRepository>();
        }
    }
}