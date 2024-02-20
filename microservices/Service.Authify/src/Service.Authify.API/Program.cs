using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Authify.Data.PostgreSql.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
// IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PSQL"), action => { action.CommandTimeout(30); });
    options.EnableDetailedErrors(true);
    options.EnableSensitiveDataLogging(true);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();