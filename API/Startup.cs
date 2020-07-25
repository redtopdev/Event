// <copyright file="Startup.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Core.Persistance.Cassandra;
using Engaze.Core.Web;
using Engaze.EventSourcing.Core;
using Evento.ApplicationService.Handler;
using Evento.DataPersistance;
using Evento.DataPersistance.Cassandra;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evento.Service
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
            if (Configuration.GetValue<bool>("UseEventStore"))
            {
                var connString = Configuration.GetValue<string>("EVENTSTORE_CONNSTRING");
                services.AddSingleton(x => EventStoreConnection.Create(new Uri(connString)));
                services.AddSingleton<IEventStore, EventStoreEventStore>();
                services.AddSingleton<IAggregateRespository<Domain.Entity.Evento>, AggregateRespository<Domain.Entity.Evento>>();
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
                services.AddSingleton(x =>
                {
                    ICommandDispatcher dispatcher = new CommandDispatcher();
                    dispatcher.Register(new EventoCommandHandlerNoEventSourcing(x.GetService<IEventCommandRepository>()));
                    return dispatcher;
                });
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
