using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace TONBRAINS.QUANTON.Grpc
{
    public class Program
    {

       
        public static void Main(string[] args)
        {
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .WriteTo.LokigRPC("localhost:9095")
            //    .CreateLogger();

            //var webhost = Host
            //    .CreateDefaultBuilder(args)
            //    .UseSerilog()
            //    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            //    .Build();

            //webhost.Run();
            CreateHostBuilder(args).Build().Run();

  
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var requestCertStore = new X509Store("REQUEST", StoreLocation.LocalMachine);
            var host = Host.CreateDefaultBuilder(args);
            //host.UseSystemd();
           // host.UseSerilog();

            return host.ConfigureLogging(logging =>
            {
                logging.AddConsole();
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
#if !DEBUG
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Limits.MaxConcurrentConnections = 1000;
                        serverOptions.Limits.MaxConcurrentUpgradedConnections = 2000;
                        serverOptions.Limits.MaxRequestBodySize = 64 * 1024;
                        serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                        serverOptions.Limits.MinResponseDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                 

                        serverOptions.Listen(IPAddress.Parse("0.0.0.0"), 8000,
                            listenOptions =>
                            {
                                listenOptions.Protocols = HttpProtocols.Http2;
                                listenOptions.UseHttps(StoreName.Root, "tonbrains");
                            }
                        );
                        serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                        serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                    });
#endif


                    webBuilder.UseStartup<Startup>();
                });

        }


    }
}
