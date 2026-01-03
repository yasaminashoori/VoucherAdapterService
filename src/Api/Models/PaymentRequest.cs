using Application;
using Domain;

namespace Api.Models;

public record PaymentRequest(
    PaymentType Type,
    long Amount,
    Currency Currency,
    string? Description = null
);

