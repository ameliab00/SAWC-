using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAW.DTO.Event;
using SAW.Infrastructure;
using SAW.Mappers;
using SAW.Models;
using SAW.Services;
using SAW.Repositories;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

// Rejestracja DbContext dla MySQL
var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<ApplicationDbContext>(s => new ApplicationDbContext(optionBuilder.Options));
builder.Services.AddEntityFrameworkMySql();

builder.Services.AddScoped<UpdateEventMapper>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.CreateMap<UpdateEventRequest, Event>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddHttpContextAccessor();

// Rejestracja repozytoriów
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<EventRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<TicketRepository>();

// Rejestracja serwisów
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<TicketService>();

// Rejestracja dodatkowych konfiguracji i usług
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Obsługa wyjątków
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

// Obsługa plików statycznych
app.UseStaticFiles();

// Włączamy routing
app.UseRouting();

// Autoryzacja i autentykacja
// app.UseAuthentication();
// app.UseAuthorization();

// Konfiguracja routingu dla MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Obsługuje plik index.html z katalogu wwwroot
app.MapGet("/", async context =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
    if (File.Exists(filePath))
    {
        var htmlContent = await File.ReadAllTextAsync(filePath);
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(htmlContent);
    }
    else
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Strona główna nie została znaleziona.");
    }
});

// AutoMigrate 
if (builder.Configuration.GetValue<bool>("Database:AutoMigrate"))
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureCreated(); // Automatyczne stosowanie migracji
    }
}


app.Run();


