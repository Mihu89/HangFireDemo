using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using HangFireDemo.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HangFireDemo
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
            services.AddControllersWithViews();
            services.AddHangfire(configuration =>
            {
                string connStr = Configuration["HangFire:HangConnection"];
                configuration.UseSqlServerStorage(connStr, new Hangfire.SqlServer.SqlServerStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(1),
                    PrepareSchemaIfNecessary = false,
                    SchemaName = "Hangfire"
                });
            });

            // HangFire Jobs
            services.AddScoped<IDemoJob, DemoJob>();
            services.AddScoped<BackgroundJobClient>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            var hangFireOptions = new BackgroundJobServerOptions
            {
                WorkerCount = Environment.ProcessorCount * 1
            };

            app.UseHangfireServer(hangFireOptions);
            app.UseHangfireDashboard();

            BackgroundJobClient backgroundJobs = new BackgroundJobClient();
            backgroundJobs.Enqueue(() => Console.WriteLine("My First Job using HangFire"));


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHangfireDashboard();
            });

        }
    }
}
