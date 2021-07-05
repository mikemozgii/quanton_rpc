using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TONBRAINS.QUANTON.Core.Handlers;
using TONBRAINS.QUANTON.Core.Interfaces;
using TONBRAINS.QUANTON.Core.Services;

namespace TONBRAINS.QUANTON.Grpc
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            KataHelpers.ConnectionString = GlobalAppConfHdlr.QunatonDbConnectionString;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddGrpc(options => options.Interceptors.Add(typeof(SessionInterceptor)));

            services.AddTransient<IActionLogSvc, ActionLogSvc>();
            services.AddTransient<IAccountSvc, AccountSvc>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<TonMobileService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Success");
                });
            });
        }
    }
}
