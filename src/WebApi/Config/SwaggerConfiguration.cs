using Microsoft.OpenApi.Models;

namespace WebApi.Config;

public static class SwaggerConfiguration
{
    public static void ConfigureSwaggerApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ws Transacciones SPI COOPMEGO",
                Version = "v1",
                Description = "Servicio de transacciones SPI COOPMEGO",
            });
            c.AddSecurityDefinition("Authorization-Mego", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization-Mego",
                Type = SecuritySchemeType.ApiKey,
                Description = "Ingrese su clave de API aquí. Ejemplo: 'Auth-Mego 12345'"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization-Mego"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}