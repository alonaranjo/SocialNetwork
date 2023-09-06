using API.DTOs;
using API.BussinesLogic.Helpers;
using Microsoft.AspNetCore.Mvc;
using API.BussinesLogic.Services.IServices;

namespace API.Controllers
{
    public class MessageController : BaseApiController
    {       
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService )
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {           
            return Ok(await _messageService.CreateMessage(createMessageDto, UserName));
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            return Ok(await _messageService.GetMessagesForUser(messageParams, UserName, Response));
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            return Ok(await _messageService.GetMessageThread(username, UserName));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            await _messageService.DeleteMessage(id, UserName);
            return Ok();
        }

    }
}