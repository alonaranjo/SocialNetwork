using API.Data.Repositories.IRepositories;
using API.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var resultContext = await next();
           if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;
           
           var userID = resultContext.HttpContext.User.GetUserId();
           var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
           var user = await repo.UserRepository.GetUserByIdAsync(userID);
           user.LastActive = DateTime.UtcNow;
           await repo.CompleteAsync();
        }
    }
}