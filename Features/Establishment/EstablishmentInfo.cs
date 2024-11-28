using Coffee_Ecommerce.Communication.API.ThirdParty.GoogleCloud.MapsPlatform;
using Coffee_Ecommerce.Communication.API.ThirdParty.GoogleCloud.MapsPlatform.Clients;

namespace Coffee_Ecommerce.Communication.API.Features.Establishment
{
    public sealed class EstablishmentInfo
    {
        private EstablishmentEntity Establishment;
        private DistanceMatrixResponse? DistanceData;
        public int Distance { get; private set; }

        public EstablishmentInfo(EstablishmentEntity establishment)
        {
            Establishment = establishment;
        }

        public async Task<bool> CalculateDistance(IDistanceMatrixClient client, string origin, string key)
        {
            try
            {
                DistanceData = await client.GetDistanceAsync(Establishment.GetFormattedAddress(), origin, key);
                Distance = DistanceData.GetDistance();

                return true;
            }
            catch (Exception)
            {
                Distance = int.MaxValue;
                throw;
            }
        }

        public EstablishmentEntity GetEstablishment()
        {
            return Establishment;
        }
    }
}
