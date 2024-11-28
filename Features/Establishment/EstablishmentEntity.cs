namespace Coffee_Ecommerce.Communication.API.Features.Establishment
{
    public sealed class EstablishmentEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CNPJ { get; set; }
        public Guid AdministratorId { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Complement { get; set; }

        public string GetFormattedAddress()
        {
            return $"{Street} - {District}, {City} - {State}, {PostalCode}, {Country}";
        }
    }
}