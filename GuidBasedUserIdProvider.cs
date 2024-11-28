using Microsoft.AspNetCore.SignalR;

namespace Coffee_Ecommerce.Communication.API
{
    public class GuidBasedUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst("UserId")!.Value;
        }
    }
}
