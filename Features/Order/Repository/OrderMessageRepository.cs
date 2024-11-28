using Dapper;
using Npgsql;

namespace Coffee_Ecommerce.Communication.API.Features.Order.Repository
{
    public class OrderMessageRepository : IOrderMessageRepository
    {
        private readonly NpgsqlDataSourceBuilder _dataBuilder;

        public OrderMessageRepository(NpgsqlDataSourceBuilder dataBuilder)
        {
            _dataBuilder = dataBuilder;
        }

        public async Task<bool> AddMessage(OrderMessage message)
        {
            await using var dataSource = _dataBuilder.Build();
            using var connection = await dataSource.OpenConnectionAsync();
            message.Id = Guid.NewGuid();
            var query =
                $@"INSERT INTO ""ORDER_{message.Order}"" (""Id"", ""Sender"", ""Receiver"", ""Order"", ""Content"", ""Time"")
                VALUES (@Id, @Sender, @Receiver, @Order, @Content, @Time);";
            int affected = await connection.ExecuteAsync(query, message);

            if (affected < 1 )
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CreateTable(Guid orderId)
        {
            await using var dataSource = _dataBuilder.Build();
            using var connection = await dataSource.OpenConnectionAsync();
            var query =
                $@"CREATE TABLE ""ORDER_{orderId}"" (
                    ""Id"" UUID PRIMARY KEY,
                    ""Sender"" UUID NOT NULL,
                    ""Receiver"" UUID NOT NULL,
                    ""Order"" UUID NOT NULL,
                    ""Content"" VARCHAR NOT NULL,
                    ""Time"" TIMESTAMP NOT NULL
                )";

            await connection.ExecuteAsync(query, new { OrderId = orderId });

            return true;
        }

        public async Task<List<OrderMessage>> GetMessages(Guid orderId)
        {
            await using var dataSource = _dataBuilder.Build();
            using var connection = await dataSource.OpenConnectionAsync();
            var query = $@"SELECT * FROM ""ORDER_{orderId}""";

            var result = await connection.QueryAsync<OrderMessage>(query);

            return result.OrderBy(message => message.Time).ToList();
        }
    }
}
