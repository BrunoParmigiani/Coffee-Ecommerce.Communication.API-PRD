namespace Coffee_Ecommerce.Communication.API.Features.Establishment
{
    public interface IEstablishmentService
    {
        public Task<List<EstablishmentEntity>> GetEstablishments();
    }
}
