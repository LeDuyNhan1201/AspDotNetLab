using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace Presentation.Configurations.ValidationHandler;

public static class ValidationHandlerConfiguration
{

    public static void AddValidationHanlder(this IServiceCollection services, IConfiguration configuration, IStringLocalizer stringLocalizer)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0) // Kiểm tra null
                    .ToDictionary(
                        kvp =>
                        {
                            string keyString = kvp.Key.Split('.').Last();
                            return LowercaseFirstChar(keyString);
                        },
                        kvp => kvp.Value!.Errors
                            .Select(e => stringLocalizer[e.ErrorMessage]?.ToString() ?? e.ErrorMessage) // Xử lý fallback nếu không tìm thấy trong stringLocalizer
                    );

                AppError error = AppError.VALIDATION_ERROR;
                string type = error.Type.ToString();
                string message = stringLocalizer[error.Code].ToString();

                return new BadRequestObjectResult(new
                {
                    type,
                    message,
                    errors
                });

            };
        });
    }

    private static string LowercaseFirstChar(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
            return input;

        return char.ToLower(input[0]) + input.Substring(1);
    }

}
