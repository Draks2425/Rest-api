using FirstAPI.Data;
using FirstAPI.Repositories; // <--- Make sure to add this using directive
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();