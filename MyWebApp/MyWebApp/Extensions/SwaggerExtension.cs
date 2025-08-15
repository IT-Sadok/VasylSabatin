using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerwithJwt(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebApp API", Version = "v1" });
            c.CustomSchemaIds(t => t.FullName);

            var bearer = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Bearer {token}"
            };
            c.AddSecurityDefinition("Bearer", bearer);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement { { bearer, Array.Empty<string>() } });
        });

        return services;
    }

    public static IApplicationBuilder UsePrettySwagger(this IApplicationBuilder app)
    {
        return app.UseSwaggerUI(o =>
        {
            o.DisplayRequestDuration();
            o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            o.DefaultModelsExpandDepth(-1);
            o.DisplayOperationId();
        });
    }
}