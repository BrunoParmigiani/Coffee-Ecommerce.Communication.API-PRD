using Coffee_Ecommerce.Communication.API.Features.Establishment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.Communication.API.Features.Locator
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class LocatorController : ControllerBase
    {
        private readonly IEstablishmentsClient _client;
        private readonly ILocator _locator;

        public LocatorController(IEstablishmentsClient client, ILocator locator)
        {
            _client = client;
            _locator = locator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _client.GetAll();

            return Ok(result);
        }

        [HttpGet("/nearest")]
        public async Task<IActionResult> GetNearest()
        {
            string address = User.Claims.FirstOrDefault(c => c.Type == "Address")!.Value;

            var result = await _locator.FindNearestEstablishment(address);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
