namespace Coffee_Ecommerce.Communication.API.Shared.Models
{
    public class Result<T>
    {
        public T? Data { get; set; }
        public ApiError? Error { get; set; }
    }
}
