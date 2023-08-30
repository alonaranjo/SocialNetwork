using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.UserName)) 
            {
                return BadRequest("Username is taken");
            }

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();
            
            user.UserName = registerDto.UserName;
            user.PasswordHash =  hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            
            _context.Users.Add(user);
           await _context.SaveChangesAsync();
           return GetUserDto(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.Include(x => x.Photos).SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if(user == null)
            {
                return Unauthorized("Invalid UserName");
            }
            
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i = 0; i < computedPassword.Length; i++)
            {
                if(computedPassword[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }
           
            return GetUserDto(user);
        }

        private async Task<bool> UserExists(string username) 
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }

        private UserDto GetUserDto(AppUser user)
        {
            return new UserDto()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Gender = user.Gender
            };
        }
        
    }
}