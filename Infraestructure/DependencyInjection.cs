using Coffee_Ecommerce.Communication.API.Features.Establishment;
using Coffee_Ecommerce.Communication.API.Features.Locator;
using Refit;

namespace Coffee_Ecommerce.Communication.API.Infraestructure
{
    public static class DependencyInjection
    {
        public static void InitializeDependencies(WebApplicationBuilder builder)
        {
            string api = builder.Configuration.GetValue<string>("Api");

            builder.Services.AddRefitClient<IEstablishmentsClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(api))
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    // War crime
                    var handler = new HttpClientHandler();

                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                    return handler;
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();

            builder.Services.AddSingleton<IEstablishmentService, EstablishmentService>();
            builder.Services.AddSingleton<ILocator, Locator>();
        }
    }
}
