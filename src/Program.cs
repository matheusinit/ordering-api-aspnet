using Microsoft.EntityFrameworkCore;
using OrderingApi.Controllers;
using OrderingApi.Data;
using OrderingApi.Config;

DotEnv.Configure();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(
    options =>
        options.UseSqlServer(
            $"Server={Env.DB_HOST},{Env.DB_PORT};User Id={Env.DB_USER};Database={Env.DB_NAME};Trusted_Connection=false;Password={Env.DB_PASSWORD};TrustServerCertificate=true;"
        )
);
builder.Services.AddScoped<CreateProductService, CreateProductService>();

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
