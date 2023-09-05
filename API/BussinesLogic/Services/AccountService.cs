using System.Security.Cryptography;
using System.Text;
using API.BussinesLogic.Services.IServices;
using API.Data;
using API.Data.Entities;
using API.Data.Repositories.IRepositories;
using API.DTOs;
using AutoMapper;

namespace API.BussinesLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(ITokenService tokenService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        
        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if(await _unitOfWork.UserRepository.UserExistsAsync(registerDto.UserName)) 
            {
                throw new Exception($"Username is taken");
            }

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();
            
            user.UserName = registerDto.UserName;
            user.PasswordHash =  hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            
            await _unitOfWork.UserRepository.AddUserAsync(user);
            if(await _unitOfWork.CompleteAsync())
            {
                return GetUserDto(user);
            }
            throw new Exception($"Error registering the new user.");
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == loginDto.UserName);            
            if(user == null)
            {
                throw new Exception("Invalid UserName");
            }
            
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i = 0; i < computedPassword.Length; i++)
            {
                if(computedPassword[i] != user.PasswordHash[i])
                {
                    throw new Exception("Invalid Password");
                }
            }
           
            return GetUserDto(user);
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