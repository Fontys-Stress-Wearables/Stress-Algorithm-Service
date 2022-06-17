using StressAlgorithmService.Controllers;
using StressAlgorithmService.Interfaces;
using StressAlgorithmService.Logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<INatsService, NatsService>();
builder.Services.AddSingleton<IHRVAlgorithm, HRVAlgorithm>();
builder.Services.AddSingleton<UnprocessedStressDataService>();
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

app.Services.GetServices<INatsService>();
app.Services.GetServices<UnprocessedStressDataService>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
