using Refit;

namespace Coffee_Ecommerce.Communication.API.ThirdParty.GoogleCloud.MapsPlatform.Clients
{
    public interface IDistanceMatrixClient
    {
        [Get("/distancematrix/json?destinations={destination}&origins={origin}&units=universal&key={accessKey}")]
        public Task<DistanceMatrixResponse> GetDistanceAsync(string destination, string origin, string accessKey);
    }
}
