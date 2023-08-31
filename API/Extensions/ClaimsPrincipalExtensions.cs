
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
           var resultID = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           if(string.IsNullOrEmpty(resultID))
           {
                return 0;
           }
           return int.Parse(resultID);
        }  

        public static string GetUserName(this ClaimsPrincipal user)
        {
           return user.FindFirst(ClaimTypes.Name)?.Value;
        }   
    }
}