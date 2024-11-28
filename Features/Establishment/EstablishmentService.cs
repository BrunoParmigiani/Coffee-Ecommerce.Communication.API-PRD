
using Refit;

namespace Coffee_Ecommerce.Communication.API.Features.Establishment
{
    public sealed class EstablishmentService : IEstablishmentService
    {
        private readonly IEstablishmentsClient _client;
        private readonly ILogger<EstablishmentService> _logger;

        public EstablishmentService(IEstablishmentsClient client, ILogger<EstablishmentService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<EstablishmentEntity>> GetEstablishments()
        {
            try
            {
                var result = await _client.GetAll();

                return result.Data;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
