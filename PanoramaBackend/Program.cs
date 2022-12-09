using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Net;

namespace PanoramaBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureKestrel(serverOptions =>
            {

                //serverOptions.Listen(IPAddress.Parse("192.168.18.7"), 5000);
                //serverOptions.Limits.MaxConcurrentConnections = 100;
                //serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                //serverOptions.Limits.MaxRequestBodySize = 10 * 1024;
                //serverOptions.Limits.MinRequestBodyDataRate =
                //    new MinDataRate(bytesPerSecond: 100,
                //        gracePeriod: TimeSpan.FromSeconds(10));
                //serverOptions.Limits.MinResponseDataRate =
                //    new MinDataRate(bytesPerSecond: 100,
                //        gracePeriod: TimeSpan.FromSeconds(10));

                //serverOptions.Listen(IPAddress.Loopback, 5001,
                //    listenOptions =>
                //    {
                //        listenOptions.UseHttps("testCert.pfx",
                //            "testPassword");
                //    });
                //webBuilder.UseUrls("http://192.168.100.38:5000/");
            })
            .UseStartup<Startup>();
        });
               
    }
}
