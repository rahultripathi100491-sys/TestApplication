using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestApplication.Application.Email.Services;
using TestApplication.Repository.Services;

namespace TestApplication.Repository.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(TestApplication.Application.Email.Commands.SendEmailCommand).Assembly));

        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
