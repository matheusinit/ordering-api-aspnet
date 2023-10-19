using Microsoft.EntityFrameworkCore;
using OrderingApi.Data;
using OrderingApi.Config;
using OrderingApi.Producers;

DotEnv.Configure();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<OrderingProducer, OrderingKafkaProducer>();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseSqlServer(DatabaseConnection.GetConnectionString())
);

// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureAppConfiguration(
    (ctx, builder) =>
    {
        builder.AddEnvironmentVariables();
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
