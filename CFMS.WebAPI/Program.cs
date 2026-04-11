using CFMS.Application;
using CFMS.Infrastructure;
using CFMS.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handler (must be early in pipeline)
app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
