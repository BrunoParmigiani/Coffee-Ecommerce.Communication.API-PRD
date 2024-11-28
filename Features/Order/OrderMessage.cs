namespace Coffee_Ecommerce.Communication.API.Features.Order
{
    public sealed class OrderMessage
    {
        public Guid Id { get; set; }
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
        public Guid Order { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}
