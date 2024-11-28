namespace Coffee_Ecommerce.Communication.API.Features.Locator
{
    public interface ILocator
    {
        public Task<LocatorResult> FindNearestEstablishment(string address);
    }
}
