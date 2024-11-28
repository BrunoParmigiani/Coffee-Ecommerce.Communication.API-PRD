namespace Coffee_Ecommerce.Communication.API.Features.Report.Repository
{
    public interface IReportMessageRepository
    {
        public Task<bool> CreateTable(Guid reportId);
        public Task<bool> AddMessage(ReportMessage message);
        public Task<List<ReportMessage>> GetMessages(Guid reportId);
    }
}
