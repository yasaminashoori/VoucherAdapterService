using Application;
using Application.Validators;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services)
    {
        services.AddScoped(_ => new BankAdaptee("1234567890"));
        services.AddScoped(_ => new ChequeAdaptee("CHQ-12345", DateTime.Now.AddDays(30)));
        services.AddScoped(_ => new CashAdaptee("REG-001"));

        services.AddScoped<ITarget, BankAdapter>();
        services.AddScoped<ITarget, ChequeAdapter>();
        services.AddScoped<ITarget, CashAdapter>();

        services.AddScoped<PaymentService>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IPaymentRequestValidator, PaymentRequestValidator>();

        return services;
    }
}
