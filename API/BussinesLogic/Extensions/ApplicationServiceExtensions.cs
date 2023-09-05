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
                           .AddCors()
                           .AddScoped<ITokenService, TokenServices>()                           
                           .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                           .Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"))
                           .AddScoped<IPhotoService, PhotoService>()
                           .AddScoped<LogUserActivity>()
                           .AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}