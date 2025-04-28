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
        // Próba po³¹czenia z baz¹ i wykonania prostego zapytania
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Po³¹czenie z baz¹ danych zosta³o nawi¹zane.");
        }
        else
        {
            Console.WriteLine("Nie uda³o siê po³¹czyæ z baz¹ danych.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"B³¹d po³¹czenia z baz¹ danych: {ex.Message}");
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
