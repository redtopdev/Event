// <copyright file="Startup.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Engaze.Core.Persistance.Cassandra;
using Engaze.Core.Web;
using Engaze.Event.ApplicationService.Core.Dispatcher;
using Engaze.Event.ApplicationService.Handler;
using Engaze.Event.ApplicationService.Query;
using Engaze.Event.DataPersistence;
using Engaze.Event.DataPersistence.Cassandra;
using Engaze.Event.DataPersistence.EventStore;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PushNotificationHelper;

namespace Engaze.Event.Service
{
    public class Startup : EngazeStartup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureComponentServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            if (Configuration.GetValue<bool>("UseEventStore"))
            {
                var connString = Configuration.GetValue<string>("EVENTSTORE_CONNSTRING");
                services.AddSingleton(x => EventStoreConnection.Create(new Uri(connString)));
                services.AddSingleton<IEventStore, EventStoreEventStore>();
                services.AddSingleton<IAggregateRespository<Domain.Entity.Evento>, AggregateRespository<Domain.Entity.Evento>>();
                services.AddSingleton<INotificationManager, NotificationManager>();
                services.AddSingleton(x =>
                {
                    ICommandDispatcher dispatcher = new CommandDispatcher();
                    dispatcher.Register(new EventoCommandHandler(x.GetService<IAggregateRespository<Domain.Entity.Evento>>()));
                    return dispatcher;
                });
            }
            else
            {
                services.ConfigureCloudCassandra(Configuration);
                services.AddSingleton<IEventCommandRepository, EventCommandRepository>();
                services.AddSingleton<IEventQueryRepository, EventQueryRepository>();
                services.AddSingleton<INotificationManager, NotificationManager>();
                services.AddSingleton(x =>
                {
                    ICommandDispatcher dispatcher = new CommandDispatcher();
                    dispatcher.Register(new EventoCommandHandlerNoEventSourcing(x.GetService<IEventCommandRepository>(), x.GetService<INotificationManager>()));
                    return dispatcher;
                });

                services.AddSingleton<IEventQueryManager, EventQueryManager>();
            }
        }

        public override void ConfigureComponent(IApplicationBuilder app)
        {
            if (Configuration.GetValue<bool>("UseEventStore"))
            {
            }
        }
    }
}
