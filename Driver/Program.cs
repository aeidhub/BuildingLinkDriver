using BuildingLink_Driver.Data;
using BuildingLinkDriver.Data;
using BuildingLinkDriver.Interfaces;
using BuildingLinkDriver.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SQLite;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddLogging();

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<IDriverRepository>(_ => new DriverRepository(new SQLiteConnection(connectionString)));
builder.Services.AddTransient<IDriverService, DriverService>();
builder.Services.AddTransient<IOperationService, OperationService>();

// If database and drivers tables not exist, create them and add a seed user
new Database(new SQLiteConnection(connectionString)).CreateTableAndDatabaseIfNotExist();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
