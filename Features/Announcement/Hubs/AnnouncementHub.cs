using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Coffee_Ecommerce.Communication.API.Features.Announcement.Hubs
{
    [Authorize("Bearer")]
    public sealed class AnnouncementHub : Hub
    {
        private readonly ILogger<AnnouncementHub> logger;

        public AnnouncementHub(ILogger<AnnouncementHub> logger)
        {
            this.logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            string role = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
            string connectionId = Context.ConnectionId;

            logger.LogInformation($"- - Announcements Hub - -\n{role}\n{connectionId}");

            await Groups.AddToGroupAsync(connectionId, role);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string role = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
            string connectionId = Context.ConnectionId;

            await Groups.RemoveFromGroupAsync(connectionId, role);
        }

        public async Task SendNotification(string[] groups)
        {
            await Clients.Groups(groups).SendAsync("AnnouncementNotification");
        }
    }
}
