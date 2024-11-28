namespace Coffee_Ecommerce.Communication.API.Features.Order
{
    public sealed class OrderItem
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}