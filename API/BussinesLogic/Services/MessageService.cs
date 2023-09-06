
using API.BussinesLogic.Extensions;
using API.BussinesLogic.Helpers;
using API.BussinesLogic.Services.IServices;
using API.Data.Entities;
using API.Data.Repositories.IRepositories;
using API.DTOs;
using AutoMapper;

namespace API.BussinesLogic.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;            
        }
        public async Task<MessageDto> CreateMessage(CreateMessageDto createMessageDto, string userName)
        {
            if(userName == createMessageDto.RecipientUsername.ToLower())
            {
                throw new Exception("You cannot send messages to yourself");
            }

            var sender = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == userName);
            var recipient = await _unitOfWork.UserRepository.GetUserAsync(x => x.UserName == createMessageDto.RecipientUsername);

            if(recipient == null)
            {
                throw new Exception("Not found");
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
                return _mapper.Map<MessageDto>(message);
            }
            throw new Exception("Failed to send message");
        }

        public async Task DeleteMessage(int id, string currentUserName)
        {
            var message = await _unitOfWork.MessageRepository.GetMessage(id); 
            if(message.SenderUserName != currentUserName && message.RecipientUserName != currentUserName)
            {
                throw new Exception("Unauthorized");
            }           

            if(message.SenderUserName == currentUserName)
            {
                message.SenderDeleted = true;
            }

            if(message.RecipientUserName == currentUserName)
            {
                message.RecipientDeleted = true;
            }

            if(message.SenderDeleted && message.RecipientDeleted)
            {
                _unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if(!await _unitOfWork.CompleteAsync())
            {
                throw new Exception("Problem deleting the message");
            }
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams, string userName, HttpResponse response)
        {
            messageParams.Username = userName;
            var messages = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);
            response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));
            return messages;
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string username, string currentUserName)
        {
            var response = await _unitOfWork.MessageRepository.GetMessageThread(currentUserName, username);
            if(_unitOfWork.HasChanges())
            {
                if(await _unitOfWork.CompleteAsync())
                {
                    return response;
                }
                throw new Exception("Problem getting the messages");
            }
            return response;
        }
    }
}