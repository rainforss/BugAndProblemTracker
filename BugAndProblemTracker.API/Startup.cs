using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BugAndProblemTracker.API.Attributes;
using BugAndProblemTracker.API.Contexts;
using BugAndProblemTracker.API.Models;
using BugAndProblemTracker.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BugAndProblemTracker.API
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
            services.AddAuthentication(o => { o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer(o => { o.Authority = $"https://{Configuration["Auth0:Domain"]}/";o.Audience = Configuration["Auth0:Audience"]; });

            ConventionRegistry.Register("Camel Case", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

            services.AddSingleton<IMongoClient>(s => new MongoClient(Configuration.GetConnectionString("MongoDBConnectionString")));

            services.AddScoped(s => new MongoDBContext(s.GetRequiredService<IMongoClient>(), Configuration["DbName"]));

            services.AddTransient<IBugService,BugService>();

            services.AddTransient<LanguageService>();

            services.AddTransient<FrameworkService>();

            services.AddTransient<LibraryService>();

            services.AddTransient<ErrorService>();

            services.AddControllers();

            services.AddSwaggerGen(c=> { c.SchemaFilter<SwaggerIgnoreFilter>(); c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Bugs and Problems Tracker", Description = "Never make the same mistake again. If you do, just look it up.", Contact = new OpenApiContact { Name = "Jake Chen", Email = "shengyan@ualberta.ca" } }); var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); c.RoutePrefix = string.Empty;  });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
