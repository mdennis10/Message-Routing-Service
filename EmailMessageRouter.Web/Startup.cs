using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Util.Internal;
using AutoMapper;
using EmailMessageRouter.Data.Cache;
using EmailMessageRouter.Data.EntityModel;
using EmailMessageRouter.Data.Repositories;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Domain.Services;
using EmailMessageRouter.Processor.Actors;
using EmailMessageRouter.Processor.Model;
using EmailMessageRouter.Web.EntityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailMessageRouter.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg => 
            { 
                cfg.CreateMap<PostmarkEmail, Email>();
                cfg.CreateMap<EmailMessage, Email>();
            });
            var mapper = new Mapper(mapperConfig);
            services.AddSingleton(x => mapper);

            // Loads configuration from appsetting.json file.
            // CI build tool will override this file for each environment.
            var maxBatchSize = Configuration.GetValue<int>("MaxBatchEmails");
            var cacheLifetime = Configuration.GetValue<int>("CacheLifetime");
            var validationRulesSettings = new Dictionary<string, bool>();
            Configuration
                .GetSection("ValidationRules")
                .GetChildren()
                .ForEach(x => validationRulesSettings.Add(x.Key, bool.Parse(x.Value)));

            var handlersSettings = new Dictionary<string, bool>();
            Configuration
                .GetSection("BusinessEvaluationHandlers")
                .GetChildren()
                .ForEach(x => handlersSettings.Add(x.Key, bool.Parse( x.Value)));

            
            // All dependencies required by actor system are
            // instantiate and inserted here for the purposes of
            // this assessment. The parent MessageRoutingManagerActor 
            // would have its internal DI framework to handle injecting
            // its dependencies. Configuration from appsettings.json would
            // still passed to actor from here.
            var accountRepository = new AccountRepositoryImpl();
            var actorSystem = ActorSystem.Create("EmailRoutingActorSystem");
            services.AddSingleton(x => actorSystem);
            services.AddSingleton(x => 
                actorSystem.ActorOf(
                    MessageRoutingManagerActor.Props(
                        new MessageRoutingServiceImpl(
                            accountRepository, 
                            new MessageRequestRepositoryImpl(), 
                            new InMemoryCache<string, Account>(cacheLifetime)
                        ), 
                        new EmailDeliveryServiceImpl(),
                        accountRepository, mapper, 
                        maxBatchSize, validationRulesSettings, handlersSettings
                    ),
                    typeof(MessageRoutingManagerActor).Name
                )
            );
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            // Code to register instance with Consul Agent 
            // would go here.
        }
    }
}