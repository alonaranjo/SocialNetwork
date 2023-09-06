
using API.BussinesLogic.Helpers;
using API.DTOs;

namespace API.BussinesLogic.Services.IServices
{
    public interface IMessageService
    {
        Task<MessageDto> CreateMessage(CreateMessageDto createMessageDto, string userName);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams, string userName, HttpResponse response);
        Task<IEnumerable<MessageDto>> GetMessageThread(string username,  string currentUserName);
        Task DeleteMessage(int id, string currentUserName);
    }
}