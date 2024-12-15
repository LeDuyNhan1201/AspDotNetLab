using Application;
using Infrastructure.Postresql.Data;
using Infrastructure.Postresql.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Presentation.Configurations.Cookie;
using Presentation.Configurations.Localization;
using Presentation.Configurations.Serilog;
using Presentation.Configurations.ValidationHandler;
using Presentation.Configurations.Web;
using Presentation.Mappers;
using Presentation.Swagger;
using Serilog;
using System;

namespace Presentation.Configurations;

internal static class HostingExtensions
{

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext();

        builder.Services.AddRepositories();

        builder.Services.AddMappers();

        //builder.Services.AddServices();

        builder.Services.AddLocalization(builder.Configuration);

        builder.Services.AddControllersWithViews();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerPreConfigured(options =>
        {
            builder.Configuration.GetSection("Swagger").Bind(options);
        });

        var serviceProvider = builder.Services.BuildServiceProvider();
        var localizer = serviceProvider.GetRequiredService<IStringLocalizer<SharedResource>>();

        builder.Services.AddCookieAuthenticaton();

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddValidationHanlder(builder.Configuration, localizer);

        builder.AddCommonSerilog();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseLocalization();

        app.UseGlobalExceptionHandler();

        app.UseSwaggerPreConfigured();

        app.UsePathBase(configuration["Server:BasePath"]);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.AutoMigration();

        app.Services.SeedEssentialData(configuration);

        app.MapControllers();

        app.MapControllerRoute(
            name: "User",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "Admin",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        return app;
    }

}
