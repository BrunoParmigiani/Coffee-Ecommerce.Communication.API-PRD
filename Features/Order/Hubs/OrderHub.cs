using Coffee_Ecommerce.Communication.API.Features.Order.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text;

namespace Coffee_Ecommerce.Communication.API.Features.Order.Hubs
{
    [Authorize("Bearer")]
    public sealed class OrderHub : Hub
    {
        private readonly IOrderMessageRepository _messaging;
        private readonly ILogger<OrderHub> logger;

        public OrderHub(ILogger<OrderHub> logger, IOrderMessageRepository messaging)
        {
            this.logger = logger;
            _messaging = messaging;
        }

        public override Task OnConnectedAsync()
        {
            DisplayClientInformation();
            return base.OnConnectedAsync();
        }

        public async Task SendOrder(OrderData order)
        {
            string receiverId = order.EstablishmentId.ToString();
            await Clients.User(receiverId).SendAsync("ReceiveOrder", order);
        }

        public async Task SendResponse(Guid orderId, Guid userId, bool accepted)
        {
            if (accepted)
            {
                await _messaging.CreateTable(orderId);
            }

            await Clients.User(userId.ToString()).SendAsync("ReceiveResponse", accepted);
        }

        public async Task SendMessage(OrderMessage message)
        {
            string receiverId = message.Receiver.ToString();
            string senderId = message.Sender.ToString();

            message.Time = DateTime.UtcNow;
            logger.LogInformation($"{message.Order}, {message.Content}");
            await _messaging.AddMessage(message);

            await Clients.User(senderId).SendAsync("ReceiveMessage", message);
            await Clients.User(receiverId).SendAsync("ReceiveMessage", message);
        }

        public void DisplayClientInformation()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine($"Connection identifier: {Context.UserIdentifier}");
            sb.AppendLine($"Connection identifier: {Context.ConnectionId}");
            sb.AppendLine($"Claim identifier: {Context.User.Identity.Name}");
            sb.AppendLine($"User id: {Context.User.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value}");
            sb.AppendLine();

            logger.LogInformation(sb.ToString());
        }
    }
}
