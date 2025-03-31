using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.MassTransit
{
  public static class Extensions
    {

        public static IServiceCollection AddMessageBroker
            (this IServiceCollection services,IConfiguration configuration, Assembly? assembly=null) 
        {

            //Impelement RabbitMQ MassTransit configuration  
            services.AddMassTransit(config => { 

            config.SetKebabCaseEndpointNameFormatter();

                if(assembly !=null)
                {
                    config.AddConsumers(assembly);
                }

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(configuration["MessageBroker:Host"], host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]);
                        host.Password(configuration["MessageBroker:Password"]);
                    });
                    configurator.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
