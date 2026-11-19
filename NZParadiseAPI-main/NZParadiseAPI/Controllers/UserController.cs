using Microsoft.AspNetCore.Mvc;
using NZParadiseAPI.Model;
using NZParadiseAPI.Service;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Entities;
using Azure.Core.Pipeline;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NZParadiseAPI.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        UsersService _service;
        private readonly IConfiguration _configuration;
        public UserController(ApplicationDbContext context, IConfiguration configuration, IOptions<JwtSettings> jwtSettings)
        {
            _service = new UsersService(context, jwtSettings.Value);
            _configuration = configuration;
        }

        // [HttpGet]
        // public string Index()
        // {
        //     return "User Controller; Version 1.0.0.2";
        // }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser(Users aUser)
        {
            try
            {
                return Ok(await _service.RegisterNewUser(aUser));
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

        [HttpPut("{userID}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int userID, [FromBody] Users aUser)
        {
            try
            {
                aUser.UserID = userID;
                return Ok(await _service.UpdateUser(aUser));
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

        [HttpPost("signin")]
        public async Task<IActionResult> UserSignIn([FromBody] SignInModel model)
        {
            try
            {
                return Ok(await _service.ValidateLogin(model.Email, model.Password));
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

        [HttpGet("{userID}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int userID)
        {
            try
            {
                return Ok(await _service.GetUserByID(userID));
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

        [HttpGet("getList/{adminUserID}")]
        [Authorize]
        public async Task<IActionResult> GetUserList(int adminUserID)
        {
            try
            {
                return Ok(await _service.GetUserList(adminUserID));
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
