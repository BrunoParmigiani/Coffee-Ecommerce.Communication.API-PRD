namespace Coffee_Ecommerce.Communication.API.Shared.Models
{
    public sealed class ApiError(string? message)
    {
        public string? Message { get; private set; } = message;
    }
}