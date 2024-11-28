using Coffee_Ecommerce.Communication.API.Features.Establishment;
using Coffee_Ecommerce.Communication.API.Shared.Models;
using Coffee_Ecommerce.Communication.API.ThirdParty.GoogleCloud.MapsPlatform.Clients;
using Refit;

namespace Coffee_Ecommerce.Communication.API.Features.Locator
{
    public sealed class Locator : ILocator
    {
        private readonly IEstablishmentService _establishmentService;
        private readonly IDistanceMatrixClient _distanceMatrix = RestService.For<IDistanceMatrixClient>("https://maps.googleapis.com/maps/api");
        private readonly ILogger<Locator> _logger;
        private readonly string _accessKey;

        public Locator(IEstablishmentService establishmentClient, IConfiguration configuration, ILogger<Locator> logger)
        {
            _establishmentService = establishmentClient;
            _logger = logger;
            _accessKey = configuration["GoogleCloud"]!;
        }

        public async Task<LocatorResult> FindNearestEstablishment(string address)
        {
            List<EstablishmentInfo> establishmentsInfo = new List<EstablishmentInfo>();

            var establishments = await _establishmentService.GetEstablishments();

            if (establishments.Count == 0)
                return new LocatorResult { Error = new ApiError("No establishments were found") };

            foreach (var establishment in establishments)
            {
                try
                {
                    EstablishmentInfo info = new EstablishmentInfo(establishment);
                    await info.CalculateDistance(_distanceMatrix, address, _accessKey);
                    establishmentsInfo.Add(info);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }

            var min = establishmentsInfo.MinBy(info => info.Distance);
            return new LocatorResult { Data = min.GetEstablishment() };
        }
    }
}
