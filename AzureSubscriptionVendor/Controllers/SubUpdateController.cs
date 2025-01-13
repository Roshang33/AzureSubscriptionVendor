using AzureSubscriptionVendor.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Text.Json;

namespace AzureSubscriptionVendor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateController : ControllerBase
    {
        private readonly MongoService _mongoService;

        public UpdateController(MongoService mongoService)
        {
            _mongoService = mongoService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateJson([FromBody] JsonElement jsonPayload)
        {
            try
            {
                if (!jsonPayload.TryGetProperty("subscriptionname", out JsonElement subscriptionNameElement))
                {
                    return BadRequest("The 'subscriptionname' field is required.");
                }
                string subscriptionName = subscriptionNameElement.GetString();

                var jsonString = jsonPayload.GetRawText();
                await _mongoService.UpdateJsonAsync(jsonString, subscriptionName);
                return Ok(new { Message = "JSON stored successfully" });
            }
            
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
