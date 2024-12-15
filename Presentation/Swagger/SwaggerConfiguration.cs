using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Presentation.Swagger;

public static class SwaggerConfiguration
{

    public static IServiceCollection AddSwaggerPreConfigured(this IServiceCollection services, Action<SwaggerOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);

        ArgumentNullException.ThrowIfNull(setupAction);

        services.Configure(setupAction);

        var options = services.BuildServiceProvider().GetRequiredService<IOptions<SwaggerOptions>>();

        // Register the Swagger generator, defining one or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.OperationFilter<SwaggerFileOperationFilter>();
            c.SwaggerDoc(options.Value.Version, new OpenApiInfo
            {
                Title = options.Value.Title,
                Version = options.Value.Version,
                Description = options.Value.Description,
                TermsOfService = new Uri("http://swagger.io/terms/"),
                Contact = new OpenApiContact
                {
                    Name = options.Value.ContactName,
                    Email = options.Value.ContactEmail
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });

            c.AddServer(new OpenApiServer
            {
                Url = options.Value.ServerUrl,
                Description = options.Value.Description
            });

            //c.AddSecurityDefinition(
            //    "Bearer",
            //    new OpenApiSecurityScheme()
            //    {
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer",
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Description =
            //            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            //    }
            //);

            //c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //            }
            //        }, []
            //    }
            //});
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerPreConfigured(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var options = app.ApplicationServices.GetRequiredService<IOptions<SwaggerOptions>>();
        if (options == null) throw new ArgumentNullException(nameof(options));

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.ConfigObject = new ConfigObject
            {
                ShowCommonExtensions = true
            };
            c.SupportedSubmitMethods([SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete, SubmitMethod.Patch]);
            c.SwaggerEndpoint("/swagger/" + options.Value.Version + "/swagger.json", options.Value.ApiDocs);
            //redirect root url to swagger ui
            //c.RoutePrefix = "";
        });

        return app;
    }
}
