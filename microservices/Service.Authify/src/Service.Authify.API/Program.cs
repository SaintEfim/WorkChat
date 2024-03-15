using Microsoft.EntityFrameworkCore;
using Service.Authify.API;
using Service.Authify.API.Helpers;
using Service.Authify.Data.PostgreSql.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapperFromAllAssemblies();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDependencyInjection();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<UserCredentialDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PSQL"), action => { action.CommandTimeout(30); });
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseRouting();
app.MapControllers();
app.Run();