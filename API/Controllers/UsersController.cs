using API.DTOs;
using API.BussinesLogic.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.BussinesLogic.Services.IServices;

namespace API.Controllers
{
    [Authorize]
    public class UsersController: BaseApiController
    {
        private readonly IUserService _userService;        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }  

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams) 
        {
            return Ok(await _userService.GetUsers(userParams, UserName, Response));
        }     

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           return Ok(await _userService.GetUser(username));
        } 

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            await _userService.UpdateUser(memberUpdateDto, UserName);
            return Ok();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {            
            return CreatedAtAction(nameof(GetUser), new {username = UserName}, await _userService.AddPhoto(file, UserName));
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            await _userService.SetMainPhoto(photoId, UserName);
            return Ok();
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            await _userService.DeletePhoto(photoId, UserName);
            return Ok();
        }
    }
}