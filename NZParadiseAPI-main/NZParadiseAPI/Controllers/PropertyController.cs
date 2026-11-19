using Microsoft.AspNetCore.Mvc;
using NZParadiseAPI.Model;
using NZParadiseAPI.Service;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Entities;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;

namespace NZParadiseAPI.Controllers
{
    [ApiController]
    [Route("api/property")]
    public class PropertyController : ControllerBase
    {
        PropertyService _service;
        public PropertyController(ApplicationDbContext context, AzureBlobService blobService)
        {
            _service = new PropertyService(context, blobService);
        }

        // [HttpGet]
        // public string Index()
        // {
        //     return "Property Controller; Version 1.0.0.1";
        // }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewProperty([FromForm] Properties propertyData, [FromForm] List<IFormFile> files)
        {
            try
            {
                return Ok(await _service.RegisterNewProperty(propertyData, files));
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

        [HttpPut("{propertyID}")]
        [Authorize]
        public async Task<IActionResult> UpdateProperty(int propertyID, [FromForm] Properties propertyData, [FromForm] List<IFormFile> files, [FromForm] List<int> removedFileIDs)
        {
            try
            {
                propertyData.PropertyID = propertyID;
                return Ok(await _service.UpdateProperty(propertyData, files, removedFileIDs));
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

        [HttpGet("{propertyID}")]
        public async Task<IActionResult> GetPropertyDetails(int propertyID)
        {
            try
            {
                return Ok(await _service.GetPropertyDetails(propertyID));
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

        [HttpGet("getlist")]
        public async Task<IActionResult> GetPropertyList([FromQuery] PropertyFilter filter)
        {
            try
            {
                return Ok(await _service.GetPropertyList(filter));
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

        [HttpGet("metadata")]
        public async Task<IActionResult> GetReferences()
        {
            try
            {
                return Ok(await _service.GetReferences());
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

        [HttpPost("toggleWishlist")]
        [Authorize]
        public async Task<IActionResult> ToggleWishlist([FromBody] ToggleWishlistData param)
        {
            try
            {
                return Ok(await _service.ToggleWishlist(param.PropertyID, param.UserID, param.AddToWishlist));
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

        [HttpGet("summary/{userID}")]
        [Authorize]
        public async Task<IActionResult> GetDashboardData(int userID)
        {
            try
            {
                return Ok(await _service.GetDashboardData(userID));
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

        [HttpDelete("{propertyID}/user/{userID}")]
        [Authorize]
        public async Task<IActionResult> DeleteProperty(int propertyID, int userID)
        {
            try
            {
                return Ok(await _service.DeleteProperty(propertyID, userID));
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
