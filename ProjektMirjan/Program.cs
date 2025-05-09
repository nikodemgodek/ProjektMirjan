using Microsoft.EntityFrameworkCore;
using ProjektMirjan.Context;
using ProjektMirjan.Interfaces;
using ProjektMirjan.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddScoped<CurrencyRateService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CurrencyContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSingleton<INbpApiService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var baseUrl = config.GetValue<string>("NbpApi:BaseUrl");
    return new NbpApiService(baseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
