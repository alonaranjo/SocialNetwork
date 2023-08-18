using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController: BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }  

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(CancellationToken cancellationToken = default) 
        {
            var users = await _userRepository.GetMembersAsync(cancellationToken);
            return Ok(users);
        }     

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username, CancellationToken cancellationToken = default)
        {
           return await _userRepository.GetMemberAsync(username, cancellationToken);
        } 
    }
}