namespace Coffee_Ecommerce.Communication.API.Features.Report
{
    public sealed class ReportMessage
    {
        public Guid Id { get; set; }
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
        public Guid Report { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}
