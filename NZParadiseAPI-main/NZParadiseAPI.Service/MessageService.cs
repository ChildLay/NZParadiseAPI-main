
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZParadiseAPI.Model;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Concrete;
using NZParadiseAPI.Model.Entities;
using System.Runtime.InteropServices;


namespace NZParadiseAPI.Service
{
    public class MessageService
    {
        private readonly AuditLogService _logService;
        private readonly MessageRepository _messageRepo;
        public MessageService(ApplicationDbContext context)
        {
            _messageRepo = new MessageRepository(context);
            _logService = new AuditLogService(context);
        }

        public async Task<ResponseObject> GetPropertyMessages(int userID)
        {
            try
            {
                List<PropertyMessagesDTO> result = await _messageRepo.GetPropertyMessagesByUserID(userID);

                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Success,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseObject> SendMessage(MessageSubmitData data)
        {
            if (string.IsNullOrWhiteSpace(data.MessageDesc))
            {
                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Data = "Message cannot be empty"
                };
            }

            Messages msg = new Messages
            {
                FromUserID = data.FromUserID,
                PropertyID = data.PropertyID,
                MessageDesc = data.MessageDesc,
                CreatedDate = DateTime.Now
            };

            bool result = await _messageRepo.SendMessage(msg);

            await _logService.WriteLog(data.FromUserID, "Send message to prop: " + data.PropertyID + ", Msg: " + data.MessageDesc);

            return new ResponseObject
            {
                Status = result ? (int)CommonEnum.ResponseStatus.Success : (int)CommonEnum.ResponseStatus.Failed,
                Message = result ? "Success" : "Failed to send message to selected property"
            };
        }
    }
}