using Microsoft.AspNetCore.Mvc;
using NZParadiseAPI.Model;
using NZParadiseAPI.Service;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Entities;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace NZParadiseAPI.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _service;
        public MessageController(ApplicationDbContext context)
        {
            _service = new MessageService(context);
        }

        [HttpPost("sendMessage")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] MessageSubmitData param)
        {
            try
            {
                return Ok(await _service.SendMessage(param));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject
                {
                    Message = "Unexpected error occurred",
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Data = ex.Message
                });
            }
        }

        [HttpGet("getMessages/{userID}")]
        [Authorize]
        public async Task<IActionResult> GetPropertyMessages(int userID)
        {
            try
            {
                return Ok(await _service.GetPropertyMessages(userID));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject
                {
                    Message = "Unexpected error occurred",
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Data = ex.Message
                });
            }
        }
    }
}
