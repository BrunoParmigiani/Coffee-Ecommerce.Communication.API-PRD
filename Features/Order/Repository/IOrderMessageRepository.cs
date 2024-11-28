namespace Coffee_Ecommerce.Communication.API.Features.Order.Repository
{
    public interface IOrderMessageRepository
    {
        public Task<bool> CreateTable(Guid orderId);
        public Task<bool> AddMessage(OrderMessage message);
        public Task<List<OrderMessage>> GetMessages(Guid orderId);
    }
}
