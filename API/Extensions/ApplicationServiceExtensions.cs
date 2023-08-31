
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
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
                           .AddScoped<LogUserActivity>();
        }
    }
}