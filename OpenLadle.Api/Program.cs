using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerContext) =>
{
    loggerContext.ReadFrom.Configuration(hostingContext.Configuration) 
        .WriteTo.Console();
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddOpenLadle(openLadleOptions =>
{
    openLadleOptions.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
});

builder.Services.AddControllers();

builder.Services.AddRouting(routingOptions =>
{
    routingOptions.LowercaseUrls = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
