using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace StoreWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .UseIISIntegration()
                    .UseKestrel()
                    .UseContentRoot(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        IHostingEnvironment env = hostingContext.HostingEnvironment;
                        config.SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));

                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    })
                .UseStartup<Startup>()
                .Build();
    }
}
