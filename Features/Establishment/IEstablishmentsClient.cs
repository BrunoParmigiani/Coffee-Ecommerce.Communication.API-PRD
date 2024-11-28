using Coffee_Ecommerce.Communication.API.Features.Establishment.GetEstablishments;
using Refit;

namespace Coffee_Ecommerce.Communication.API.Features.Establishment
{
    public interface IEstablishmentsClient
    {
        [Get("/api/Establishment")]
        public Task<GetEstablishmentsResult> GetAll();
    }
}
