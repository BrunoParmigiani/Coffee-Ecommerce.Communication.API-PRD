using Coffee_Ecommerce.Communication.API.Features.Report.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text;

namespace Coffee_Ecommerce.Communication.API.Features.Report.Hubs
{
    [Authorize("Bearer")]
    public sealed class ReportHub : Hub
    {
        private readonly IReportMessageRepository _messaging;
        private readonly ILogger<ReportHub> logger;

        public ReportHub(ILogger<ReportHub> logger, IReportMessageRepository messaging)
        {
            this.logger = logger;
            _messaging = messaging;
        }

        public override Task OnConnectedAsync()
        {
            DisplayClientInformation();
            return base.OnConnectedAsync();
        }

        public async Task SendNotification(Guid establishmentId, Guid reportId)
        {
            string receiverId = establishmentId.ToString();

            await _messaging.CreateTable(reportId);
            logger.LogInformation($"Created table REPORT_{reportId}");

            logger.LogInformation($"New report notification sent to {receiverId}");

            await Clients.User(receiverId).SendAsync("ReceiveReportNotification");
        }

        public async Task SendMessage(ReportMessage message)
        {
            string receiverId = message.Receiver.ToString();
            string senderId = message.Sender.ToString();

            message.Time = DateTime.UtcNow;
            logger.LogInformation($"{message.Report}, {message.Content}");
            await _messaging.AddMessage(message);

            await Clients.User(senderId).SendAsync("ReceiveMessage", message);
            await Clients.User(receiverId).SendAsync("ReceiveMessage", message);
        }

        public void DisplayClientInformation()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("- - Report Hub - -");
            sb.AppendLine($"Connection identifier: {Context.UserIdentifier}");
            sb.AppendLine($"Connection identifier: {Context.ConnectionId}");
            sb.AppendLine($"Claim identifier: {Context.User.Identity.Name}");
            sb.AppendLine($"User id: {Context.User.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value}");
            sb.AppendLine();

            logger.LogInformation(sb.ToString());
        }
    }
}
