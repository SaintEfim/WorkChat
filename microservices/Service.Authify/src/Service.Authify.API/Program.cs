using Microsoft.EntityFrameworkCore;
using Service.Authify.API;
using Service.Authify.API.Helpers;
using Service.Authify.Data.PostgreSql.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapperFromAllAssemblies();
builder.Services.AddControllers();
builder.Services.AddDependencyInjection();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
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
app.UseRouting();
app.MapControllers();
app.Run();