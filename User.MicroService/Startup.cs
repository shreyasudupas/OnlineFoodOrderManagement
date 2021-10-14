using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Identity.MicroService.Extensions;
using Identity.MicroService.Installers;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Identity.MicroService.Data.SeedData;

namespace Identity.MicroService
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
            services.AddCors();
            services.InstallServiceAssembly(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.MicroService v1"));

                //Seeding data for first time
                MigrationSeedData.InitialDataSeed(app,Configuration.GetConnectionString("DBConnectionString1"));
            }

            app.UseHttpsRedirection();

            //app.UseHealthChecks("/heath", new HealthCheckOptions
            //{
            //    //ResponseWriter = async (context,report) =>
            //    //{
            //    //    context.Response.ContentType = "application/json";
            //    //    var response = new HealthCheckResponse
            //    //    {
            //    //        Status = report.Status.ToString(),
            //    //        Checks = report.Entries.Select(entry => new HealthCheck
            //    //        {
            //    //            Status = entry.Key,
            //    //            Component = entry.Value.Status.ToString(),
            //    //            Description = entry.Value.Description
            //    //        }),
            //    //        Duration = report.TotalDuration
            //    //    };

            //    //    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            //    //}
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //}) ;


            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });

            app.UseRouting();

            app.UseAuthentication();

            //middleware
            app.CallRequestLoggingMiddleware();
            app.ConfigureCustomExceptionMiddleware();
            app.ConfigureAuthoriziationMiddleware();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapControllers();
            });
        }

    }
}
