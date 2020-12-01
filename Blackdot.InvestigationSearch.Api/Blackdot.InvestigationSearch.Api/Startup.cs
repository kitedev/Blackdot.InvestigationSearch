using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using Blackdot.InvestigationSearch.Queries.Handlers;
using Blackdot.InvestigationSearch.SearchEngines;
using Blackdot.InvestigationSearch.Core;
using Blackdot.InvestigationSearch.SearchEngines.Interfaces;
using FluentValidation;
using Blackdot.InvestigationSearch.Validators;
using Blackdot.InvestigationSearch.Api.Wrapper;
using System;
using System.IO;

namespace Blackdot.InvestigationSearch.Api
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
            services.AddControllers();
            services.AddCors();
            services.AddMediatR(Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(GetSearchResultsQueryHandler)));

            services.AddSingleton<IQueryStringEncoder, QueryStringEncoder>();
            services.AddSingleton<IAngleSharpHtmlParser, AngleSharpHtmlParser>();
            services.AddSingleton<IValidator<string>, SearchTermValidator>();
            services.AddSingleton<Microsoft.Extensions.Caching.Memory.IMemoryCache, Microsoft.Extensions.Caching.Memory.MemoryCache>();
            services.AddSingleton<ICacheWrapper, CacheWrapper>();

            var provider = services.BuildServiceProvider();
            var htmlParser = provider.GetService<IAngleSharpHtmlParser>();
            var queryStringEncoder = provider.GetService<IQueryStringEncoder>();

            var bingSelector = new BingSearchEngineSelector(Configuration.GetValue<string>("BingUrlSelector"),
                                                                    Configuration.GetValue<string>("BingTitleSelector"),
                                                                    Configuration.GetValue<string>("BingCaptionSelector"));

            var yahooSelector = new YahooSearchEngineSelector(Configuration.GetValue<string>("YahooUrlSelector"),
                                                        Configuration.GetValue<string>("YahooTitleSelector"),
                                                        Configuration.GetValue<string>("YahooCaptionSelector"));

            // Bing search
            services.AddSingleton<ISearchEngine>(new SearchEngine(htmlParser,
                                                                  bingSelector,
                                                                  queryStringEncoder,
                                                                  Configuration.GetValue<string>("BingUrl"),
                                                                  Configuration.GetValue<string>("BingSearchResultSelector")
                                                                  ));

            // Yahoo search
            services.AddSingleton<ISearchEngine>(new SearchEngine(htmlParser,
                                                                  yahooSelector,
                                                                  queryStringEncoder,
                                                                  Configuration.GetValue<string>("YahooUrl"),
                                                                  Configuration.GetValue<string>("YahooSearchResultSelector")));


            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Blackdot Search API",
                    Description = "A meta search engine for investigation searches",
                    Contact = new OpenApiContact
                    {
                        Name = "Lee Walton",
                        Email = "kite.developer@live.com",
                        Url = new Uri("http://kitedeveloper.blogspot.com/"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
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

            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseSwagger();

            //https://localhost:44375/swagger/index.html
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blackdot Search API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
