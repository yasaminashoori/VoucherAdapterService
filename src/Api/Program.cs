using Application;
using Common.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<BankAdaptee>(_ => new BankAdaptee("1234567890"));
builder.Services.AddScoped<ChequeAdaptee>(_ => new ChequeAdaptee("CHQ-12345", DateTime.Now.AddDays(30)));
builder.Services.AddScoped<CashAdaptee>(_ => new CashAdaptee("REG-001"));

builder.Services.AddScoped<ITarget, BankAdapter>(sp => 
    new BankAdapter(sp.GetRequiredService<BankAdaptee>()));

builder.Services.AddScoped<ITarget, ChequeAdapter>(sp => 
    new ChequeAdapter(sp.GetRequiredService<ChequeAdaptee>()));

builder.Services.AddScoped<ITarget, CashAdapter>(sp => 
    new CashAdapter(sp.GetRequiredService<CashAdaptee>()));

builder.Services.AddScoped<PaymentService>(sp => 
    new PaymentService(sp.GetServices<ITarget>()));

var app = builder.Build();

// Add exception handling middleware early in the pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
