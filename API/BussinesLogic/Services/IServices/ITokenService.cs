using API.Data.Entities;

namespace API.BussinesLogic.Services.IServices
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}