using API.BussinesLogic.Extensions;
using API.BussinesLogic.Helpers;
using API.BussinesLogic.Services.IServices;
using API.Data.Entities;
using API.Data.Repositories.IRepositories;
using API.DTOs;
using AutoMapper;

namespace API.BussinesLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<PhotoDto> AddPhoto(IFormFile file, string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == userName);
            if(user == null)
            {
                throw new Exception($"User {userName} not found");
            }

            var result = await _photoService.AddPhotoAsync(file);
            if(result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }            

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = user.Photos.Count == 0
            };

            user.Photos.Add(photo);

            if(!await _unitOfWork.CompleteAsync())
            {
                throw new Exception("Problem adding photo");                
            }   

            return _mapper.Map<PhotoDto>(photo);        
        }

        public async Task DeletePhoto(int photoId, string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == userName);
            if(user == null)
            {
                throw new Exception($"User {userName} not found");
            }

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if(photo == null)
            {
                throw new Exception($"Photo not found");
            }
            if(photo.IsMain)
            {
                throw new Exception("You cannot delete your main photo");
            }

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null)
                {
                    throw new Exception(result.Error.Message);
                }
            }

            user.Photos.Remove(photo);
            if(!await _unitOfWork.CompleteAsync())
            {
                throw new Exception("Problem deleting the main photo");
            }
        }

        public async Task<MemberDto> GetUser(string username)
        {
            return await _unitOfWork.UserRepository.GetMemberAsync(username);
        }

        public async Task<PagedList<MemberDto>> GetUsers(UserParams userParams, string userName, HttpResponse reponse)
        {
            var currentUser = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == userName);
            userParams.CurrentUserName = currentUser.UserName;

            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);
            reponse.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return users;
        }

        public async Task SetMainPhoto(int photoId, string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == userName);
            if(user == null)
            {
                throw new Exception($"User {userName} not found");
            }

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if(photo == null)
            {
                throw new Exception($"Photo not found");
            }            

            var mainPhotos = user.Photos.Where(x => x.IsMain).ToList();
            foreach(var p in mainPhotos)
            {
                p.IsMain = false;
            }

            photo.IsMain = true;

            if(!await _unitOfWork.CompleteAsync())
            {
                throw new Exception("Problem setting the main photo");
            }
        }

        public async Task UpdateUser(MemberUpdateDto memberUpdateDto, string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == username);
            if(user == null)
            {
                throw new Exception($"User {username} not found");
            }

            _mapper.Map(memberUpdateDto, user);

            if(!await _unitOfWork.CompleteAsync())
            {
                throw new Exception("Failed to update user");
            }            
        }
    }
}