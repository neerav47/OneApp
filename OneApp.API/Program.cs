using Microsoft.EntityFrameworkCore;
using OneApp.Data.Context;
using OneApp.Data.Interfaces;
using OneApp.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!);
});
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

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

