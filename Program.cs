using Coffee_Ecommerce.Communication.API;
using Coffee_Ecommerce.Communication.API.Features.Announcement.Hubs;
using Coffee_Ecommerce.Communication.API.Features.Order.Hubs;
using Coffee_Ecommerce.Communication.API.Features.Order.Repository;
using Coffee_Ecommerce.Communication.API.Features.Report.Hubs;
using Coffee_Ecommerce.Communication.API.Features.Report.Repository;
using Coffee_Ecommerce.Communication.API.Identity;
using Coffee_Ecommerce.Communication.API.Infraestructure;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DependencyInjection.InitializeDependencies(builder);

var tokenConfigurations = new TokenConfiguration();
new ConfigureFromConfigurationOptions<TokenConfiguration>(builder.Configuration.GetSection("TokenConfigurations"))
    .Configure(tokenConfigurations);

string connectionString = builder.Configuration.GetConnectionString("postgre")!;
builder.Services.AddSingleton(_ =>
{
    DefaultTypeMap.MatchNamesWithUnderscores = true;
    return new NpgsqlDataSourceBuilder(connectionString);
});

builder.Services.AddCors();

builder.Services.AddSingleton(tokenConfigurations);

builder.Services.AddTransient<AuthHeaderHandler>();

builder.Services.AddSingleton<IUserIdProvider, GuidBasedUserIdProvider>();

builder.Services.AddScoped<IOrderMessageRepository, OrderMessageRepository>();

builder.Services.AddScoped<IReportMessageRepository, ReportMessageRepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenConfigurations.Issuer,
        ValidAudience = tokenConfigurations.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/orderhub") || path.StartsWithSegments("/reporthub") || path.StartsWithSegments("/announcementhub")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        },

    };
});

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});

builder.Services.AddSignalR();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(configs =>
    {
        configs.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<OrderHub>("/orderhub");
app.MapHub<ReportHub>("/reporthub");
app.MapHub<AnnouncementHub>("/announcementhub");
app.Run();
