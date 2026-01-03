using Application;

namespace Api.Models;

public record PaymentRequest(
    PaymentType Type,
    long Amount,
    string Currency,
    string Description
);

