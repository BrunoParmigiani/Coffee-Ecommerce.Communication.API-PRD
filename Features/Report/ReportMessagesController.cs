using Coffee_Ecommerce.Communication.API.Features.Report.Repository;
using Coffee_Ecommerce.Communication.API.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.Communication.API.Features.Report
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer", Roles = "commercial_place,customer")]
    public class ReportMessagesController : ControllerBase
    {
        private readonly IReportMessageRepository _messaging;

        public ReportMessagesController(IReportMessageRepository messaging)
        {
            _messaging = messaging;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(Guid reportId)
        {
            var result = await _messaging.GetMessages(reportId);

            var response = new Result<List<ReportMessage>>
            {
                Data = result
            };

            return Ok(response);
        }
    }
}
