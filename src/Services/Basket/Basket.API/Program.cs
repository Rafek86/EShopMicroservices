using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

//Add Services to the container .


//Application Services 
builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));  
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));  
});



//Data Services 
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

//builder.Services.AddScoped<IBasketRepository>(prv =>
//{
//    var basketRepository = prv.GetRequiredService<BasketRepository>();
//    return new CachedBasketRepository(basketRepository, prv.GetRequiredService<IDistributedCache>());
//});

//Grpc Services 
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    options=> {
        options.Address = new Uri(builder.Configuration["GrpcSetting:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() => 
{
    var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback=
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
    return handler;
    });



// Cross-Cutting Services 
builder.Services.AddExceptionHandler<CustomExceptionHandler>();


builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(opts =>
{

});

app.UseHealthChecks("/health",
    new HealthCheckOptions { 
    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
