using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.API;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);


//Add Service to container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);


var app = builder.Build();

//Configure the Http Request pipeline 
    
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();
