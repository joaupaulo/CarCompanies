using CarCompanies.Domain;
using CarCompanies.Repository;
using CarCompanies.Repository.Interface;
using CarCompanies.Service;
using CarCompanies.Service.Interface;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IVehicle, VehicleService>();
builder.Services.AddTransient<IRepositoryBase, RepositoryBase>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddScoped(typeof(IBsonFilter<>),typeof(BsonFilter<>));
builder.Services.AddTransient<IEventBsonFilter, EventBsonFilter>();
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