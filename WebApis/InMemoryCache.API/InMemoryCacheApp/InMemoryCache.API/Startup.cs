using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace InMemoryCache.API;

public class Startup
{
    private const string _swaggerDocVersion = "v1";
    private const string _swaggerDocTitle = "InMemoryCache.API";

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddMemoryCache();

        #region SWAGGER

        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc(_swaggerDocVersion,
                new OpenApiInfo { Title = _swaggerDocTitle, Version = _swaggerDocVersion });
        });

        #endregion
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

        #region SWAGGER

        app.UseSwagger();
        app.UseSwaggerUI(x => { x.SwaggerEndpoint("v1/swagger.json", $"{_swaggerDocTitle} {_swaggerDocVersion}"); });

        #endregion

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}