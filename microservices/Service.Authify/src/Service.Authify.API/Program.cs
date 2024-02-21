using Microsoft.EntityFrameworkCore;
using Service.Authify.Data.Helpers;
using Service.Authify.Data.PostgreSql.Context;
using Service.Authify.Data.PostgreSql.Repository;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PSQL"), action => { action.CommandTimeout(30); });
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<GenerateClaimsHelper>(); 
builder.Services.AddScoped<GenerateKeyHelper>(); 
builder.Services.AddScoped<GenerateTokenHelper>();
builder.Services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
builder.Services.AddScoped<IUserCredentialManager, UserCredentialManager>();
builder.Services.AddScoped<IUserCredentialProvider, UserCredentialProvider>();

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