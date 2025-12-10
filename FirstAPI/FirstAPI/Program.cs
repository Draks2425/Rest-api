using FirstAPI.Data;
using FirstAPI.Repositories; // <--- Make sure to add this using directive
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ensure the app listens on the same URLs used in launchSettings and the .http file.
// This makes `http://localhost:5191` available when running with `dotnet run`.
builder.WebHost.UseUrls("http://localhost:5191", "https://localhost:7285");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<FirstAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// === REGISTRATION START ===
// Register the Repository in the Dependency Injection container.
// This tells the app: "Whenever a controller asks for IBookRepository, give them BookRepository"
builder.Services.AddScoped<IBookRepository, BookRepository>();
// === REGISTRATION END ===

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Do not force HTTPS in development so HTTP requests to localhost:5191 work from REST clients.
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();