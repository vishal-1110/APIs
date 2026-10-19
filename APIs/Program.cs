
using APIs.Middleware;
using APIs.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Lifetime registrations
builder.Services.AddTransient<ITransientService, DIService>();
builder.Services.AddScoped<IScopedService, DIService>();
builder.Services.AddSingleton<ISingletonService, DIService>();

// Register the secondary consumer as Scoped so it lives for the HTTP request
builder.Services.AddScoped<DIReporterService>();

// Provided by FluentValidation.DependencyInjectionExtensions:
// Scans your assembly and registers all validators automatically
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Modern exception handling and RFC 7807 problem details
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Exception handler sits at the top to catch errors downstream
app.UseExceptionHandler();

// Custom performance and header tracking middleware
app.UseMiddleware<RequestExecutionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
