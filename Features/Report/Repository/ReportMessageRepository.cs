using Dapper;
using Npgsql;

namespace Coffee_Ecommerce.Communication.API.Features.Report.Repository
{
    public class ReportMessageRepository : IReportMessageRepository
    {
        private readonly NpgsqlDataSourceBuilder _dataBuilder;

        public ReportMessageRepository(NpgsqlDataSourceBuilder dataBuilder)
        {
            _dataBuilder = dataBuilder;
        }

        public async Task<bool> AddMessage(ReportMessage message)
        {
            await using var dataSource = _dataBuilder.Build();
            using var connection = await dataSource.OpenConnectionAsync();
            message.Id = Guid.NewGuid();
            var query =
                $@"INSERT INTO ""REPORT_{message.Report}"" (""Id"", ""Sender"", ""Receiver"", ""Report"", ""Content"", ""Time"")
                VALUES (@Id, @Sender, @Receiver, @Report, @Content, @Time);";
            int affected = await connection.ExecuteAsync(query, message);

            if (affected < 1 )
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CreateTable(Guid reportId)
        {
            await using var dataSource = _dataBuilder.Build();
            using var connection = await dataSource.OpenConnectionAsync();
            var query =
                $@"CREATE TABLE ""REPORT_{reportId}"" (
                    ""Id"" UUID PRIMARY KEY,
                    ""Sender"" UUID NOT NULL,
                    ""Receiver"" UUID NOT NULL,
                    ""Report"" UUID NOT NULL,
                    ""Content"" VARCHAR NOT NULL,
                    ""Time"" TIMESTAMP NOT NULL
                )";

            await connection.ExecuteAsync(query, new { reportId = reportId });

            return true;
        }

        public async Task<List<ReportMessage>> GetMessages(Guid reportId)
        {
            await using var dataSource = _dataBuilder.Build();
            using var connection = await dataSource.OpenConnectionAsync();
            var query = $@"SELECT * FROM ""REPORT_{reportId}""";

            var result = await connection.QueryAsync<ReportMessage>(query);

            return result.OrderBy(message => message.Time).ToList();
        }
    }
}
