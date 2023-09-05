
using API.Data.Entities;
using API.Data.Repositories.IRepositories;
using API.DTOs;
using API.BussinesLogic.Extensions;
using API.BussinesLogic.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessageController : BaseApiController
    {       
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public MessageController(IUnitOfWork unitOfWork, IMapper mapper )
        {
            _unitOfWork = unitOfWork;          
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto, CancellationToken cancellationToken = default)
        {           
            if(UserName == createMessageDto.RecipientUsername.ToLower())
            {
                return BadRequest("You cannot send messages to yourself");
            }

            var sender = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == UserName);
            var recipient = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == createMessageDto.RecipientUsername);

            if(recipient == null)
            {
                return NotFound();
            }

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            if(await _unitOfWork.CompleteAsync())
            {
                return Ok(_mapper.Map<MessageDto>(message));
            }

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = UserName;
            var messages = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));
            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var response = await _unitOfWork.MessageRepository.GetMessageThread(UserName, username);
            if(_unitOfWork.HasChanges())
            {
                if(await _unitOfWork.CompleteAsync())
                {
                    return Ok(response);
                }
                return BadRequest("Problem getting the messages");
            }
            return Ok(response);           
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetMessage(id); 
            if(message.SenderUserName != UserName && message.RecipientUserName != UserName)
            {
                return Unauthorized();
            }           

            if(message.SenderUserName == UserName)
            {
                message.SenderDeleted = true;
            }

            if(message.RecipientUserName == UserName)
            {
                message.RecipientDeleted = true;
            }

            if(message.SenderDeleted && message.RecipientDeleted)
            {
                _unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if(await _unitOfWork.CompleteAsync())
            {
                return Ok();
            }

            return BadRequest("Problem deleting the message");

        }

    }
}