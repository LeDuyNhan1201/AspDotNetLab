using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Presentation.Configurations.Serilog;

public static class SerilogConfiguration
{
    public static void AddCommonSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
        builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
    }
}
