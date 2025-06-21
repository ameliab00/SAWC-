using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SAW.DTO.Event;
using SAW.Infrastructure;
using SAW.Mappers;
using SAW.Models;
using SAW.Services;
using SAW.Repositories;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = Path.Combine("..", "saw-frontend", "dist") 
});

var configuration = builder.Configuration;


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


builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<EventRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<TicketRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<TicketService>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "..", "saw-frontend", "dist")),
    RequestPath = ""
});


app.UseRouting();

// Autoryzacja:
// app.UseAuthentication();
// app.UseAuthorization();


app.MapControllers();


app.MapFallbackToFile("index.html");


if (builder.Configuration.GetValue<bool>("Database:AutoMigrate"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated(); 
}

app.Run();
