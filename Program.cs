using Microsoft.EntityFrameworkCore;
using ProjektMirjan.Context;
using ProjektMirjan.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<NbpApiService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddDbContext<CurrencyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjektMirjanDbContext")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyContext>();
    Console.WriteLine("DUPA");
    try
    {
        // Pr�ba po��czenia z baz� i wykonania prostego zapytania
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Po��czenie z baz� danych zosta�o nawi�zane.");
        }
        else
        {
            Console.WriteLine("Nie uda�o si� po��czy� z baz� danych.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"B��d po��czenia z baz� danych: {ex.Message}");
    }
}

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
