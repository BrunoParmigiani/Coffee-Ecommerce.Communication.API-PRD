using System.Text;

namespace Coffee_Ecommerce.Communication.API.Features.Order
{
    public sealed class OrderData
    {
        public Guid UserId { get; set; }
        public int PaymentMethod { get; set; }
        public Guid EstablishmentId { get; set; }
        public int DeliveryTime { get; set; }
        public OrderItem[] Items { get; set; }
        public bool DeniedOrder { get; set; }
        public string? DeniedReason { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserComplement { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"User: {UserId}");
            sb.AppendLine($"Payment: {PaymentMethod}");
            sb.AppendLine($"Establishment: {EstablishmentId}");
            sb.AppendLine($"- - Items - -");

            foreach (var item in Items)
            {
                sb.AppendLine($"{item.Name}, R${item.Price}, {item.Quantity}");
            }

            sb.AppendLine();

            return sb.ToString();
        }
    }
}
