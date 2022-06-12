using Microsoft.EntityFrameworkCore;
using OpenLadle.Data;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerContext) =>
{
    loggerContext.ReadFrom.Configuration(hostingContext.Configuration) 
        .WriteTo.Console();
});

builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
{
    dbContextOptions.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        mySqlBuilder =>
        {
            mySqlBuilder.MigrationsAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext)).GetName().Name);
        });
});

builder.Services.AddOpenLadle();

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
