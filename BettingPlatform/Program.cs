using BettingPlatform.BLL.Contracts;
using BettingPlatform.BLL.Services;
using BettingPlatform.DAL;
using BettingPlatform.DAL.Entities;
using BettingPlatform.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDbContext<SportsDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddSingleton<UpdateManager>();
builder.Services.AddScoped<UpdateConsumer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IHostedService>(provider =>
        new PeriodicRequestService(provider.GetRequiredService<HttpClient>(),
        provider.GetRequiredService<IServiceProvider>(),
        provider.GetRequiredService<IMemoryCache>(),
        provider.GetRequiredService<UpdateManager>()));

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

using var serviceScope = app.Services.CreateScope();

var serviceProvider = serviceScope.ServiceProvider;

var db = serviceProvider.GetRequiredService<SportsDbContext>();

db.Database.EnsureCreated();

app.Run();
