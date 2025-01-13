using AzureSubscriptionVendor.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Text.Json;

namespace AzureSubscriptionVendor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsertController : ControllerBase
    {
        private readonly MongoService _mongoService;

        public InsertController(MongoService mongoService)
        {
            _mongoService = mongoService;
        }

        [HttpPost]
        public async Task<IActionResult> PostJson([FromBody] JsonElement jsonPayload)
        {
            try
            {
                var jsonString = jsonPayload.GetRawText();
                await _mongoService.StoreJsonAsync(jsonString);
                return Ok(new { Message = "JSON stored successfully" });
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return Conflict("The 'subscriptionname' value must be unique.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
