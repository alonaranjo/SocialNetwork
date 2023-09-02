using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task InitDbAsync(WebApplication app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var context =  scope.ServiceProvider.GetService<DataContext>();
                await context.Database.MigrateAsync();
                await SeedUsers(context);
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration");
            }
        }

        private  static async Task SeedUsers(DataContext context)
        {
            if(await context.Users.AnyAsync())
            {
                return;
            }

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            foreach(var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
        }
    }

}