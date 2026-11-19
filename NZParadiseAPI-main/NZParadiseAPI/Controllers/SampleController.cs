using Microsoft.AspNetCore.Mvc;
using NZParadiseAPI.Model;
using Microsoft.Extensions.Options;

namespace NZParadiseAPI.Controllers
{
    // [ApiController]
    // [Route("api/[controller]")]
    // public class SampleController : ControllerBase
    // {
    //     private readonly IConfiguration _configuration;
    //     public SampleController(ApplicationDbContext context, IConfiguration configuration, IOptions<JwtSettings> jwtSettings)
    //     {
    //         _configuration = configuration;
    //     }

    //     [HttpGet]
    //     public object Index()
    //     {
    //         try
    //         {
    //             return new
    //             {
    //                 jwt_issuer = _configuration["Jwt:Issuer"],
    //                 jwt_audience = _configuration["Jwt:Audience"],
    //                 jwt_key = _configuration["Jwt:Key"],
    //                 db_connection = _configuration.GetConnectionString("NZParadiseDBConn"),
    //                 cors = _configuration["Cors:AllowedOrigins"],
    //                 azure_blob_connection = _configuration["AzureBlob:ConnectionString"],
    //                 azure_blob_container = _configuration["AzureBlob:ContainerName"],
    //             };
    //         }
    //         catch (Exception ex)
    //         {
    //             return new
    //             {
    //                 errorMsg = ex.Message,
    //                 detail = ex.ToString()
    //             };
    //         }
    //     }
    // }
}