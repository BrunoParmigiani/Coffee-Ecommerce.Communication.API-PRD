using Coffee_Ecommerce.Communication.API.Features.Order.Repository;
using Coffee_Ecommerce.Communication.API.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.Communication.API.Features.Order
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer", Roles = "commercial_place,customer")]
    public class OrderMessagesController : ControllerBase
    {
        private readonly IOrderMessageRepository _messaging;

        public OrderMessagesController(IOrderMessageRepository messaging)
        {
            _messaging = messaging;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(Guid orderId)
        {
            var result = await _messaging.GetMessages(orderId);

            var response = new Result<List<OrderMessage>>
            {
                Data = result
            };

            return Ok(response);
        }
    }
}
